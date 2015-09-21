using System;
using System.Windows.Forms;
using log4net.Core;
using log4net.Appender;

namespace Renamer.NewClasses.Logging {
	class TextBoxAppender : AppenderSkeleton {

		protected TextBoxBase someTextBox;

		public string FormName {
			get;
			set;
		}

		public string TextBoxName {
			get;
			set;
		}

		protected virtual void initTextBox() {
			if (someTextBox != null) {
				return;
			}
			if (string.IsNullOrEmpty(FormName) ||
				string.IsNullOrEmpty(TextBoxName)) {
				return;
			}
			Form form = Application.OpenForms[FormName];
			if (form == null) {
				return;
			}
			someTextBox = (TextBoxBase)form.Controls[TextBoxName];
			if (someTextBox == null) {
				return;
			}
			form.FormClosing += 
				(sender, closingEvent) => 
					someTextBox = null;
		}

		protected override void Append(LoggingEvent loggingEvent) {
			someTextBox.AppendText(loggingEvent.RenderedMessage + Environment.NewLine);
			// DP: Vielleicht muss das hier noch eingearbeitet werden
			//MethodInvoker logDelegate = delegate {
			//	textBox.Text = level.toString() + ": " + strLogMessage + System.Environment.NewLine + textBox.Text;
			//};
			//if (textBox.InvokeRequired)
			//	textBox.Invoke(logDelegate);
			//else
			//	logDelegate();
		}
	}
}
