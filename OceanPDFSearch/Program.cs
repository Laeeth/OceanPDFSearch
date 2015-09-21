using System;
using System.Windows.Forms;

namespace OceanPDFSearch
{
	internal sealed class Program
	{
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			new StartingWindow();
			Application.Run();
		}
		
	}
}
