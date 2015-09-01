using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OceanPDFSearchLib;
using System.Linq;
using NETools;
using System.Runtime.InteropServices;

namespace OceanPDFSearch
{
	public partial class MainForm : Form
	{
		private bool useEmbeddedPDFMode = true;
		private string pdfViewer = string.Empty;
		private string pdfViewerArguments = @"/A pagemode=none&view=FitH&search=""{1}"" ""{0}""";
		private string workingDirectory = Environment.CurrentDirectory;
		private List<string> history = new List<string>();
		private int historyCursor = 0;
		private object locking = new object();
		private IndexWindow currentIndexWindow = null;
		private bool indexingIsRunning = false;
		private bool isSearching = false;
		private Task<string[]> searcher = null;
		private Task indexer = null;
		private Process dockedProcess = null;
		private IntPtr dockedHandle = IntPtr.Zero;
		
		private const int GWL_STYLE = -16;
		private const int WS_VISIBLE = 0x10000000;
		private const int WM_CLOSE = 0x10;
		
		[DllImport("user32.dll")]
		public static extern IntPtr SetParent(IntPtr hwndChild, IntPtr hwndNewParent);
		
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool MoveWindow(IntPtr hwnd, int x, int y, int width, int height, bool repaint);
		
		[DllImport("user32.dll", EntryPoint="SetWindowLongA", SetLastError=true)]
		private static extern long SetWindowLong(IntPtr hwnd, int index, long newValue);
		
		[DllImport("user32.dll", EntryPoint="PostMessageA", SetLastError=true)]		
		private static extern bool PostMessage(IntPtr hwnd, uint message, long param1, long param2);
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			// Set the title:
			this.Text = string.Format("Ocean PDF Search v{0}@{1}", Assembly.GetExecutingAssembly().getAppVersion(), Assembly.GetAssembly(OceanSearchManager.INSTANCE.GetType()).getAppVersion());
			
			// Normalize the working directory:
			this.workingDirectory = this.workingDirectory.Last() == Path.DirectorySeparatorChar ? this.workingDirectory : this.workingDirectory + Path.DirectorySeparatorChar;
			
			// Setup the search library:
			OceanSearchManager.INSTANCE.Setup(this.workingDirectory);
			
			// Re-set the splitter's width:
			this.splitContainer1.SplitterWidth = 16;
			
			// Read the user's pdf viewer:
			this.pdfViewer = global::OceanPDFSearch.Settings1.Default.PDFViewer.Trim();
			
			// Read the user's history:
			var countHistoryEntries = global::OceanPDFSearch.Settings1.Default.History.Count;
			var historyEntries = new string[countHistoryEntries];
			global::OceanPDFSearch.Settings1.Default.History.CopyTo(historyEntries, 0);
			this.history.AddRange(historyEntries);
		}
		
		private void dock(string filename, string searchString)
		{
			// Is already a process docked?
		    if (this.dockedHandle != IntPtr.Zero)
		    {
		        return;
		    }
		    
		    // Start the process:
		    this.dockedProcess = Process.Start(this.pdfViewer, string.Format(this.pdfViewerArguments, filename, searchString));
		    
		    // Wait to get the process ready:
		    while (this.dockedHandle == IntPtr.Zero)
		    {
		    	// Wait for the process:
		        this.dockedProcess.WaitForInputIdle(1000);
		        this.dockedProcess.Refresh();
		        
		        // Has the process ended?
		        if (this.dockedProcess.HasExited)
		        {
		            return;
		        }
		        
		        // Store the handle:
		        this.dockedHandle = this.dockedProcess.MainWindowHandle;
		    }
		    
		    // Remove the border:
			SetWindowLong(this.dockedHandle, GWL_STYLE, WS_VISIBLE);
		    
		    // Set the new parent to the panelTarget:
		    SetParent(this.dockedHandle, this.panelTarget.Handle);
		    
		    // Try to move the process into the panel:
		    this.fitPDFIntoPanel();
		}
		
		// Start the next selected app:
		private void startApp(string filename, string searchString)
		{
			if(this.useEmbeddedPDFMode)
			{
				// Start & dock the next app:
				this.dock(filename, searchString);
			}
			else
			{
				this.dockedProcess = Process.Start(this.pdfViewer, string.Format(this.pdfViewerArguments, filename, searchString));
			}
		}
		
		// Close an app before closing the main program:
		private void closeApp()
		{
			if(this.dockedProcess != null)
			{ 	
			    try
			    {
					PostMessage(this.dockedHandle, WM_CLOSE, 0, 0);
			    }
			    catch
			    {
			    }
				
				try
				{
         			this.dockedProcess.WaitForExit(1000);
				}
				catch
				{
				}
         		
				try
				{
					this.dockedProcess.Kill();
				}
				catch
				{
				}
				
         		try
         		{
					this.dockedProcess = null;
					this.dockedHandle = IntPtr.Zero;
         		}
         		catch
         		{
         		}
			}
		}
		
		private void TextBox1KeyUp(object sender, KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter)
			{
				this.ButtonSearchClick(sender, e);
			}
			
			if(e.KeyCode == Keys.Up)
			{
				this.textBoxSearchFor.Text = this.history.Count > 0 ? Enumerable.Reverse(this.history).Skip(Math.Abs(this.historyCursor++) % this.history.Count).First() : string.Empty;
			}
			
			if(e.KeyCode == Keys.Down)
			{
				this.textBoxSearchFor.Text = this.history.Count > 0 ? Enumerable.Reverse(this.history).Skip(Math.Abs(this.historyCursor--) % this.history.Count).First() : string.Empty;
			}
		}
		
		private void ButtonSearchClick(object sender, EventArgs e)
		{
			lock(this.locking)
			{
				if(this.isSearching)
				{
					return;
				}
				
				var searchFor = this.textBoxSearchFor.Text;
				if(searchFor.Trim().Length == 0)
				{
					return;
				}
				
				this.history.Add(this.textBoxSearchFor.Text);
				this.historyCursor = 0;
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
			var results = this.searcher.Result;
			foreach(var item in results)
			{
				this.listBoxResults.Items.Add(item.Replace(this.workingDirectory, string.Empty));
			}
			
			this.searcher.Dispose();
			this.searcher = null;
			this.isSearching = false;
		}
		
		private void ButtonIndexClick(object sender, EventArgs e)
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
		
		private void indexingDone()
		{
			this.indexingIsRunning = false;
			this.currentIndexWindow.Close();
			this.currentIndexWindow.Dispose();
			this.currentIndexWindow = null;
		}
		
		private bool progressUpdate(byte directories, byte files, int dirNow, int dirTotal, int filesNow, int filesTotal)
		{
			this.currentIndexWindow.setProgress(directories, files, dirNow, dirTotal, filesNow, filesTotal);
			return true;
		}
		
		private void ListBoxResultsSelectedValueChanged(object sender, EventArgs e)
		{
			var selectedFile = this.listBoxResults.SelectedItem as string;
			var searchString = this.textBoxSearchFor.Text;
			
			if(this.dockedProcess != null)
			{
				// Start a new thread to keep the GUI responsive:
				Task.Run(() => {
				         	
		         	var selectedFileInner = selectedFile;
		         	var searchStringInnner = searchString;
		         	
				    try
				    {
				    	if(this.dockedHandle != IntPtr.Zero)
				    	{
							PostMessage(this.dockedHandle, WM_CLOSE, 0, 0);
				    	}
				    }
				    catch
				    {
				    }
					
					try
					{
	         			this.dockedProcess.WaitForExit(1000);
					}
					catch
					{
					}
	         		
					try
					{
						this.dockedProcess.Kill();
					}
					catch
					{
					}
					
	         		try
	         		{
						this.dockedProcess = null;
						this.dockedHandle = IntPtr.Zero;
	         		}
	         		catch
	         		{
	         		}
	         		
	         		this.startApp(selectedFileInner, searchStringInnner);
		        });
			}
			else
			{
				this.startApp(selectedFile, searchString);
			}
		}
		
		private void PanelTargetSizeChanged(object sender, EventArgs e)
		{
			this.fitPDFIntoPanel();
		}
		
		private void fitPDFIntoPanel()
		{
			if(this.panelTarget.InvokeRequired)
			{
				this.panelTarget.Invoke(new MethodInvoker(this.fitPDFIntoPanel));
                return;
			}	
			
			if(this.dockedProcess != null && this.dockedHandle != IntPtr.Zero && !this.dockedProcess.HasExited)
			{
				// Ensure that the process matches into the panel: 
		    	MoveWindow(this.dockedHandle, 0, 0, this.panelTarget.Width, this.panelTarget.Height, true);
			}
		}
		
		private void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			// Save the history:
			global::OceanPDFSearch.Settings1.Default.History.Clear();
			global::OceanPDFSearch.Settings1.Default.History.AddRange(this.history.ToArray());
			global::OceanPDFSearch.Settings1.Default.Save();
			
			// Close first the external process:
			closeApp();
		}
		
		private void ButtonSelectPDFViewerClick(object sender, EventArgs e)
		{
			var open = new OpenFileDialog();
			open.CheckFileExists = true;
			open.DefaultExt = "*.exe";
			open.FileName = this.pdfViewer.Trim();
			open.InitialDirectory = this.pdfViewer.Trim();
			open.Filter = "Applications (*.exe)|*.exe";
			open.Multiselect = false;
			open.Title = "Please select your PDF viewer";
			
			if(open.ShowDialog(this) == DialogResult.OK)
			{
				this.pdfViewer = open.FileName.Trim();
				global::OceanPDFSearch.Settings1.Default.PDFViewer = open.FileName.Trim();
				global::OceanPDFSearch.Settings1.Default.Save();
			}
		}
		
		private void ButtonClearHistoryClick(object sender, EventArgs e)
		{
			global::OceanPDFSearch.Settings1.Default.History.Clear();
			global::OceanPDFSearch.Settings1.Default.Save();
			this.historyCursor = 0;
			this.history.Clear();
		}
		
		private void ButtonWebsiteClick(object sender, EventArgs e)
		{
			Process.Start("https://github.com/SommerEngineering/OceanPDFSearch");
		}
		
		private void ButtonChangePDFModeClick(object sender, EventArgs e)
		{
			this.useEmbeddedPDFMode = !this.useEmbeddedPDFMode;
			if(this.useEmbeddedPDFMode)
			{
				this.buttonChangePDFMode.Text = "Embedded PDFs";
			}
			else
			{
				this.buttonChangePDFMode.Text = "External PDFs";
			}
		}
	}
}
