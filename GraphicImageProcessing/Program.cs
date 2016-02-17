﻿using System;
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
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());

		}
	}
}
