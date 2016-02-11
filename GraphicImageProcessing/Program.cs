using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GraphicImageProcessing.Pointer;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;

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
		private static void TestPointer()
		{
			Bitmap bm = new Bitmap(10, 10);
			int len = bm.Width * bm.Height * 4;//ARGB
			Graphics.FromImage(bm).Clear(Color.LightBlue);
			Graphics.FromImage(bm).DrawLine(new Pen(Color.Green, 1), 1, 1, 4, 1);
			var bd = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

			ObjectPointer op = new ObjectPointer(bd);
			op.IntPointer = bd.Scan0.ToInt32();
			var temp = op.GetArrayPointer<byte>();
			for (int i = 0; i < len; i++)
			{
				System.Diagnostics.Debug.WriteLine(temp[i]);
			}
		}
		private static void testUnsafe()
		{
			Bitmap bm = new Bitmap(10, 10);
			int len = bm.Width * bm.Height * 4;//ARGB
			Graphics.FromImage(bm).Clear(Color.LightBlue);
			Graphics.FromImage(bm).DrawLine(new Pen(Color.Green, 1), 1, 1, 4, 1);
			var bd = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

			ObjectPointer op = new ObjectPointer(bd);
			op.IntPointer = bd.Scan0.ToInt32();
			unsafe
			{
				byte* ptr = (byte*)op.IntPointer;
				for (int i = 0; i < 100; i++)
				{
					System.Diagnostics.Debug.WriteLine(ptr[i]);
				}
			}
 
		}
	}
}
