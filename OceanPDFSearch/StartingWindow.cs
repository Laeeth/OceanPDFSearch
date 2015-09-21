using System;
using System.Windows.Forms;

namespace OceanPDFSearch
{
	internal partial class StartingWindow : Form
	{
		internal StartingWindow()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			new MainForm().Show();
		}
		
		private void NotifyIcon1MouseClick(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Right)
			{
				if(MessageBox.Show("Do you want to exit Ocean PDF Search?", "Sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
				{
					this.Close();
					this.Dispose();
					return;
				}
				else
				{
					return;
				}
			}
			
			new MainForm().Show();
		}
	}
}
