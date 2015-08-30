using System;
using System.Drawing;
using System.Windows.Forms;

namespace OceanPDFSearch
{
	public partial class IndexWindow : Form
	{
		private int directoryProgressPercent = 0;
		private int fileProgressPercent = 0;
		private int dirNow = 0;
		private int dirTotal = 0;
		private int filesNow = 0;
		private int filesTotal = 0;
		
		public IndexWindow()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
		}
		
		void IndexWindowFormClosing(object sender, FormClosingEventArgs e)
		{
			if(this.progressBarDirectories.Value < 99 || this.progressBarFiles.Value < 99)
			{
				e.Cancel = true;
			}
		}
		
		private void updateProgressUI()
		{
			if (this.progressBarFiles.InvokeRequired)
            {
                this.progressBarFiles.Invoke(new MethodInvoker(updateProgressUI));
                return;
            }

			this.progressBarFiles.Value = this.fileProgressPercent;
			this.progressBarDirectories.Value = this.directoryProgressPercent;
			this.labelDirectories.Text = string.Format("Current Directory: {0}/{1}", this.dirNow, this.dirTotal);
			this.labelFiles.Text = string.Format("Current Directory's Files: {0}/{1}", this.filesNow, this.filesTotal);
		}
		
		public void setProgress(int progressDirectoriesPercent, int progressFilesPercent, int dirNow, int dirTotal, int filesNow, int filesTotal)
		{
			this.directoryProgressPercent = progressDirectoriesPercent;
			this.fileProgressPercent = progressFilesPercent;
			this.dirNow = dirNow;
			this.dirTotal = dirTotal;
			this.filesNow = filesNow;
			this.filesTotal = filesTotal;
			this.updateProgressUI();
		}
	}
}
