using System;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CommonUI
{
	public partial class AboutBox : Form
	{
		private int _charInterval = 100;	// in ms
		private string[] _displayStrings = null;
		private int _selectedIndex = 0;
		private int _charCount = 0;
		private int _elapsedTime = 0;

		public AboutBox()
		{
			InitializeComponent();
		}

		#region Assembly Attribute Accessors

		public string AssemblyTitle
		{
			get
			{
				// Get all Title attributes on this assembly
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				// If there is at least one Title attribute
				if (attributes.Length > 0)
				{
					// Select the first one
					AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
					// If it is not an empty string, return it
					if (titleAttribute.Title != "")
						return titleAttribute.Title;
				}
				// If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
				return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			}
		}

		public string AssemblyVersion
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version.ToString();
			}
		}

		public string AssemblyDescription
		{
			get
			{
				// Get all Description attributes on this assembly
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
				// If there aren't any Description attributes, return an empty string
				if (attributes.Length == 0)
					return "";
				// If there is a Description attribute, return its value
				return ((AssemblyDescriptionAttribute)attributes[0]).Description;
			}
		}

		public string AssemblyProduct
		{
			get
			{
				// Get all Product attributes on this assembly
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
				// If there aren't any Product attributes, return an empty string
				if (attributes.Length == 0)
					return "";
				// If there is a Product attribute, return its value
				return ((AssemblyProductAttribute)attributes[0]).Product;
			}
		}

		public string AssemblyCopyright
		{
			get
			{
				// Get all Copyright attributes on this assembly
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
				// If there aren't any Copyright attributes, return an empty string
				if (attributes.Length == 0)
					return "";
				// If there is a Copyright attribute, return its value
				return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
			}
		}

		public string AssemblyCompany
		{
			get
			{
				// Get all Company attributes on this assembly
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
				// If there aren't any Company attributes, return an empty string
				if (attributes.Length == 0)
					return "";
				// If there is a Company attribute, return its value
				return ((AssemblyCompanyAttribute)attributes[0]).Company;
			}
		}

		#endregion

		private void OnLoad(object sender, EventArgs e)
		{
			// Don't load if in Designer mode
			if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv")
				return;

			_displayStrings = new string[]
			{
				string.Format("PhotoShack version {0}", this.AssemblyVersion),
				"for METCS 503 with Professor Hawili",
				"programmed by Dinh-Yen Tran",
				this.AssemblyCopyright
			};

			_timer.Enabled = true;
		}

		private void OnTimerTick(object sender, EventArgs e)
		{
			_elapsedTime += _timer.Interval;

			if (_elapsedTime % _charInterval == 0)
			{
				if (_charCount++ < _displayStrings[_selectedIndex].Length)
				{
					_txtInfo.Text = _displayStrings[_selectedIndex].Substring(0, _charCount);
					return;
				}
			}

			_timer.Stop();

			_charCount = 0;

			if (++_selectedIndex == _displayStrings.Length)
				_selectedIndex = 0;

			_elapsedTime = 0;

			_timer.Start();

		}

		private void OnFormClosing(object sender, FormClosingEventArgs e)
		{
			_timer.Dispose();
		}

	}	// class

}	// namespace