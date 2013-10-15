namespace CommonUI
{
	partial class AboutBox
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBox));
			this._timer = new System.Windows.Forms.Timer(this.components);
			this._txtInfo = new CommonUI.MyTextBox();
			this.SuspendLayout();
			// 
			// _timer
			// 
			this._timer.Tick += new System.EventHandler(this.OnTimerTick);
			// 
			// _txtInfo
			// 
			this._txtInfo.BackColor = System.Drawing.SystemColors.Window;
			this._txtInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._txtInfo.Cursor = System.Windows.Forms.Cursors.Hand;
			this._txtInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.134328F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._txtInfo.ForeColor = System.Drawing.Color.Chocolate;
			this._txtInfo.Location = new System.Drawing.Point(158, 268);
			this._txtInfo.Name = "_txtInfo";
			this._txtInfo.Size = new System.Drawing.Size(313, 20);
			this._txtInfo.TabIndex = 1;
			this._txtInfo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// AboutBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.ClientSize = new System.Drawing.Size(500, 300);
			this.Controls.Add(this._txtInfo);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutBox";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About PhotoShack";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Timer _timer;
		private MyTextBox _txtInfo;
	}
}