using System;
using System.Drawing;
using System.Windows.Forms;

namespace CommonUI
{
	/// <remarks>
	/// Same functionality as MessageBox, but is associated with 
	/// owner form and is display at the center of the owner form.  
	/// Equivalent of ShowDialog is ShowCustomDialog, which returns
	/// CustomDialogResult.
	/// </remarks>
	public partial class CustomMessageBox : Form
	{
		/// <summary>
		/// Same as MessageBoxButtons enumeration, plus ExitCancel option
		/// </summary>
		public enum CustomMessageBoxButtons : uint
		{
			Ok, YesNo, OkCancel, ExitCancel, DeleteCancel, AbortRetryIgnore, YesNoCancel
		};

		/// <summary>
		/// Same as DialogResult, plus value for Exit.
		/// </summary>
		public enum CustomDialogResult : uint
		{
			Ok, Yes, No, Exit, Delete, Cancel, Abort, Retry, Ignore, None
		};

		#region Member variables
		private string _message;	// text to display
		private string _caption;	// name of dialog box
		private CustomMessageBoxButtons _type;	// number and type of buttons

		private const int BUTTON_WIDTH = 75;
		private const int BUTTON_HEIGHT = 30;
		private const int BUTTON_SPACE = 15;	// distance between adjacent buttons
		private const int PADDING = 15;	// distnce between  label and buttons
		#endregion

		#region Properties

		public string Message
		{
			get { return _message; }
			set { _message = value; }
		}

		public string Caption
		{
			get { return _caption; }
			set { _caption = value; }
		}

		#endregion

		#region Ctors

		// Make private to disable default construction
		private CustomMessageBox()
		{
			InitializeComponent();
		}

		public CustomMessageBox(string message, string caption, CustomMessageBoxButtons type)
			: this()
		{
			_message = message;
			_caption = caption;
			_type = type;

			_lblMessage.Text = _message;
			this.Text = _caption;

			// Adjust dimensions of message box to accommodate different-length messages
			Graphics grafx = this.CreateGraphics();	// Need Graphics object for MeasureString
			Font font = _lblMessage.Font;
			// Measure width of label
			SizeF messageSize = grafx.MeasureString(this._message, font);
			int messageWidth = (int)messageSize.Width;
			int messageHeight = (int)messageSize.Height;
			// New form width = message width + 2x message offset
			int newClientWidth = messageWidth + 2 * _lblMessage.Location.X;
			// New form height = message height + button width + offsets
			int newClientHeight = _lblMessage.Location.Y + messageHeight + PADDING + BUTTON_HEIGHT + 20;

			// Make form wider if necessary
			int clientWidth = this.ClientSize.Width;
			int clientHeight = this.ClientSize.Height;
			if (newClientWidth > clientWidth)
				clientWidth = newClientWidth;
			if (newClientHeight > clientHeight)
				clientHeight = newClientHeight;
			this.ClientSize = new Size(clientWidth, clientHeight);

			AddButtons();
		}

		#endregion

		/// <summary>
		/// Add buttons to form.
		/// </summary>
		private void AddButtons()
		{
			Button[] buttons;
			
			switch (this._type)
			{
				case (CustomMessageBoxButtons.Ok):
					buttons = CreateButtons(new string[] { "OK" });
					buttons[0].DialogResult = DialogResult.OK;
					break;
				case (CustomMessageBoxButtons.YesNo):
					buttons = CreateButtons(new string[] { "Yes", "No" });
					buttons[0].DialogResult = DialogResult.Yes;
					buttons[1].DialogResult = DialogResult.No;
					break;
				case (CustomMessageBoxButtons.OkCancel):
				default:
					buttons = CreateButtons(new string[] { "OK", "Cancel" });
					buttons[0].DialogResult = DialogResult.OK;
					buttons[1].DialogResult = DialogResult.Cancel;
					break;
				case (CustomMessageBoxButtons.ExitCancel):
					buttons = CreateButtons(new string[] { "Exit", "Cancel" });
					buttons[0].DialogResult = DialogResult.OK;	// returns OK
					buttons[1].DialogResult = DialogResult.Cancel;
					this.AcceptButton = buttons[1];	// doesn't seem to work
					break;
				case (CustomMessageBoxButtons.DeleteCancel):
					buttons = CreateButtons(new string[] { "Delete", "Cancel" });
					buttons[0].DialogResult = DialogResult.OK;	// returns OK
					buttons[1].DialogResult = DialogResult.Cancel;
					this.AcceptButton = buttons[1];	// doesn't seem to work
					break;

				case (CustomMessageBoxButtons.AbortRetryIgnore):
					buttons = CreateButtons(new string[] { "Abort", "Retry", "Ignore" });
					buttons[0].DialogResult = DialogResult.Abort;
					buttons[1].DialogResult = DialogResult.Retry;
					buttons[2].DialogResult = DialogResult.Ignore;
					break;
				case (CustomMessageBoxButtons.YesNoCancel):
					buttons = CreateButtons(new string[] { "Yes", "No", "Cancel" });
					buttons[0].DialogResult = DialogResult.Yes;
					buttons[1].DialogResult = DialogResult.No;
					buttons[2].DialogResult = DialogResult.Cancel;
					break;
			}

			// Format button
			foreach (Button button in buttons)
			{
				button.Height = BUTTON_HEIGHT;
				button.Width = BUTTON_WIDTH;
			}

			// Add the buttons
			this.SuspendLayout();
			this.Controls.AddRange(buttons);
			this.PerformLayout();
		}

		/// <summary>
		/// Create array with specified number of buttons.
		/// </summary>
		/// <param name="count">Number of buttons to create.</param>
		/// <returns>Array of buttons.</returns>
		private Button[] CreateButtons(string[] names)
		{
			int count = names.Length;	// number of buttons

			// y-offset puts button below message
			int yOffSet = _lblMessage.Location.Y + _lblMessage.Height + PADDING;
			// x-offset dependds on width of form and number of buttons
			int xOffSet = (this.ClientSize.Width - (count * BUTTON_WIDTH) - ((count - 1) * BUTTON_SPACE)) / 2;

			Button[] buttons = new Button[count];
			Point location;
			for (int i = 0; i < count; ++i)
			{
				buttons[i] = new Button();
				buttons[i].Name = String.Format("btn{0}", i);
				buttons[i].Text = names[i];
				location = new Point(xOffSet + i * (BUTTON_WIDTH + BUTTON_SPACE), yOffSet);
				buttons[i].Location = location;
			}
			return buttons;
		}

		/// <summary>
		/// Provides same function as Form.ShowDialog, but returns a CustomDialogResult.
		/// Implements a wrapper around Form.ShowDialog.
		/// </summary>
		/// <param name="owner">Top-level owner of the modal dialog.</param>
		/// <returns>The result of the form's action.</returns>
		public CustomDialogResult ShowCustomDialog(IWin32Window owner)
		{
			CustomDialogResult customResult;
			DialogResult result = this.ShowDialog(owner);
			switch (result)
			{
				case (DialogResult.Abort):
					customResult = CustomDialogResult.Abort;
					break;
				case (DialogResult.Cancel):
					customResult = CustomDialogResult.Cancel;
					break;
				case (DialogResult.Ignore):
					customResult = CustomDialogResult.Ignore;
					break;
				case (DialogResult.No):
					customResult = CustomDialogResult.No;
					break;
				case (DialogResult.None):	// Nothing
					customResult = CustomDialogResult.None;
					break;
				case (DialogResult.OK):
					// Ok can be generated by OK, Exit or Delete button
					if (_type == CustomMessageBoxButtons.ExitCancel)
						customResult = CustomDialogResult.Exit;
					else if (_type == CustomMessageBoxButtons.DeleteCancel)
						customResult = CustomDialogResult.Delete;
					else
						customResult = CustomDialogResult.Ok;
					break;
				case (DialogResult.Retry):
					customResult = CustomDialogResult.Retry;
					break;
				case (DialogResult.Yes):
					customResult = CustomDialogResult.Yes;
					break;
				default:
					customResult = CustomDialogResult.None;
					break;
			}
			return customResult;
		}

	}	// class
}	// namespace