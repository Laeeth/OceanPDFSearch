using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using OceanPDFSearchLib;
using NETools;

namespace OceanPDFSearch
{
	public partial class MainForm : Form
	{
		private string workingDirectory = Environment.CurrentDirectory;
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			this.Text = string.Format("Ocean PDF Search v{0}@v{1}", Assembly.GetExecutingAssembly().getAppVersion(), Assembly.GetAssembly(OceanSearchManager.INSTANCE.GetType()).getAppVersion());
			OceanSearchManager.INSTANCE.Setup(this.workingDirectory);
		}
		
		void TextBox1KeyUp(object sender, KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter)
			{
				this.ButtonSearchClick(sender, e);
			}
		}
		
		void ButtonSearchClick(object sender, EventArgs e)
		{
			var searchFor = this.textBox1.Text;
			var result = new string[0];
			
			if (searchFor.Contains(">"))
			{
				var elements = searchFor.Split('<', '>');
				var word1 = elements[0].Trim();
				var word2 = elements[2].Trim();
				var distance = int.Parse(elements[1].Trim());
				result = OceanSearchManager.INSTANCE.FindDocumentsProximitySearch(word1, word2, distance);
			}
			else
			{
				result = OceanSearchManager.INSTANCE.FindDocumentsContainAllWords(searchFor.Split(' '));
			}
			
			this.listBoxResults.Items.Clear();
			this.listBoxResults.Items.AddRange(result);
		}
		
		void ButtonIndexClick(object sender, EventArgs e)
		{
			//OceanSearchManager.INSTANCE.IndexNow();
		}
		
		void ListBoxResultsSelectedValueChanged(object sender, EventArgs e)
		{
			var selectedFile = this.listBoxResults.SelectedItem as string;
			Process.Start(@"c:\Program Files\Tracker Software\PDF Editor\PDFXEdit.exe", string.Format(@"/A search=""{1}"" ""{0}""", selectedFile, this.textBox1.Text));
		}
	}
}
