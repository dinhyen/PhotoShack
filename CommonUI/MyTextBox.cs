using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace CommonUI
{
	/// <summary>
	/// Hides the caret
	/// </summary>
	public class MyTextBox : TextBox
	{
		[DllImport("user32.dll", EntryPoint = "HideCaret")]

		public static extern long HideCaret(IntPtr hwnd);

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);

			HideCaret(this.Handle);
		}
	}

}
