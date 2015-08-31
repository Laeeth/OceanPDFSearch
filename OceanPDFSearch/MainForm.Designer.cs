/*
 * Created by SharpDevelop.
 * User: TSLocal
 * Date: 29.08.2015
 * Time: 21:25
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace OceanPDFSearch
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button buttonSearch;
		private System.Windows.Forms.TextBox textBoxSearchFor;
		private System.Windows.Forms.ListBox listBoxResults;
		private System.Windows.Forms.Button buttonIndex;
		private System.Windows.Forms.Panel panelTarget;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Button buttonSelectPDFViewer;
		private System.Windows.Forms.Button buttonClearHistory;
		private System.Windows.Forms.Button buttonWebsite;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.buttonSearch = new System.Windows.Forms.Button();
			this.textBoxSearchFor = new System.Windows.Forms.TextBox();
			this.buttonIndex = new System.Windows.Forms.Button();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.listBoxResults = new System.Windows.Forms.ListBox();
			this.panelTarget = new System.Windows.Forms.Panel();
			this.buttonSelectPDFViewer = new System.Windows.Forms.Button();
			this.buttonClearHistory = new System.Windows.Forms.Button();
			this.buttonWebsite = new System.Windows.Forms.Button();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 5;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 166F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 89F));
			this.tableLayoutPanel1.Controls.Add(this.buttonSearch, 4, 0);
			this.tableLayoutPanel1.Controls.Add(this.textBoxSearchFor, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonIndex, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.buttonSelectPDFViewer, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.buttonClearHistory, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.buttonWebsite, 4, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(873, 540);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// buttonSearch
			// 
			this.buttonSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.buttonSearch.Location = new System.Drawing.Point(787, 3);
			this.buttonSearch.Name = "buttonSearch";
			this.buttonSearch.Size = new System.Drawing.Size(83, 29);
			this.buttonSearch.TabIndex = 1;
			this.buttonSearch.Text = "Search";
			this.buttonSearch.UseVisualStyleBackColor = true;
			this.buttonSearch.Click += new System.EventHandler(this.ButtonSearchClick);
			// 
			// textBoxSearchFor
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.textBoxSearchFor, 4);
			this.textBoxSearchFor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxSearchFor.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBoxSearchFor.Location = new System.Drawing.Point(3, 3);
			this.textBoxSearchFor.Name = "textBoxSearchFor";
			this.textBoxSearchFor.Size = new System.Drawing.Size(778, 29);
			this.textBoxSearchFor.TabIndex = 0;
			this.textBoxSearchFor.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBox1KeyUp);
			// 
			// buttonIndex
			// 
			this.buttonIndex.Dock = System.Windows.Forms.DockStyle.Fill;
			this.buttonIndex.Location = new System.Drawing.Point(3, 508);
			this.buttonIndex.Name = "buttonIndex";
			this.buttonIndex.Size = new System.Drawing.Size(64, 29);
			this.buttonIndex.TabIndex = 3;
			this.buttonIndex.TabStop = false;
			this.buttonIndex.Text = "Index";
			this.buttonIndex.UseVisualStyleBackColor = true;
			this.buttonIndex.Click += new System.EventHandler(this.ButtonIndexClick);
			// 
			// splitContainer1
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.splitContainer1, 5);
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(3, 38);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.listBoxResults);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.panelTarget);
			this.splitContainer1.Panel2MinSize = 600;
			this.splitContainer1.Size = new System.Drawing.Size(867, 464);
			this.splitContainer1.SplitterDistance = 263;
			this.splitContainer1.SplitterWidth = 16;
			this.splitContainer1.TabIndex = 5;
			this.splitContainer1.TabStop = false;
			// 
			// listBoxResults
			// 
			this.listBoxResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBoxResults.FormattingEnabled = true;
			this.listBoxResults.HorizontalScrollbar = true;
			this.listBoxResults.ItemHeight = 18;
			this.listBoxResults.Location = new System.Drawing.Point(0, 0);
			this.listBoxResults.Name = "listBoxResults";
			this.listBoxResults.Size = new System.Drawing.Size(263, 464);
			this.listBoxResults.TabIndex = 2;
			this.listBoxResults.SelectedValueChanged += new System.EventHandler(this.ListBoxResultsSelectedValueChanged);
			// 
			// panelTarget
			// 
			this.panelTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelTarget.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelTarget.Location = new System.Drawing.Point(0, 0);
			this.panelTarget.Name = "panelTarget";
			this.panelTarget.Size = new System.Drawing.Size(600, 464);
			this.panelTarget.TabIndex = 5;
			this.panelTarget.SizeChanged += new System.EventHandler(this.PanelTargetSizeChanged);
			// 
			// buttonSelectPDFViewer
			// 
			this.buttonSelectPDFViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.buttonSelectPDFViewer.Location = new System.Drawing.Point(73, 508);
			this.buttonSelectPDFViewer.Name = "buttonSelectPDFViewer";
			this.buttonSelectPDFViewer.Size = new System.Drawing.Size(160, 29);
			this.buttonSelectPDFViewer.TabIndex = 6;
			this.buttonSelectPDFViewer.TabStop = false;
			this.buttonSelectPDFViewer.Text = "Select PDF Viewer";
			this.buttonSelectPDFViewer.UseVisualStyleBackColor = true;
			this.buttonSelectPDFViewer.Click += new System.EventHandler(this.ButtonSelectPDFViewerClick);
			// 
			// buttonClearHistory
			// 
			this.buttonClearHistory.Dock = System.Windows.Forms.DockStyle.Fill;
			this.buttonClearHistory.Location = new System.Drawing.Point(239, 508);
			this.buttonClearHistory.Name = "buttonClearHistory";
			this.buttonClearHistory.Size = new System.Drawing.Size(134, 29);
			this.buttonClearHistory.TabIndex = 7;
			this.buttonClearHistory.TabStop = false;
			this.buttonClearHistory.Text = "Clear History";
			this.buttonClearHistory.UseVisualStyleBackColor = true;
			this.buttonClearHistory.Click += new System.EventHandler(this.ButtonClearHistoryClick);
			// 
			// buttonWebsite
			// 
			this.buttonWebsite.Dock = System.Windows.Forms.DockStyle.Fill;
			this.buttonWebsite.Location = new System.Drawing.Point(787, 508);
			this.buttonWebsite.Name = "buttonWebsite";
			this.buttonWebsite.Size = new System.Drawing.Size(83, 29);
			this.buttonWebsite.TabIndex = 8;
			this.buttonWebsite.TabStop = false;
			this.buttonWebsite.Text = "Website";
			this.buttonWebsite.UseVisualStyleBackColor = true;
			this.buttonWebsite.Click += new System.EventHandler(this.ButtonWebsiteClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(873, 540);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Ocean PDF Search";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	}
}
