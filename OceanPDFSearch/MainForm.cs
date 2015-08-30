using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using OceanPDFSearchLib;
using System.Linq;
using NETools;

namespace OceanPDFSearch
{
	public partial class MainForm : Form
	{
		private string workingDirectory = Environment.CurrentDirectory;
		private object locking = new object();
		private IndexWindow currentIndexWindow = null;
		private bool indexingIsRunning = false;
		private bool isSearching = false;
		private Task<string[]> searcher = null;
		private Task indexer = null;
		
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
			lock(this.locking)
			{
				if(this.isSearching)
				{
					return;
				}
				
				var searchFor = this.textBox1.Text;
				if(searchFor.Trim().Length == 0)
				{
					return;
				}
				
				this.isSearching = true;
				this.searcher = Task.Run<string[]>(() => {
	               	if(searchFor.Contains("|"))
	               	{
	               		//
	               		// Nested Search
	               		//
	               		
	               		var searches = searchFor.Split('|');
	               		var matchingDocs = new string[0];
	               		var firstSearchExecuted = false;
	               		foreach(var search in searches)
	               		{
	               			if(firstSearchExecuted && matchingDocs.Length == 0)
	               			{
	               				return matchingDocs;
	               			}
	               			
	               			if(matchingDocs.Length == 0)
	               			{
	               				matchingDocs = this.executeSingleSearch(search.Trim());
	               				firstSearchExecuted = true;
	               			}
	               			else
	               			{
	               				matchingDocs = this.executeSingleSearch(search.Trim(), matchingDocs);
	               			}
	               		}
	               		
	               		return matchingDocs;
	               	}
	               	else
	               	{
	               		//
			       		// Single search
			       		//
			       		
	               		return this.executeSingleSearch(searchFor);
	               	}                          	
               	});
				
				this.searcher.GetAwaiter().OnCompleted(this.addSearchResults);
			}
		}
		
		private string[] executeSingleSearch(string searchFor, string[] docs = null)
		{
			//
       		// Single search
       		//
       		
       		// Proximity search?
       		if (searchFor.Contains(">"))
			{
				var elements = searchFor.Split('<', '>');
				var word1 = elements[0].Trim();
				var word2 = elements[2].Trim();
				var distance = int.Parse(elements[1].Trim());
				
				return docs == null ? OceanSearchManager.INSTANCE.FindDocumentsProximitySearch(word1, word2, distance) : OceanSearchManager.INSTANCE.FindDocumentsProximitySearch(docs, word1, word2, distance);
			}
       		
       		// Normal AND search?
			else
			{
				return docs == null ? OceanSearchManager.INSTANCE.FindDocumentsContainAllWords(searchFor.Split(' ')) : OceanSearchManager.INSTANCE.FindDocumentsContainAllWords(docs, searchFor.Split(' '));
			}
		}
		
		private void addSearchResults()
		{
			if(this.listBoxResults.InvokeRequired)
			{
				this.listBoxResults.Invoke(new MethodInvoker(this.addSearchResults));
                return;
			}
			
			this.listBoxResults.Items.Clear();
			this.listBoxResults.Items.AddRange(this.searcher.Result);
			this.searcher.Dispose();
			this.searcher = null;
			this.isSearching = false;
		}
		
		void ButtonIndexClick(object sender, EventArgs e)
		{
			lock(this.locking)
			{
				if(this.indexingIsRunning)
				{
					return;
				}
				
				this.currentIndexWindow = new IndexWindow();
				this.currentIndexWindow.Show(this);
			
				this.indexer = Task.Factory.StartNew(() => OceanSearchManager.INSTANCE.IndexNow(new string[] { this.workingDirectory }, progressUpdate), TaskCreationOptions.LongRunning);
				this.indexer.GetAwaiter().OnCompleted(this.indexingDone);
				this.indexingIsRunning = true;
			}
		}
		
		void indexingDone()
		{
			this.indexingIsRunning = false;
			this.currentIndexWindow.Close();
			this.currentIndexWindow.Dispose();
			this.currentIndexWindow = null;
		}
		
		bool progressUpdate(byte directories, byte files, int dirNow, int dirTotal, int filesNow, int filesTotal)
		{
			this.currentIndexWindow.setProgress(directories, files, dirNow, dirTotal, filesNow, filesTotal);
			return true;
		}
		
		void ListBoxResultsSelectedValueChanged(object sender, EventArgs e)
		{
			var selectedFile = this.listBoxResults.SelectedItem as string;
			Process.Start(@"c:\Program Files\Tracker Software\PDF Editor\PDFXEdit.exe", string.Format(@"/A search=""{1}"" ""{0}""", selectedFile, this.textBox1.Text));
		}
	}
}
