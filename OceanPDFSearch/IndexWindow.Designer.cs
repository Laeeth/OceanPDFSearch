/*
 * Created by SharpDevelop.
 * User: TSLocal
 * Date: 29.08.2015
 * Time: 22:17
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace OceanPDFSearch
{
	partial class IndexWindow
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.ProgressBar progressBarDirectories;
		private System.Windows.Forms.ProgressBar progressBarFiles;
		private System.Windows.Forms.Label labelDirectories;
		private System.Windows.Forms.Label labelFiles;
		
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
			this.progressBarDirectories = new System.Windows.Forms.ProgressBar();
			this.progressBarFiles = new System.Windows.Forms.ProgressBar();
			this.labelDirectories = new System.Windows.Forms.Label();
			this.labelFiles = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.progressBarDirectories, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.progressBarFiles, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.labelDirectories, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.labelFiles, 0, 3);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 6;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(533, 180);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// progressBarDirectories
			// 
			this.progressBarDirectories.Dock = System.Windows.Forms.DockStyle.Fill;
			this.progressBarDirectories.Location = new System.Drawing.Point(3, 58);
			this.progressBarDirectories.Name = "progressBarDirectories";
			this.progressBarDirectories.Size = new System.Drawing.Size(527, 29);
			this.progressBarDirectories.TabIndex = 0;
			// 
			// progressBarFiles
			// 
			this.progressBarFiles.Dock = System.Windows.Forms.DockStyle.Fill;
			this.progressBarFiles.Location = new System.Drawing.Point(3, 128);
			this.progressBarFiles.Name = "progressBarFiles";
			this.progressBarFiles.Size = new System.Drawing.Size(527, 29);
			this.progressBarFiles.TabIndex = 1;
			// 
			// labelDirectories
			// 
			this.labelDirectories.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelDirectories.Location = new System.Drawing.Point(3, 20);
			this.labelDirectories.Name = "labelDirectories";
			this.labelDirectories.Size = new System.Drawing.Size(527, 35);
			this.labelDirectories.TabIndex = 2;
			this.labelDirectories.Text = "Current Directory: 1/1";
			this.labelDirectories.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelFiles
			// 
			this.labelFiles.Dock = System.Windows.Forms.DockStyle.Fill;
			this.labelFiles.Location = new System.Drawing.Point(3, 90);
			this.labelFiles.Name = "labelFiles";
			this.labelFiles.Size = new System.Drawing.Size(527, 35);
			this.labelFiles.TabIndex = 3;
			this.labelFiles.Text = "Current Directory\'s Files: 1/5405";
			this.labelFiles.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// IndexWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(533, 180);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.Name = "IndexWindow";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Indexing Progress";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.IndexWindowFormClosing);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	}
}
