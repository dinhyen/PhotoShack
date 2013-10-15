using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using DataObjects;

namespace CommonUI
{
	/// <summary>
	/// A MDI child form containing an image control.
	/// </summary>
	public partial class ImageForm : Form
	{
		#region Ctor

		public ImageForm()
		{
			InitializeComponent();
		}

		#endregion

		#region Properties

		public bool CanRedo
		{
			get { return _imageControl.CanRedo; }
		}

		public bool CanUndo
		{
			get { return _imageControl.CanUndo; }
		}

		public Bitmap Image
		{
			get { return _imageControl.Image; }
		}

		public bool IsImageModified
		{
			get { return _imageControl.IsModified; }
		}

		public string ImagePath
		{
			get { return _imageControl.Path; }
		}

		public string ImageTitle
		{
			get { return _imageControl.Title; }
		}

		public float ImageZoom
		{
			get { return _imageControl.Zoom; }
		}

		#endregion

		#region Context menu

		private void OnContextActualPixels(object sender, EventArgs e)
		{
			ActualPixels();
		}

		private void OnContextFitOnScreen(object sender, EventArgs e)
		{
			FitOnScreen();
		}

		private void OnContextZoomIn(object sender, EventArgs e)
		{
			ZoomIn();
		}

		private void OnContextZoomOut(object sender, EventArgs e)
		{
			ZoomOut();
		}

		#endregion

		#region Form operations

		private void OnActivated(object sender, EventArgs e)
		{
			OperationsManager.RaiseImageActivatedEvent(ImagePath, ImageTitle);
		}

		private void OnLoad(object sender, EventArgs e)
		{
			SetFormTitle();

			OperationsManager.ImageModified += new ImageModifiedEventHandler(OnImageModified);
		}

		private void OnFormClosing(object sender, FormClosingEventArgs e)
		{
			// If image is not modified, simply proceed with closing
			if (!IsImageModified)
				return;

			if (!ConfirmClose())	// if user cancels
			{
				e.Cancel = true;
				return;
			}
		}

		private void OnFormClosed(object sender, FormClosedEventArgs e)
		{
			// Raise ImageClosed event
			OperationsManager.RaiseImageClosedEvent(ImagePath, ImageTitle);
		}

		void OnImageModified(string imagePath, string imageTitle)
		{
			// Determine whether this instance is modified
			// If image with no path (i.e., cloned) and not the same title
			if (ImagePath == string.Empty && ImageTitle != imageTitle)
				return;

			if (ImagePath != imagePath)
				return;

			SetFormTitle();
		}

		/// <summary>
		/// Display confirmation dialog box on closing if image has been modified.
		/// </summary>
		/// <returns>False if user selects cancel, true otherwise.</returns>
		private bool ConfirmClose()
		{
			CustomMessageBox confirmBox = new CustomMessageBox(
				String.Format("Save changes to {0} before closing?", ImageTitle),
				"Confirm Close",
				CustomMessageBox.CustomMessageBoxButtons.YesNoCancel
			);

			CustomMessageBox.CustomDialogResult dialogResult = confirmBox.ShowCustomDialog(this);

			if (dialogResult == CustomMessageBox.CustomDialogResult.Yes)
			{
				// HACK: Invoke MdiParent's SaveImage function 
				return (MdiParent as MainForm).SaveImage();

				//// Display error message if save is not successful
				//if (!result)
				//{
				//   CustomMessageBox messageBox = new CustomMessageBox(
				//      string.Format("Error saving image to {0}", ImageTitle),
				//      "Save Error",
				//      CustomMessageBox.CustomMessageBoxButtons.Ok
				//   );
				//   messageBox.ShowCustomDialog(this);

				//   return false;
				//}
				//return true;
			}
			else if (dialogResult == CustomMessageBox.CustomDialogResult.No)
				return true;
			else // cancel
				return false;
		}

		/// <summary>
		/// Set title of form to file name.
		/// </summary>
		private void SetFormTitle()
		{
			Text = ImageTitle;

			if (IsImageModified)
			{
				Text += " *";
			}
		}

		#endregion

		#region Display operations

		/// <summary>
		/// Show the image at 100% zoom, but adjust the form so that 
		/// as much of the image is shown as possible.  Compare with
		/// simply setting zoom to 100%, which does not resize the image
		/// form.
		/// </summary>
		public void ActualPixels()
		{
			MdiClient mdiContainer = GetMdiContainer();

			if (mdiContainer == null)
				return;

			// First pass, fit form to image
			int newClientWidth = (Image.Width < mdiContainer.ClientSize.Width) ? Image.Width : mdiContainer.ClientSize.Width;
			int newClientHeight = (Image.Height < mdiContainer.ClientSize.Height) ? Image.Height : mdiContainer.ClientSize.Height;
			ClientSize = new Size(newClientWidth, newClientHeight);

			// Second pass, make sure we don't exceed mdiContainer
			int newWidth = (Width < mdiContainer.ClientSize.Width) ? Width : mdiContainer.ClientSize.Width;
			int newHeight = (Height < mdiContainer.ClientSize.Height) ? Height : mdiContainer.ClientSize.Height;

			// HACK: Adjust for scrollbars
			if (Width > mdiContainer.ClientSize.Width)
				newHeight += SystemInformation.HorizontalScrollBarHeight;

			if (Height > mdiContainer.ClientSize.Height)
				newWidth += SystemInformation.VerticalScrollBarWidth;

			Size = new Size(newWidth, newHeight);
			Location = Point.Empty;

			SetZoom(1);	// set to 100%
		}

		public void Draw()
		{
			_imageControl.Draw();
		}

		public void FitOnScreen()
		{
			// Resize form to fit main form's MdiClient container (sans menu, toolstrip, status bar)
			MdiClient mdiContainer = GetMdiContainer();

			if (mdiContainer == null)
				return;

			Width = mdiContainer.ClientSize.Width;
			Height = mdiContainer.ClientSize.Height;

			// Line up image form to top left corner of Mdi container
			Location = Point.Empty;

			// Fill image to image form 
			int imageFormWidth = ClientRectangle.Width;
			int imageFormHeight = ClientRectangle.Height;

			float xScaling = imageFormWidth / (float)(Image.Width + 2);
			float yScaling = imageFormHeight / (float)(Image.Height + 2);
			float zoom = Math.Min(xScaling, yScaling);

			SetZoom(zoom);
		}

		public void RenderForPrint(Graphics grfx, PageSettings page)
		{
			_imageControl.RenderForPrint(grfx, page);
		}

		/// <summary>
		/// Get the size of the MdiClient control; i.e., the main form's Mdi container
		/// sans other controls such as menu strip, tool strip, status bar, etc.
		/// </summary>
		/// <returns>The Mdi client, or null.</returns>
		public MdiClient GetMdiContainer()
		{
			foreach (Control ctrl in MdiParent.Controls)
			{
				if (ctrl is MdiClient)
				{
					return ctrl as MdiClient;
				}
			}
			return null;
		}

		public void SetZoom(float zoom)
		{
			_imageControl.SetZoom(zoom);
		}

		public void ZoomIn()
		{
			_imageControl.ZoomIn();
		}

		public void ZoomOut()
		{
			_imageControl.ZoomOut();
		}

		#endregion

		#region Image handling

		/// <summary>
		/// Instruct ImageControl to load and display image.
		/// </summary>
		/// <param name="path">The path of the image.</param>
		/// <returns>True if no error.</returns>
		public bool OpenImage(string path)
		{
			bool result = _imageControl.OpenImage(path);

			if (!result)
				return false;

			// If image is smaller than client area, reduce client area to fit image
			if (ClientSize.Width > Image.Width || ClientSize.Height > Image.Height)
			{
				ClientSize = Image.Size;
			}

			// Render the image _after_ the ClientSize has been set
			_imageControl.Draw();

			return true;
		}

		/// <summary>
		/// Overload which uses an existing image.
		/// </summary>
		public bool OpenImage(Bitmap existingImage, string imageTitle)
		{
			bool result = _imageControl.OpenImage(existingImage, imageTitle);

			if (!result)
				return false;

			// If image is smaller than client area, reduce client area to fit image
			if (ClientSize.Width > Image.Width || ClientSize.Height > Image.Height)
			{
				ClientSize = Image.Size;
			}

			// Render the image _after_ the ClientSize has been set
			_imageControl.Draw();

			return true;
		}

		/// <summary>
		/// Pass on command to ImageControl to save image.
		/// </summary>
		public bool SaveImage()
		{
			bool result = _imageControl.SaveImage();

			if (result)
				SetFormTitle();

			return result;
		}

		public bool SaveImageAs(string newPath, ImageFormat newFormat)
		{
			return _imageControl.SaveImageAs(newPath, newFormat);
		}

		#endregion

		#region Image operations

		#region Color

		public void Grayscale()
		{
			_imageControl.Grayscale();
		}

		public void InvertColors()
		{
			_imageControl.InvertColors();
		}

		public void RotateColors()
		{
			_imageControl.RotateColors();
		}

		public void Sepia()
		{
			_imageControl.Sepia();
		}

		#endregion

		public void Redo()
		{
			_imageControl.Redo();
		}

		public void RotateFlip(RotateFlipType rotateFlipType)
		{
			_imageControl.RotateFlip(rotateFlipType);
		}

		public void Undo()
		{
			_imageControl.Undo();
		}

		#endregion

	}	// class
}	// namespace