namespace CommonUI
{
	partial class CustomMessageBox
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
			this._lblMessage = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// _lblMessage
			// 
			this._lblMessage.AutoSize = true;
			this._lblMessage.Location = new System.Drawing.Point(15, 16);
			this._lblMessage.Name = "_lblMessage";
			this._lblMessage.Size = new System.Drawing.Size(0, 17);
			this._lblMessage.TabIndex = 3;
			this._lblMessage.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// CustomMessageBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 97);
			this.Controls.Add(this._lblMessage);
			this.Font = new System.Drawing.Font("Arial", 8.059701F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CustomMessageBox";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "CustomMessageBox";
			this.TopMost = true;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label _lblMessage;
	}
}