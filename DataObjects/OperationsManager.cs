using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace DataObjects
{
	public delegate void ImageOpenedEventHandler(string imagePath, string imageTitle);
	public delegate void ImageClosedEventHandler(string imagePath, string imageTitle);
	public delegate void ImageModifiedEventHandler(string imagePath, string imageTitle);
	public delegate void ImageActivatedEventHandler(string imagePath, string imageTitle);
	public delegate void ImageZoomedEventHandler(string imagePath, string imageTitle, float zoom);
	public delegate void MouseOverImageEventHandler(object sender, Point coordinate);

	public static class OperationsManager
	{
		#region Events

		public static event ImageOpenedEventHandler ImageOpened;
		public static event ImageClosedEventHandler ImageClosed;
		public static event ImageModifiedEventHandler ImageModified;
		public static event ImageActivatedEventHandler ImageActivated;
		public static event ImageZoomedEventHandler ImageZoomed;
		public static event MouseOverImageEventHandler MouseOverImage;

		// Provide methods for raising events since an event
		// can only be raised by the class that defines it.
		
		public static void RaiseImageOpenedEvent(string imagePath, string imageTitle)
		{
			if (ImageOpened != null)	// if there are subscribers
				ImageOpened(imagePath, imageTitle);
		}
		
		public static void RaiseImageClosedEvent(string imagePath, string imageTitle)
		{
			if (ImageClosed != null)
				ImageClosed(imagePath, imageTitle);
		}
		
		public static void RaiseImageModifiedEvent(string imagePath, string imageTitle)
		{
			if (ImageModified != null)
				ImageModified(imagePath, imageTitle);
		}

		public static void RaiseImageActivatedEvent(string imagePath, string imageTitle)
		{
			if (ImageActivated != null)
				ImageActivated(imagePath, imageTitle);
		}

		public static void RaiseImageZoomedEvent(string imagePath, string imageTitle, float zoom)
		{
			if (ImageZoomed != null)
				ImageZoomed(imagePath, imageTitle, zoom);
		}

		public static void RaiseMouseOverImageEvent(object sender, Point coordinate)
		{
			if (MouseOverImage != null)
				MouseOverImage(sender, coordinate);
		}

		#endregion

		/// <summary>
		/// Read an image from disk and return a copy of the image.
		/// </summary>
		/// <param name="path">The path to the image.</param>
		/// <returns>True if there is no error.</returns>
		public static bool Open(out Bitmap image, string path, out ImageFormat format)
		{
			Image originalImage;

			try
			{
				originalImage = Bitmap.FromFile(path);
				format = originalImage.RawFormat;
			}
			catch (FileNotFoundException ex)
			{
				image = null;
				format = null;
				return false;
			}

			// HACK: To avoid "Generic error in GDI+" when image is read as stream, COPY image using following workaround:
			//1. Open the image file
			//2. create a temporary in-memory image the same size as the original
			//3. obtain a Graphics object for the temporary image
			//4. Draw the original image onto the temporary one
			//5. dispose of the original
			//6. do any drawing you'd like on the image using the Graphics you have
			//7. Dispose of the Graphics
			//8. Write the temporary file to any filename and any format you please including the same name and format if you wish.
			// Also try Bitmap.Clone(Rectangle, PixelFormat)

			image = new Bitmap(originalImage.Width, originalImage.Height);
			Graphics grfx = Graphics.FromImage(image);
			grfx.DrawImage(originalImage, new Rectangle(Point.Empty, originalImage.Size));	// works!

			//grfx.DrawImage(originalImage, grfx.ClipBounds);	// came out black
			//grfx.DrawImage(originalImage, new Point(0, 0));	// came out reduced in size
			//grfx.DrawImageUnscaled(originalImage, Point.Empty);	// display depends on pixel resolution!

			originalImage.Dispose();
			grfx.Dispose();

			return true;
		}

		/// <summary>
		/// Make a copy of an existing image.
		/// </summary>
		/// <param name="originalImage">The image to clone.</param>
		/// <returns>A copy of the image.</returns>
		public static Bitmap Clone(Bitmap originalImage)
		{
			return originalImage.Clone(new Rectangle(Point.Empty, originalImage.Size), originalImage.PixelFormat);
		}

		/// <summary>
		/// Save the image to the specific path and with the specified format.
		/// </summary>
		/// <param name="path">The fully qualified path.</param>
		/// <param name="format">The output format.</param>
		/// <returns>True if successful.</returns>
		public static bool Save(Bitmap image, string path, ImageFormat format)
		{
			try
			{
				image.Save(path, format);
			}
			catch (Exception ex)
			{
				throw ex;	// rethrow for now
			}
			return true;
		}

	}	// class
}	// namespace
