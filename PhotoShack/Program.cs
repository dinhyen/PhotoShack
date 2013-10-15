using System;
using System.Threading;
using System.Windows.Forms;
using CommonUI;

namespace PhotoShack
{
	public static class Program
	{
		public static ApplicationContext _context;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			// Approach to display splash screen using ApplicationContext based on:
			// http://www.c-sharpcorner.com/UploadFile/johnconwell/ApplicationContexttoEncapsulateSplashScreenFunctionality11232005041406AM/ApplicationContexttoEncapsulateSplashScreenFunctionality.aspx

			using (SplashScreen splash = new SplashScreen())
			{
				_context = new ApplicationContext(splash);
				Application.Run(_context);
			}

			// When splash screen exits, start a new context which loads the main form
			// Although, we can also simply call another Application.Run(mainForm);
			using (MainForm mainForm = new MainForm())
			{
				//_context.MainForm = mainForm;
				_context = new ApplicationContext(mainForm);
				Application.Run(_context);
			}
		}

	}	//class
}	// namespace