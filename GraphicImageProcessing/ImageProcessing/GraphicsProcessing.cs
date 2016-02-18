using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using GraphicImageProcessing.Pointer;


namespace GraphicImageProcessing.ImageProcessing
{
	[Flags]	
	public enum BitmapChanel
	{
		None  = 0x0,
		Red   = 0x1,
		Green = 0x2,
		Blue  = 0x4
	}
	public static class GraphicsProcessing
	{
		/// <summary>
		/// Up and down Brightness
		/// </summary>
		/// <param name="bitmap"></param>
		/// <returns></returns>
		public static Bitmap Brightness(Bitmap bitmap, int value)
		{
			Bitmap result = new Bitmap(bitmap);
			int len = bitmap.Width * bitmap.Height * 4;//ARGB
			//get pointer of byte array in Bitmpa
			BitmapData bitmapData = result.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
			
			Func<int, byte> intToByte = (int num) =>
			{
				if (num > 255) num = 255;
				else if (num < 0) num = 0;
				return (byte)num;
			};
			unsafe
			{
				byte* ptr = (byte*)bitmapData.Scan0.ToInt32();
				for (int i = 0; i < len; i++)
				{
					ptr[i] = intToByte(ptr[i] + value); i++;
					ptr[i] = intToByte(ptr[i] + value); i++;
					ptr[i] = intToByte(ptr[i] + value); i++;
				}
			}
			result.UnlockBits(bitmapData);
			return result;
		}
		/// <summary>
		/// Up and down Contrast
		/// </summary>
		/// <param name="bitmap"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Bitmap Contrast(Bitmap bitmap, int value)
		{		
			Bitmap result = new Bitmap(bitmap);
			int len = bitmap.Width * bitmap.Height * 4;//ARGB
			//get pointer of byte array in Bitmpa
			BitmapData bitmapData = result.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
			Func<double, byte> doubleToByte = (double num) =>
			{
				if (num > 255) num = 255;
				else if (num < 0) num = 0;
				return (byte)num;
			};
			unsafe
			{
				byte* ptr = (byte*)bitmapData.Scan0.ToInt32();
				if (value > 0)
				{
					for (int i = 0; i < len; i++)
					{
						ptr[i] = doubleToByte((ptr[i] * 100 - 128 * value) / (100D - value)); i++;
						ptr[i] = doubleToByte((ptr[i] * 100 - 128 * value) / (100D - value)); i++;
						ptr[i] = doubleToByte((ptr[i] * 100 - 128 * value) / (100D - value)); i++;
					}
				}
				else
				{
					value *= -1;
					for (int i = 0; i < len; i++)
					{
						ptr[i] = doubleToByte((ptr[i] * (100 - value) + 128 * value) / 100D); i++;
						ptr[i] = doubleToByte((ptr[i] * (100 - value) + 128 * value) / 100D); i++;
						ptr[i] = doubleToByte((ptr[i] * (100 - value) + 128 * value) / 100D); i++;
					}
				}
				
			}
			result.UnlockBits(bitmapData);
			return result;
		}

		/// <summary>
		/// Convert RGB palette to HSV
		/// </summary>
		/// <param name="R"></param>
		/// <param name="G"></param>
		/// <param name="B"></param>
		/// <returns></returns>
		public static int[] ConverRGBToHSV(byte r, byte g, byte b)
		{
			double R = r / 255D , G = g / 255D, B = b / 255D;
			double h = 0, s = 0, v = 0;
			double max = Math.Max(Math.Max(R,G),B);
			double min = Math.Min(Math.Min(R,G),B);
			v = max;
			s = max == 0 ? 0 : (int)(1 - ((double)min) / max);
			//h
			if (max == min) h = 0;
			else if (max == R && G >= B) h = (int)(60 * (G - B) / (double)(max - min));
			else if (max == R && G < B) h = (int)(60 * (G - B) / ((double)(max - min)) + 360);
			else if (max == G) h = (int)(60 * (B - R) / ((double)(max - min)) + 120);
			else if (max == B) h = (int)(60 * (R - G) / ((double)(max - min)) + 240);
			return new int[] { (int)h, (int)s * 100, (int)v * 100 };
		}
		public static int[] ConverRGBToHSV(Color color)
		{
			return ConverRGBToHSV(color.R, color.G, color.B);
		}
		/// <summary>
		/// make Gaussian Bluer
		/// </summary>
		/// <param name="bitmap"></param>
		/// <returns></returns>
		private static Bitmap GaussianBluer(Bitmap bitmap)
		{
			Bitmap result = new Bitmap(bitmap);
			return result;
		}
		/// <summary>
		/// Make from a color image black and white 
		/// </summary>
		/// <param name="bitmap"></param>
		/// <param name="gradation"></param>
		/// <returns></returns>
		public static Bitmap MakeBlackWhite(Bitmap bitmap)
		{
			Bitmap result = new Bitmap(bitmap);
			int len = bitmap.Width * bitmap.Height * 4;//ARGB
			byte color_b = 0;
			//get pointer of byte array in Bitmpa
			BitmapData bitmapData = result.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
			//unsefe because it's the fastest way to update Bitmap (see TestPerformance)
			unsafe
			{
				byte* ptr = (byte*)bitmapData.Scan0.ToInt32();;
				for (int i = 0; i < len; i++)
				{
					color_b = (byte)((ptr[i] + ptr[i + 1] + ptr[i + 2]) / 3);
					ptr[i++] = color_b;
					ptr[i++] = color_b;
					ptr[i++] = color_b;
				}
			}
			result.UnlockBits(bitmapData);
			return result;
		}
		/// <summary>
		/// Make TestBitmap with rgb spectrum
		/// </summary>
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <returns></returns>
		public static Bitmap GetTestBitmap(int w, int h)
		{
			int amountRectangles = 20;
			int rectangleSize = 15;
			Point p = new Point(rectangleSize, rectangleSize);
			int gray = 255 / (amountRectangles - 1);
			Bitmap result = new Bitmap(w, h);
			Graphics g = Graphics.FromImage(result);
			g.Clear(Color.Black);
			int[][] arrayOfColors = new int[][] 
			{
				new int[]{1,1,1},//white
				new int[]{1,0,0},//red
				new int[]{0,1,0},//green
				new int[]{0,0,1},//blue
				new int[]{0,1,1},
				new int[]{1,0,1},
				new int[]{1,1,0}
			};
			for (int i = 0; i < amountRectangles; i++)
			{
				for (int j = 0; j < arrayOfColors.Length; j++)
				{
					g.FillRectangle(
					new SolidBrush(Color.FromArgb(
						255 - arrayOfColors[j][0] * i * gray,
						255 - arrayOfColors[j][1] * i * gray,
						255 - arrayOfColors[j][2] * i * gray)),
					p.X * (i + 1), p.Y * (j + 1) + j, rectangleSize, rectangleSize);
				}
			}
			return result;
		}
		/// <summary>
		/// Choose channel in Bitmap
		/// </summary>
		/// <param name="bitmap"></param>
		/// <param name="bitmapChanel"></param>
		public static Bitmap ChooseChannel(Bitmap bitmap, BitmapChanel bitmapChanel)
		{
			Bitmap result = new Bitmap(bitmap);
			int len = bitmap.Width * bitmap.Height * 4;//ARGB
			//get pointer of byte array in Bitmpa
			BitmapData bitmapData = result.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
			//unsefe because it's the fastest way to update Bitmap (see TestPerformance)
			unsafe
			{
				byte* ptr = (byte*)bitmapData.Scan0.ToInt32();
				if ((bitmapChanel & BitmapChanel.Red) != BitmapChanel.Red)
					for (int i = 2; i < len; i += 4) 
						ptr[i] = 0;
				if ((bitmapChanel & BitmapChanel.Green) != BitmapChanel.Green) 
					for (int i = 1; i < len; i += 4) 
						ptr[i] = 0;
				if ((bitmapChanel & BitmapChanel.Blue) != BitmapChanel.Blue)
					for (int i = 0; i < len; i += 4) 
						ptr[i] = 0;				
			}
			result.UnlockBits(bitmapData);


			return result;
		}
		
	}
}
