using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Windows.Forms;
using DataObjects;
using AForge.Imaging;
using AForge.Imaging.Filters;

namespace CommonUI
{
	/// <summary>
	/// Control for image display and manipulation.
	/// </summary>
	public partial class ImageControl : UserControl
	{
		#region Member variables

		private Bitmap _image = null;
		private Bitmap _redoImage = null;
		private Bitmap _undoImage = null;

		private bool _isModified = false;

		private string _path = string.Empty;
		private ImageFormat _format = null;
		private string _title = string.Empty;

		private float _zoom = 1f;	// zoom factor

		#endregion

		#region Properties

		public bool CanRedo
		{
			get
			{
				return (_redoImage != null);
			}
		}

		public bool CanUndo
		{
			get
			{
				return (_undoImage != null);
			}
		}

		/// <summary>
		/// Read-only property.  Memory copy of image.
		/// </summary>
		public Bitmap Image
		{
			get { return _image; }
		}

		public bool IsModified
		{
			get { return _isModified; }
		}

		/// <summary>
		/// Read-only property. Each image opened must have a path on disk.
		/// </summary>
		public string Path
		{
			get { return _path; }
		}

		public string Title
		{
			get { return _title; }
		}

		public float Zoom
		{
			get { return _zoom; }
		}

		#endregion

		#region Ctor

		public ImageControl()
		{
			InitializeComponent();

			Init();
		}

		#endregion

		private void Init()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
		}

		#region Display operations

		public void Draw()
		{
			// Update Autoscroll to cover new image size
			AutoScrollMinSize = new Size((int)(_image.Width * _zoom), (int)(_image.Height * _zoom));

			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (_image == null)
				return;

			// Set display size to adjust for zoom
			int width = (int)(_image.Width * _zoom);
			int height = (int)(_image.Height * _zoom);

			// Calculate left corner such that image is centered in control
			Rectangle rect = ClientRectangle;
			int x = (rect.Width < width) ? 0 : (rect.Width - width) / 2;
			int y = (rect.Height < height) ? 0 : (rect.Height - height) / 2;

			Graphics grfx = e.Graphics;

			// Set scroll
			grfx.TranslateTransform(AutoScrollPosition.X, AutoScrollPosition.Y);

			grfx.DrawImage(_image, x, y, width, height);
		}

		public void RenderForPrint(Graphics grfx, PageSettings page)
		{
			Font font = this.Font;
			int height = font.Height + 2;
			Pen pen = new Pen(Color.Black, 2);
			Brush brush = new SolidBrush(Color.Black);
			Brush bgBrush = new SolidBrush(Color.Silver);

			// Transform axes to account for margin
			grfx.TranslateTransform(page.Margins.Left, page.Margins.Top);

			// Compute printable dimensions (inside margins)
			int printableWidth = page.Bounds.Width - page.Margins.Left - page.Margins.Right;
			int printableHeight = page.Bounds.Height - page.Margins.Top - page.Margins.Bottom;

			// Computer print area that maximally fills printable surface
			float xScaling = (float)printableWidth / _image.Width;
			float yScaling = (float)printableHeight / _image.Height;
			float scaling = Math.Min(xScaling, yScaling);

			int printWidth = (int)(_image.Width * scaling);
			int printHeight = (int)(_image.Height * scaling);

			int x = (int)((printableWidth - printWidth) / 2);
			int y = (int)((printableHeight - printHeight) / 2);

			Rectangle rect = new Rectangle(x, y, printWidth, printHeight);

			// Draw image title
			int titleWidth = (int)grfx.MeasureString(_title, font).Width;
			grfx.DrawString(_title, font, brush, (x + printWidth / 2 - titleWidth / 2), -(page.Margins.Top / 2));

			// Draw image
			grfx.DrawImage(_image, rect);

			// Draw a border around image
			grfx.DrawRectangle(pen, rect);
		}

		public void SetZoom(float zoom)
		{
			_zoom = zoom;

			Draw();

			// Raise ImageZoomed event
			OperationsManager.RaiseImageZoomedEvent(_path, _title, _zoom);
		}

		public void ZoomIn()
		{
			float zoom = _zoom * 1.25f;

			if (zoom < 10)	// max at 1000%
			{
				SetZoom(zoom);
			}
		}

		public void ZoomOut()
		{
			float zoom = _zoom / 1.25f;

			if (zoom >= 0.05)	// min at 5%
			{
				SetZoom(zoom);
			}
		}

		#endregion

		#region Image handling
		
		#region Mouse events handling

		private void OnMouseLeave(object sender, EventArgs e)
		{
			// mouse is obviously outside image region
			Point coordinate = new Point(-1, -1);
			OperationsManager.RaiseMouseOverImageEvent(this, coordinate);
		}

		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			GetImageCoordinate(e);
		}

		/// <summary>
		/// Translate mouse coordinates on form to image coordinates.
		/// </summary>
		private void GetImageCoordinate(MouseEventArgs e)
		{
			Rectangle rect = this.ClientRectangle;

			int zoomedWidth = (int)(_image.Width * _zoom);
			int zoomedHeight = (int)(_image.Height * _zoom);

			// Top left corner
			int x = (rect.Width < zoomedWidth) ? 0 : (rect.Width - zoomedWidth) / 2;
			int y = (rect.Height < zoomedHeight) ? 0 : (rect.Height - zoomedHeight) / 2;

			// If mouse coordinate is within image region
			if ((e.X >= x) && (e.Y >= y) && (e.X < x + zoomedWidth) && (e.Y < y + zoomedHeight))
			{
				// mouse is over the image
				Point coordinate = new Point((int)((e.X - x) / _zoom), (int)((e.Y - y) / _zoom));
				OperationsManager.RaiseMouseOverImageEvent(this, coordinate);
			}
			else
			{
				// mouse is outside image region
				Point coordinate = new Point(-1, -1);
				OperationsManager.RaiseMouseOverImageEvent(this, coordinate);
			}
		}



		#endregion

		/// <summary>
		/// Read the specified image and save a copy of it into memory.
		/// </summary>
		/// <param name="path">The path to the image</param>
		/// <returns>True if no error.</returns>
		public bool OpenImage(string imagePath)
		{
			// Store the path
			_path = imagePath;

			// Set the title
			_title = System.IO.Path.GetFileName(_path);

			// Retrieve the image and image format
			return OperationsManager.Open(out _image, imagePath, out _format);
		}

		/// <summary>
		/// Overload which uses an existing image.
		/// </summary>
		/// <param name="existingImage">An existing bitmap.</param>
		/// <param name="imageTitle">The title of the image. A title must be specified because the image does not have a disk path.</param>
		/// <returns>True if successful</returns>
		public bool OpenImage(Bitmap existingImage, string imageTitle)
		{
			// Nullify the path
			_path = string.Empty;

			// Set the title
			_title = imageTitle;

			// Set format to default
			_format = ImageFormat.Jpeg;

			_image = existingImage;

			return true;
		}

		/// <summary>
		/// If image is modified, save the image to the same path.
		/// </summary>
		/// <returns>True if no error.</returns>
		public bool SaveImage()
		{
			SetModificationState(false);

			return OperationsManager.Save(_image, _path, _format);
		}

		/// <summary>
		/// Save the image to the specified path and with the specified format.
		/// </summary>
		/// <returns>True if no error.</returns>
		public bool SaveImageAs(string newPath, ImageFormat newFormat)
		{
			// note that image modified state is unchanged
			return OperationsManager.Save(_image, newPath, newFormat);
		}

		#endregion

		#region Image operations

		#region Color

		public void Grayscale()
		{
			if (_image.PixelFormat == PixelFormat.Format8bppIndexed)	// image already grayscale
				return;

			ApplyFilter(new GrayscaleBT709());
		}

		public void InvertColors()
		{
			ApplyFilter(new Invert());
		}

		public void RotateColors()
		{
			ApplyFilter(new RotateChannels());
		}

		public void Sepia()
		{
			ApplyFilter(new Sepia());
		}

		#endregion

		/// <summary>
		/// Apply a filter to the image.  A new image is created, which
		/// is set to the image of this control.
		/// </summary>
		public void ApplyFilter(IFilter filter)
		{
			PrepareUndo(false);	// not in-place operation

			Bitmap newImage = filter.Apply(_image);

			_image = newImage;

			Draw();

			SetModificationState(true);
		}

		/// <summary>
		/// Redo one level.
		/// </summary>
		public void Redo()
		{
			if (!CanRedo)
				return;

			if (_undoImage != null)
				_undoImage.Dispose();

			_undoImage = _image;
			_image = _redoImage;
			_redoImage = null;

			Draw();
		}

		public void RotateFlip(RotateFlipType rotateFlipType)
		{
			PrepareUndo(true);	// in-place operation

			_image.RotateFlip(rotateFlipType);

			Draw();

			SetModificationState(true);
		}

		/// <summary>
		/// One-level undo.
		/// </summary>
		public void Undo()
		{
			if (!CanUndo)
				return;

			if (_redoImage != null)
				_redoImage.Dispose();

			_redoImage = _image;
			_image = _undoImage;
			_undoImage = null;

			Draw();
		}

		#region Utilities

		/// <summary>
		/// Create undo image.
		/// </summary>
		/// <param name="isInPlaceOperation">If operation is in-place (no copy is created)</param>
		private void PrepareUndo(bool isInPlaceOperation)
		{
			if (_undoImage != null)
				_undoImage.Dispose();

			if (isInPlaceOperation)
				_undoImage = OperationsManager.Clone(_image);
			else
				_undoImage = _image;
		}

		/// <summary>
		/// Set flag to indicate whether image is modified.
		/// </summary>
		/// <param name="bIsImageModified">True if modified.</param>
		private void SetModificationState(bool bIsImageModified)
		{
			_isModified = bIsImageModified;

			if (_isModified)
			{
				OperationsManager.RaiseImageModifiedEvent(_path, _title);
			}
		}

		#endregion

		#endregion


	}	// class
}	// namespace
