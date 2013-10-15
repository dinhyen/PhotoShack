namespace CommonUI
{
	partial class ImageForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this._contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this._contextMenuFitOnScreen = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this._imageControl = new CommonUI.ImageControl();
			this._contextMenuActualPixels = new System.Windows.Forms.ToolStripMenuItem();
			this._contextMenuZoomIn = new System.Windows.Forms.ToolStripMenuItem();
			this._contextMenuZoomOut = new System.Windows.Forms.ToolStripMenuItem();
			this._contextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// _contextMenu
			// 
			this._contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._contextMenuActualPixels,
            this._contextMenuFitOnScreen,
            this.toolStripSeparator1,
            this._contextMenuZoomIn,
            this._contextMenuZoomOut});
			this._contextMenu.Name = "_contextMenu";
			this._contextMenu.Size = new System.Drawing.Size(180, 98);
			// 
			// _contextMenuFitOnScreen
			// 
			this._contextMenuFitOnScreen.Image = global::CommonUI.Properties.Resources.ExpandSpace;
			this._contextMenuFitOnScreen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._contextMenuFitOnScreen.Name = "_contextMenuFitOnScreen";
			this._contextMenuFitOnScreen.Size = new System.Drawing.Size(179, 22);
			this._contextMenuFitOnScreen.Text = "&Fit On Screen";
			this._contextMenuFitOnScreen.Click += new System.EventHandler(this.OnContextFitOnScreen);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(176, 6);
			// 
			// _imageControl
			// 
			this._imageControl.AutoScroll = true;
			this._imageControl.ContextMenuStrip = this._contextMenu;
			this._imageControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this._imageControl.Location = new System.Drawing.Point(0, 0);
			this._imageControl.Name = "_imageControl";
			this._imageControl.Size = new System.Drawing.Size(632, 446);
			this._imageControl.TabIndex = 0;
			// 
			// _contextMenuActualPixels
			// 
			this._contextMenuActualPixels.Image = global::CommonUI.Properties.Resources.ActualSize;
			this._contextMenuActualPixels.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._contextMenuActualPixels.Name = "_contextMenuActualPixels";
			this._contextMenuActualPixels.Size = new System.Drawing.Size(179, 22);
			this._contextMenuActualPixels.Text = "&Actual Pixels";
			this._contextMenuActualPixels.Click += new System.EventHandler(this.OnContextActualPixels);
			// 
			// _contextMenuZoomIn
			// 
			this._contextMenuZoomIn.Image = global::CommonUI.Properties.Resources.ZoomIn;
			this._contextMenuZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._contextMenuZoomIn.Name = "_contextMenuZoomIn";
			this._contextMenuZoomIn.Size = new System.Drawing.Size(179, 22);
			this._contextMenuZoomIn.Text = "Zoom &In";
			this._contextMenuZoomIn.Click += new System.EventHandler(this.OnContextZoomIn);
			// 
			// _contextMenuZoomOut
			// 
			this._contextMenuZoomOut.Image = global::CommonUI.Properties.Resources.ZoomOut;
			this._contextMenuZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._contextMenuZoomOut.Name = "_contextMenuZoomOut";
			this._contextMenuZoomOut.Size = new System.Drawing.Size(179, 22);
			this._contextMenuZoomOut.Text = "Zoom &Out";
			this._contextMenuZoomOut.Click += new System.EventHandler(this.OnContextZoomOut);
			// 
			// ImageForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(632, 446);
			this.Controls.Add(this._imageControl);
			this.Name = "ImageForm";
			this.Text = "ImageForm";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
			this.Activated += new System.EventHandler(this.OnActivated);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
			this.Load += new System.EventHandler(this.OnLoad);
			this._contextMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private ImageControl _imageControl;
		private System.Windows.Forms.ContextMenuStrip _contextMenu;
		private System.Windows.Forms.ToolStripMenuItem _contextMenuFitOnScreen;
		private System.Windows.Forms.ToolStripMenuItem _contextMenuActualPixels;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem _contextMenuZoomIn;
		private System.Windows.Forms.ToolStripMenuItem _contextMenuZoomOut;

	}
}