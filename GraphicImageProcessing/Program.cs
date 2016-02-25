using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GraphicImageProcessing.ImageProcessing;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace GraphicImageProcessing
{
	static class Program
	{
		/// <summary>
		/// Главная точка входа для приложения.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Action[] arr = new Action[]{todo1, todo2};
			var str = arr[0].Method.Name;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
		private static void todo1()
		{

		}
		private static void todo2()
		{

		}
	}
}
