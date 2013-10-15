using System;
using System.Windows.Forms;

namespace CommonUI
{
	public partial class SplashScreen : Form
	{
		private int _displayDuration = 1500;	// interval (in ms) to display splash screen
		private int _elapsedTime = 0;

		public int DisplayDuration
		{
			get { return _displayDuration; }
			set { _displayDuration = value; }
		}

		public SplashScreen()
		{
			InitializeComponent();
		}

		private void OnShown(object sender, EventArgs e)
		{
			_timer.Enabled = true;
		}

		/// <summary>
		/// On timer ticker, determine time elapsed and begin fading splash screen 
		/// after the duration specified by STEADY_DISPLAY_DURATION
		/// </summary>
		private void OnTick(object sender, EventArgs e)
		{
			_elapsedTime += _timer.Interval;

			//System.Diagnostics.Trace.WriteLine("elapsed=" + _elapsedTime);
			//System.Diagnostics.Trace.WriteLine("opacity=" + Opacity);

			if (Opacity < 0.1d)
			{
				_timer.Stop();
				_timer.Dispose();
				Close();
				return;
			}
			
			if (_elapsedTime >= _displayDuration)
			{
				// Begin fading
				Opacity -= 0.1d;
			}
		}

	}	// class
}	// namespace