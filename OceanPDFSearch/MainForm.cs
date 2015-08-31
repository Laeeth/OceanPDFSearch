using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
		private string pdfViewer = @"c:\Program Files\Tracker Software\PDF Editor\PDFXEdit.exe";
		private string pdfViewerArguments = @"/A pagemode=none&view=FitH&search=""{1}"" ""{0}""";
		private string workingDirectory = Environment.CurrentDirectory;
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
			
			this.Text = string.Format("Ocean PDF Search v{0}@{1}", Assembly.GetExecutingAssembly().getAppVersion(), Assembly.GetAssembly(OceanSearchManager.INSTANCE.GetType()).getAppVersion());
			OceanSearchManager.INSTANCE.Setup(this.workingDirectory);
			
			// Read the user's pdf viewer:
			this.pdfViewer = global::OceanPDFSearch.Settings1.Default.PDFViewer.Trim();
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
			// Start & dock the next app:
			this.dock(filename, searchString);
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
			var searchString = this.textBox1.Text;
			
			if(this.dockedProcess != null)
			{
				// Start a new thread to keep the GUI responsive:
				Task.Run(() => {
				         	
		         	var selectedFileInner = selectedFile;
		         	var searchStringInnner = searchString;
		         	
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
	         		
	         		this.startApp(selectedFileInner, searchStringInnner);
		        });
			}
			else
			{
				this.startApp(selectedFile, searchString);
			}
		}
		
		void PanelTargetSizeChanged(object sender, EventArgs e)
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
		
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			// Close first the external process:
			closeApp();
		}
		
		void ButtonSelectPDFViewerClick(object sender, EventArgs e)
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
	}
}
