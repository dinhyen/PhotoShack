using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DataObjects;

namespace CommonUI
{
	public partial class StatusStripControl : UserControl
	{
		#region Fields

		private Color _currentColor = Color.Empty;
		private float _currentZoom = 0f;
		private Size _imageSize = Size.Empty;
		private string _status = string.Empty;

		private PerformanceCounter _perfCounter;
		private Timer _timer;

		#endregion

		#region Properties

		public Color CurrentColor
		{
			get { return _currentColor; }
			set
			{
				_currentColor = value;

				if (_currentColor == Color.Empty)
				{
					_lblRgb.Text = string.Empty;
					return;
				}

				string currentColor = string.Format("R:{0} G:{1} B:{2}", _currentColor.R, _currentColor.G, _currentColor.B);
				_lblRgb.Text = currentColor;
			}
		}

		public float CurrentZoom
		{
			get { return _currentZoom; }
			set
			{
				_currentZoom = value;

				if (_currentZoom < 0.01f)	// less than 1%, practically 0
				{
					_lblZoom.Text = string.Empty;
					return;
				}

				string currentZoom = string.Format("{0}%", (int)(_currentZoom * 100));
				_lblZoom.Text = currentZoom;
			}
		}

		public Size ImageSize
		{
			get { return _imageSize; }
			set
			{
				_imageSize = value;
				if (_imageSize.Width == 0 || _imageSize.Height == 0)
				{
					_lblDimension.Text = string.Empty;
					return;
				}

				string dimension = string.Format("{0}x{1}", _imageSize.Width, _imageSize.Height);
				_lblDimension.Text = dimension;
			}
		}

		public string Status
		{
			get { return _status; }
			set
			{
				_status = value;

				_lblStatus.Text = _status;
			}
		}

		#endregion

		public StatusStripControl()
		{
			InitializeComponent();

			_perfCounter = new PerformanceCounter();

			_timer = new Timer();
		}

		private void UpdateMemoryUsage()
		{
			// Calculate memory usage in MB and update progress bar
			int memoryInMB = (int)(_perfCounter.NextValue() / 1000000f);

			_pgbMemory.Value = memoryInMB;

			// TODO: Display memory usage message here
		}

		private void OnLoad(object sender, EventArgs e)
		{
			// Don't load if in Designer mode
			if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv")
				return;

			OperationsManager.MouseOverImage += new MouseOverImageEventHandler(OnMouseOverImage);

			_lblStatus.Text = string.Empty;
			_lblRgb.Text = string.Empty;
			_lblDimension.Text = string.Empty;
			_lblZoom.Text = string.Empty;

			_timer.Interval = 10000;	// 10s
			_timer.Tick += new EventHandler(OnTimerTick);
			_timer.Enabled = true;

			_perfCounter.CategoryName = "Process";
			_perfCounter.CounterName = "Private Bytes";
			_perfCounter.InstanceName = "PhotoShack";

			UpdateMemoryUsage();
		}

		void OnMouseOverImage(object sender, Point coordinate)
		{
			// Outside image
			if (coordinate.X < 0 || coordinate.Y < 0)
			{
				CurrentColor = Color.Empty;
				return;
			}

			Color currentColor = (sender as ImageControl).Image.GetPixel(coordinate.X, coordinate.Y);
			CurrentColor = currentColor;
		}

		private void OnTimerTick(object sender, EventArgs e)
		{
			UpdateMemoryUsage();
		}

	}	// class
}	// namespace
