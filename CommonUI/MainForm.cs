using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using DataObjects;

namespace CommonUI
{
	public partial class MainForm : Form
	{
		#region Fields

		private StringList _imageList = null;

		#endregion

		#region Ctor

		public MainForm()
		{
			InitializeComponent();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Indexer
		/// </summary>
		public ImageForm this[int index]
		{
			get { return (ImageForm)MdiChildren[index]; }
		}

		/// <summary>
		/// Determine whether an image is selected (false if no image).
		/// </summary>
		public bool IsImageActive
		{
			get { return (ActiveMdiChild is ImageForm); }
		}

		/// <summary>
		/// Return active image, or null.
		/// </summary>
		public ImageForm ActiveImage
		{
			get { return (ActiveMdiChild as ImageForm); }
		}

		#endregion

		#region Form operations

		/// <summary>
		/// Display confirmation dialog box on closing.
		/// </summary>
		/// <returns>True if user choose to exit.</returns>
		private bool ConfirmClose()
		{
			CustomMessageBox messageBox = new CustomMessageBox(
				"Are you sure you want to exit?", "Confirm Exit",
				CustomMessageBox.CustomMessageBoxButtons.ExitCancel
			);
			CustomMessageBox.CustomDialogResult result = messageBox.ShowCustomDialog(this);
			if (result == CustomMessageBox.CustomDialogResult.Exit)
				return true;
			else
				return false;
		}

		/// <summary>
		/// Apply form window settings stored in registry.
		/// </summary>
		private void LoadWindowSettings()
		{
			int maximized = RegistryManager.LoadValue("Maximized");

			if (maximized > 0)
			{
				this.WindowState = FormWindowState.Maximized;
			}
			else
			{
				int x = RegistryManager.LoadValue("WindowLocationX");
				int y = RegistryManager.LoadValue("WindowLocationY");
				if (x > 0 && y > 0)	// force location to be in visible area
					this.Location = new Point(x, y);
				else
					this.Location = Point.Empty;

				int width = RegistryManager.LoadValue("WindowSizeWidth");
				int height = RegistryManager.LoadValue("WindowSizeHeight");
				if (width > 0 && height > 0)
					this.Size = new Size(width, height);
			}
		}

		void OnImageActivated(string imagePath, string imageTitle)
		{
			UpdateStatusDimension(ActiveImage.Image.Size);

			UpdateStatusZoom(ActiveImage.ImageZoom);
		}

		/// <summary>
		/// When application is idle, handle update UI elements.
		/// </summary>
		void OnApplicationIdle(object sender, EventArgs e)
		{
			UpdateInterface();
		}

		private void OnFormClosing(object sender, FormClosingEventArgs e)
		{
			if (!ConfirmClose())	// if user cancels
			{
				e.Cancel = true;
				return;
			}

			// Save application settings
			SaveWindowSettings();
		}

		/// <summary>
		/// When a new image is opened, update image collection.
		/// </summary>
		void OnImageOpened(string imagePath, string imageTitle)
		{
			_imageList.Add(imagePath);
			//MessageBox.Show("Image " + imagePath + " opened.");

			// Update the list of recent files to include the latest image
			RegistryManager.StoreRecentFile(imagePath);

			// Show the updated list
			ShowRecentFiles();
		}

		/// <summary>
		/// When an image is closed, update image collection.
		/// </summary>
		void OnImageClosed(string imagePath, string imageTitle)
		{
			_imageList.Remove(imagePath);

			// Reset status bar
			UpdateStatusDimension(Size.Empty);
			UpdateStatusZoom(0f);
		}

		/// <summary>
		/// When the zoom factor is changed for the active image.
		/// </summary>
		void OnImageZoomed(string imagePath, string imageTitle, float zoom)
		{
			UpdateStatusZoom(zoom);
		}

		private void OnLoad(object sender, EventArgs e)
		{
			// Don't load if in Designer mode
			if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv")
				return;

			_imageList = new StringList();

			// Handle update UI when idle
			Application.Idle += new EventHandler(OnApplicationIdle);

			// Hook up event handler
			OperationsManager.ImageOpened += new ImageOpenedEventHandler(OnImageOpened);
			OperationsManager.ImageClosed += new ImageClosedEventHandler(OnImageClosed);
			OperationsManager.ImageActivated += new ImageActivatedEventHandler(OnImageActivated);
			OperationsManager.ImageZoomed += new ImageZoomedEventHandler(OnImageZoomed);

			// Apply saved form settings
			LoadWindowSettings();

			// Turn on toolbar by default
			ToggleToolBar(true);

			// Show recent files
			ShowRecentFiles();

			// Set printing defaults
			SetDocumentPrintingDefaults();
		}

		/// <summary>
		/// Update status bar.
		/// </summary>
		private void UpdateStatusDimension(Size activeImageSize)
		{
			_statusStrip.ImageSize = activeImageSize;
		}

		/// <summary>
		/// Update status bar.
		/// </summary>
		private void UpdateStatusZoom(float activeImageZoom)
		{
			_statusStrip.CurrentZoom = activeImageZoom;
		}

		/// <summary>
		/// Save form window settings to registry.
		/// </summary>
		private void SaveWindowSettings()
		{
			// If maximized, save maximized state only so normal state will not have maxed size
			if (this.WindowState == FormWindowState.Maximized)
			{
				RegistryManager.StoreValue("Maximized", 1);
			}
			else
			{
				RegistryManager.StoreValue("Maximized", 0);
				RegistryManager.StoreValue("WindowLocationX", this.Location.X);
				RegistryManager.StoreValue("WindowLocationY", this.Location.Y);
				RegistryManager.StoreValue("WindowSizeWidth", this.Size.Width);
				RegistryManager.StoreValue("WindowSizeHeight", this.Size.Height);
			}
		}

		/// <summary>
		/// Refresh the list of recent files on the main form.
		/// </summary>
		private void ShowRecentFiles()
		{
			StringList fileList = RegistryManager.GetRecentFiles();

			if (fileList.Count == 0)
				return;

			// Create a list of menu items corresponding to the files
			List<ToolStripMenuItem> menuItemList = new List<ToolStripMenuItem>();
			int count = 0;
			string fileName;
			foreach (string path in fileList)
			{
				if (path == string.Empty)
					continue;

				fileName = Path.GetFileName(path);
				ToolStripMenuItem menuItem = new ToolStripMenuItem();
				menuItem.Text = string.Format("&{0} {1}", count, fileName);
				menuItem.Name = string.Format("_mnuFileOpenRecentFile{0}", count);
				menuItem.Tag = path;	// store path
				menuItem.Click += new EventHandler(OnFileOpenRecentFile);
				menuItemList.Add(menuItem);

				++count;
			}

			// TODO: Add logic to resize menu to fit longest file

			// Clear existing dropdown list and add new list
			_menuStrip.SuspendLayout();
			_mnuFileOpenRecent.DropDownItems.Clear();
			_mnuFileOpenRecent.DropDownItems.AddRange(menuItemList.ToArray());
			_menuStrip.ResumeLayout();

		}

		/// <summary>
		/// Enable or disable menu and toolbar depending whether 
		/// an image form is opened/selected.
		/// </summary>
		void UpdateInterface()
		{
			// Menu
			_mnuFileClose.Enabled = IsImageActive;
			_mnuFileCloseAll.Enabled = IsImageActive;
			_mnuFileSave.Enabled = IsImageActive;
			_mnuFileSaveAs.Enabled = IsImageActive;
			_mnuFilePageSetup.Enabled = IsImageActive;
			_mnuFilePrintPreview.Enabled = IsImageActive;
			_mnuFilePrint.Enabled = IsImageActive;

			_mnuEditRedo.Enabled = IsImageActive && ActiveImage.CanRedo;
			_mnuEditUndo.Enabled = IsImageActive && ActiveImage.CanUndo;

			// TODO: Add detection logic for selection and enable cut, copy, paste
			_mnuEditCopy.Enabled = _mnuEditCut.Enabled = _mnuEditPaste.Enabled = false;

			_mnuView.Enabled = IsImageActive;
			_mnuImage.Enabled = IsImageActive;
			_mnuFilter.Enabled = IsImageActive;

			_mnuWindowArrange.Enabled = IsImageActive;

			// Toolstrip
			if (!_toolStrip.Visible)
				return;

			_tlsSave.Enabled = _mnuFileSave.Enabled;
			_tlsPageSetup.Enabled = _tlsPrintPreview.Enabled = _tlsPrint.Enabled = _mnuFilePrint.Enabled;

			_tlsCopy.Enabled = _mnuEditCopy.Enabled;
			_tlsCut.Enabled = _mnuEditCut.Enabled;
			_tlsPaste.Enabled = _mnuEditPaste.Enabled;
			_tlsRedo.Enabled = _mnuEditRedo.Enabled;
			_tlsUndo.Enabled = _mnuEditUndo.Enabled;

			_tlsActualPixels.Enabled = _mnuView.Enabled;
			_tlsFitOnScreen.Enabled = _mnuView.Enabled;
			_tlsZoomIn.Enabled = _mnuView.Enabled;
			_tlsZoomOut.Enabled = _mnuView.Enabled;

			_tlsFlipHorizontal.Enabled = _mnuImage.Enabled;
			_tlsFlipVertical.Enabled = _mnuImage.Enabled;
		}

		#endregion

		#region Image handling

		private void LoadImage(string path)
		{
			// Skip an already-opened mage
			if (_imageList.Contains(path))
				return;

			// Open and display the image in a new ImageForm
			ImageForm imageForm = new ImageForm();
			imageForm.MdiParent = this;

			bool result = imageForm.OpenImage(path);

			if (!result)
			{
				CustomMessageBox messageBox = new CustomMessageBox(
					string.Format("Error opening image {0}", path),
					"Open Error",
					CustomMessageBox.CustomMessageBoxButtons.Ok
				);
				messageBox.ShowCustomDialog(this);
				return;
			}

			imageForm.Show();

			// Invoke ImageOpened event
			OperationsManager.RaiseImageOpenedEvent(path, Path.GetFileName(path));
		}

		internal bool SaveImage()
		{
			// If no active image
			if (!IsImageActive)
				return false;

			// If not modified
			if (!ActiveImage.IsImageModified)
				return true;

			// If image has no path on disk (i.e., created with Clone or New Image)
			if (ActiveImage.ImagePath == string.Empty)
			{
				return SaveImageAs();
			}

			bool result = ActiveImage.SaveImage();

			// Display error message if save is not successful
			if (!result)
			{
				CustomMessageBox messageBox = new CustomMessageBox(
					string.Format("Error saving image to {0}", ActiveImage.ImagePath),
					"Save Error",
					CustomMessageBox.CustomMessageBoxButtons.Ok
				);
				messageBox.ShowCustomDialog(this);
				return false;
			}

			return true;

		}

		internal bool SaveImageAs()
		{
			// If no active image
			if (!IsImageActive)
				return false;

			SaveFileDialog dlgSaveFile = new SaveFileDialog();
			dlgSaveFile.Title = "Save As";
			//saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			dlgSaveFile.Filter = "BMP (*.bmp)|*.bmp|JPEG (*.jpg)|*.jpg|PNG (*.png)|*.png|All Images|*.bmp;*.jpg;*.png| All Files (*.*)|*.*";
			dlgSaveFile.FilterIndex = 2;

			using (dlgSaveFile)
			{
				DialogResult dialogResult = dlgSaveFile.ShowDialog(this);	// automatically handle overwrite confirmation

				if (dialogResult != DialogResult.OK)
					return false;

				string newPath = dlgSaveFile.FileName;
				ImageFormat newFormat;

				// Disallow save as if the selected file is currently open
				if (_imageList.Contains(newPath))
				{
					CustomMessageBox messageBox = new CustomMessageBox(
						string.Format("Cannot save as {0} because that document is already open.", newPath),
						"Save Error",
						CustomMessageBox.CustomMessageBoxButtons.Ok
					);
					messageBox.ShowCustomDialog(this);
					return false;
				}

				// Determine selected image format based on the (1-based) filter index
				switch (dlgSaveFile.FilterIndex)
				{
					case 1:
						newFormat = ImageFormat.Bmp;
						break;
					case 2:
						newFormat = ImageFormat.Jpeg;
						break;
					case 3:
						newFormat = ImageFormat.Png;
						break;
					default:
						// If the user selects any other file type
						newFormat = ImageFormat.Jpeg;
						break;
				}

				bool result = ActiveImage.SaveImageAs(newPath, newFormat);

				// Display error message if save is not successful
				if (!result)
				{
					CustomMessageBox messageBox = new CustomMessageBox(
						string.Format("Error saving image to {0}", newPath),
						"Save Error",
						CustomMessageBox.CustomMessageBoxButtons.Ok
					);
					messageBox.ShowCustomDialog(this);
					return false;
				}

				return true;

			}
		}

		#endregion

		#region Menu

		#region File

		private void OnFileClose(object sender, EventArgs e)
		{
			if (IsImageActive)
				ActiveMdiChild.Close();
		}

		private void OnFileCloseAll(object sender, EventArgs e)
		{
			foreach (Form form in MdiChildren)
			{
				form.BringToFront();	// also try Activate()
				if (form is ImageForm)
					form.Close();
			}
		}

		private void OnFileExit(object sender, EventArgs e)
		{
			// Close all open images.  While Application.Exit will close all Mdi child windows,
			// it doesn't know if the user clicks Cancel on a Save confirmation dialog box for
			// a modified image.  So it's better to handle closing ourselves, then determine
			// whether all images are successfully closed.
			OnFileCloseAll(this, e);

			// If not all images closed; i.e., if the user cancels or if there is
			// an error while saving a modified image
			if (_imageList.Count > 0)
				return;

			Application.Exit();
		}

		private void OnFileNew(object sender, EventArgs e)
		{
			throw new NotSupportedException();
		}

		private void OnFileOpen(object sender, EventArgs e)
		{
			OpenFileDialog dlgFileOpen = new OpenFileDialog();
			dlgFileOpen.Title = "Open";
			//dlgFileOpen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			dlgFileOpen.Filter = "BMP (*.bmp)|*.bmp|JPEG (*.jpg)|*.jpg|PNG (*.png)|*.png|All Images|*.bmp;*.jpg;*.png| All Files (*.*)|*.*";
			dlgFileOpen.FilterIndex = 2;	// default to JPEG
			dlgFileOpen.RestoreDirectory = true;
			dlgFileOpen.Multiselect = true;

			using (dlgFileOpen)
			{
				DialogResult dialogResult = dlgFileOpen.ShowDialog(this);

				if (dialogResult != DialogResult.OK)
					return;

				// Load all selected files
				foreach (string path in dlgFileOpen.FileNames)
				{
					// Open and display the image
					LoadImage(path);
				}
			}
		}

		private void OnFileOpenRecentFile(object sender, EventArgs e)
		{
			ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
			string path = menuItem.Tag as string;

			// Open and display the selected file
			LoadImage(path);
		}

		private void OnFileSave(object sender, EventArgs e)
		{
			SaveImage();
		}

		protected void OnFileSaveAs(object sender, EventArgs e)
		{
			SaveImageAs();
		}

		#region Printing

		private void OnFilePageSetup(object sender, EventArgs e)
		{
			PageSetup(_printDocument);
		}

		private void OnFilePrintPreview(object sender, EventArgs e)
		{
			PrintPreview(_printDocument);
		}

		private void OnFilePrint(object sender, EventArgs e)
		{
			Print(_printDocument);
		}

		private void OnPrintPage(object sender, PrintPageEventArgs e)
		{
			ActiveImage.RenderForPrint(e.Graphics, e.PageSettings);
		}

		/// <summary>
		/// Allow user to specify page settings.
		/// </summary>
		public void PageSetup(PrintDocument document)
		{
			PageSetupDialog dlgPageSetup = new PageSetupDialog();
			dlgPageSetup.Document = document;

			// Retrieve current page settings
			dlgPageSetup.PageSettings.Landscape = document.DefaultPageSettings.Landscape;
			dlgPageSetup.PageSettings.Margins = document.DefaultPageSettings.Margins;

			// Show network printers
			dlgPageSetup.ShowNetwork = false;

			dlgPageSetup.ShowDialog(this.Parent);
		}

		/// <summary>
		/// Display print preview dialog.
		/// </summary>
		public void PrintPreview(PrintDocument document)
		{
			PrintPreviewDialog dlgPrintPreview = new PrintPreviewDialog();
			dlgPrintPreview.Document = document;

			dlgPrintPreview.StartPosition = FormStartPosition.CenterParent;
			dlgPrintPreview.WindowState = FormWindowState.Maximized;

			dlgPrintPreview.ShowDialog(this.Parent);
		}

		/// <summary>
		/// Display print dialog to start printing.
		/// </summary>
		public void Print(PrintDocument document)
		{
			PrintDialog dlgPrint = new PrintDialog();
			dlgPrint.Document = document;

			if (dlgPrint.ShowDialog(this.Parent) != DialogResult.OK)
				return;

			document.Print();
		}

		/// <summary>
		/// Set default print settings for document.
		/// </summary>
		private void SetDocumentPrintingDefaults()
		{
			// Select landscape as default mode
			_printDocument.DefaultPageSettings.Landscape = true;

			// Set default margins
			_printDocument.DefaultPageSettings.Margins = new Margins(100, 100, 100, 100);	// in 1/100 inch
		}

		#endregion

		#endregion

		#region Edit

		private void OnEditUndo(object sender, EventArgs e)
		{
			ActiveImage.Undo();
		}

		private void OnEditRedo(object sender, EventArgs e)
		{
			ActiveImage.Redo();
		}

		#endregion

		#region View

		private void OnViewActualPixels(object sender, EventArgs e)
		{
			ActiveImage.ActualPixels();
		}

		private void OnViewFitOnScreen(object sender, EventArgs e)
		{
			ActiveImage.FitOnScreen();
		}

		private void OnViewSetZoom(object sender, EventArgs e)
		{
			string zoomPercent = (sender as ToolStripMenuItem).Text;
			float zoom = int.Parse(zoomPercent.Replace("%", "")) / 100f;
			ActiveImage.SetZoom(zoom);
		}

		private void OnViewZoomIn(object sender, EventArgs e)
		{
			ActiveImage.ZoomIn();
		}

		private void OnViewZoomOut(object sender, EventArgs e)
		{
			ActiveImage.ZoomOut();
		}

		#endregion

		#region Image

		private void OnImageClone(object sender, EventArgs e)
		{
			// TODO: Re-work image cloning logic
			Bitmap cloneImage = OperationsManager.Clone(ActiveImage.Image);

			ImageForm cloneImageForm = new ImageForm();
			cloneImageForm.MdiParent = this;

			// Determine a unique title for the clone
			int i = 1;
			string baseTitle = "Untitled-";
			foreach (ImageForm imageForm in MdiChildren)
			{
				if (imageForm.ImageTitle.Contains(baseTitle))
					++i;
			}

			string newTitle = baseTitle + i.ToString();

			bool result = cloneImageForm.OpenImage(cloneImage, newTitle);

			if (!result)
			{
				CustomMessageBox messageBox = new CustomMessageBox(
					string.Format("Error cloning image {0}", ActiveImage.Text),
					"Clone Error",
					CustomMessageBox.CustomMessageBoxButtons.Ok
				);
				messageBox.ShowCustomDialog(this);
				return;
			}

			cloneImageForm.Show();

			// Do not invoke ImageOpened event as this is an empty path
			//FileManager.RaiseImageOpenedEvent(string.Empty);	// empty path
		}

		private void OnImageFlipHorizontal(object sender, EventArgs e)
		{
			ActiveImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
		}

		private void OnImageFlipVertical(object sender, EventArgs e)
		{
			ActiveImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
		}

		private void OnImageRotate180(object sender, EventArgs e)
		{
			ActiveImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
		}

		private void OnImageRotate90Cw(object sender, EventArgs e)
		{
			ActiveImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
		}

		private void OnImageRotate90Ccw(object sender, EventArgs e)
		{
			ActiveImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
		}

		#endregion

		#region Filter

		#region Adjustments

		private void OnFilterAdjustmentsBrightnessContrast(object sender, EventArgs e)
		{

		}

		private void OnFilterAdjustmentsGaussianBlur(object sender, EventArgs e)
		{

		}

		private void OnFilterAdjustmentsSharpen(object sender, EventArgs e)
		{

		}

		#endregion

		#region Color

		private void OnFilterColorGrayscale(object sender, EventArgs e)
		{
			ActiveImage.Grayscale();
		}

		private void OnFilterColorInvert(object sender, EventArgs e)
		{
			ActiveImage.InvertColors();
		}

		private void OnFilterColorRotate(object sender, EventArgs e)
		{
			ActiveImage.RotateColors();
		}

		private void OnFilterColorSepia(object sender, EventArgs e)
		{
			ActiveImage.Sepia();
		}

		#endregion

		#region Effects

		private void OnFilterEffectsJitter(object sender, EventArgs e)
		{

		}

		private void OnFilterEffectsPerlinNoise(object sender, EventArgs e)
		{

		}

		private void OnFilterEffectsOilPainting(object sender, EventArgs e)
		{

		}

		private void OnFilterEffectsPixellate(object sender, EventArgs e)
		{

		}

		#endregion

		#region Stylize

		private void OnFilterStylizeFindEdges(object sender, EventArgs e)
		{

		}

		#endregion

		#endregion

		#region Window

		private void OnWindowArrangeCascade(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.Cascade);
		}

		private void OnWindowArrangeIcons(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.ArrangeIcons);
		}

		private void OnWindowArrangeTileHorizontally(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.TileHorizontal);
		}

		private void OnWindowArrangeTileVertically(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.TileVertical);
		}

		private void OnWindowToggleStatusBar(object sender, EventArgs e)
		{
			ToggleStatusBar(!_statusStrip.Visible);
		}

		private void OnWindowToggleToolBar(object sender, EventArgs e)
		{
			ToggleToolBar(!_toolStrip.Visible);
		}

		private void ToggleStatusBar(bool bShowStatusBar)
		{
			_statusStrip.Visible = bShowStatusBar;
		}

		private void ToggleToolBar(bool bShowToolBar)
		{
			_toolStrip.Visible = bShowToolBar;
		}

		#endregion

		#region Help

		private void OnHelpAbout(object sender, EventArgs e)
		{
			ShowAboutBox();
		}

		private void ShowAboutBox()
		{
			AboutBox aboutBox = new AboutBox();
			using (aboutBox)
			{
				aboutBox.ShowDialog(this);
			}
		}

		#endregion

		#endregion

	}	// class
}	// namespace