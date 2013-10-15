namespace CommonUI
{
	partial class StatusStripControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._tlpMain = new System.Windows.Forms.TableLayoutPanel();
			this._lblRgb = new System.Windows.Forms.Label();
			this._lblStatus = new System.Windows.Forms.Label();
			this._lblDimension = new System.Windows.Forms.Label();
			this._lblZoom = new System.Windows.Forms.Label();
			this._pgbMemory = new System.Windows.Forms.ProgressBar();
			this._comboBox = new System.Windows.Forms.ComboBox();
			this._tlpMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// _tlpMain
			// 
			this._tlpMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
							| System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this._tlpMain.ColumnCount = 6;
			this._tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
			this._tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this._tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
			this._tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
			this._tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
			this._tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this._tlpMain.Controls.Add(this._lblRgb, 2, 0);
			this._tlpMain.Controls.Add(this._lblStatus, 0, 0);
			this._tlpMain.Controls.Add(this._lblDimension, 1, 0);
			this._tlpMain.Controls.Add(this._lblZoom, 5, 0);
			this._tlpMain.Controls.Add(this._pgbMemory, 4, 0);
			this._tlpMain.Controls.Add(this._comboBox, 3, 0);
			this._tlpMain.Location = new System.Drawing.Point(0, 0);
			this._tlpMain.Name = "_tlpMain";
			this._tlpMain.RowCount = 1;
			this._tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tlpMain.Size = new System.Drawing.Size(800, 30);
			this._tlpMain.TabIndex = 0;
			// 
			// _lblRgb
			// 
			this._lblRgb.AutoSize = true;
			this._lblRgb.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this._lblRgb.Dock = System.Windows.Forms.DockStyle.Fill;
			this._lblRgb.Location = new System.Drawing.Point(253, 3);
			this._lblRgb.Margin = new System.Windows.Forms.Padding(3);
			this._lblRgb.Name = "_lblRgb";
			this._lblRgb.Size = new System.Drawing.Size(144, 24);
			this._lblRgb.TabIndex = 2;
			this._lblRgb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// _lblStatus
			// 
			this._lblStatus.AutoSize = true;
			this._lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this._lblStatus.Dock = System.Windows.Forms.DockStyle.Fill;
			this._lblStatus.Location = new System.Drawing.Point(3, 3);
			this._lblStatus.Margin = new System.Windows.Forms.Padding(3);
			this._lblStatus.Name = "_lblStatus";
			this._lblStatus.Size = new System.Drawing.Size(144, 24);
			this._lblStatus.TabIndex = 0;
			this._lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _lblDimension
			// 
			this._lblDimension.AutoSize = true;
			this._lblDimension.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this._lblDimension.Dock = System.Windows.Forms.DockStyle.Fill;
			this._lblDimension.Location = new System.Drawing.Point(153, 3);
			this._lblDimension.Margin = new System.Windows.Forms.Padding(3);
			this._lblDimension.Name = "_lblDimension";
			this._lblDimension.Size = new System.Drawing.Size(94, 24);
			this._lblDimension.TabIndex = 1;
			this._lblDimension.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// _lblZoom
			// 
			this._lblZoom.AutoSize = true;
			this._lblZoom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this._lblZoom.Dock = System.Windows.Forms.DockStyle.Fill;
			this._lblZoom.Location = new System.Drawing.Point(752, 3);
			this._lblZoom.Margin = new System.Windows.Forms.Padding(3);
			this._lblZoom.Name = "_lblZoom";
			this._lblZoom.Size = new System.Drawing.Size(45, 24);
			this._lblZoom.TabIndex = 3;
			this._lblZoom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _pgbMemory
			// 
			this._pgbMemory.Dock = System.Windows.Forms.DockStyle.Fill;
			this._pgbMemory.Location = new System.Drawing.Point(630, 3);
			this._pgbMemory.Name = "_pgbMemory";
			this._pgbMemory.Size = new System.Drawing.Size(116, 24);
			this._pgbMemory.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this._pgbMemory.TabIndex = 4;
			this._pgbMemory.Value = 50;
			// 
			// _comboBox
			// 
			this._comboBox.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._comboBox.FormattingEnabled = true;
			this._comboBox.Location = new System.Drawing.Point(403, 3);
			this._comboBox.Name = "_comboBox";
			this._comboBox.Size = new System.Drawing.Size(221, 24);
			this._comboBox.TabIndex = 5;
			this._comboBox.Text = "Select item...";
			// 
			// StatusStripControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this._tlpMain);
			this.Name = "StatusStripControl";
			this.Size = new System.Drawing.Size(800, 30);
			this.Load += new System.EventHandler(this.OnLoad);
			this._tlpMain.ResumeLayout(false);
			this._tlpMain.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tlpMain;
		private System.Windows.Forms.Label _lblStatus;
		private System.Windows.Forms.Label _lblDimension;
		private System.Windows.Forms.Label _lblRgb;
		private System.Windows.Forms.Label _lblZoom;
		private System.Windows.Forms.ProgressBar _pgbMemory;
		private System.Windows.Forms.ComboBox _comboBox;
	}
}
