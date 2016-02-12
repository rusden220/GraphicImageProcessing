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
//for performance test
#if DEBUG		
		public static Bitmap OptimisationEasy(Bitmap bitmap)
		{
			Bitmap result = new Bitmap(bitmap);
			//get pointer via BitmapData
			BitmapData bmpData = result.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
			IntPtr ptr = bmpData.Scan0;
			
			int numBytes = bmpData.Stride * bitmap.Height;
			int widthBytes = bmpData.Stride;
			byte[] rgbValues = new byte[numBytes];
			
			Marshal.Copy(ptr, rgbValues, 0, numBytes);
			byte color_b = 0;
			
			for (int i = 0; i < rgbValues.Length; i++)
			{
				int value = rgbValues[i] + rgbValues[i + 1] + rgbValues[i + 2];	
				color_b = Convert.ToByte(value / 3);
				rgbValues[i] = color_b;
				rgbValues[++i] = color_b;
				rgbValues[++i] = color_b;
				i++;
			}			
			Marshal.Copy(rgbValues, 0, ptr, numBytes);
			result.UnlockBits(bmpData);
			return result;
		}
		public static Bitmap OptimisationUnsafe(Bitmap bitmap)
		{
			Bitmap result = new Bitmap(bitmap);
			int len = bitmap.Width * bitmap.Height * 4;//ARGB
			byte color_b = 0;

			BitmapData bitmapData = result.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

			ObjectPointer op = new ObjectPointer(bitmapData);
			op.IntPointer = bitmapData.Scan0.ToInt32();

			unsafe
			{
				byte* ptr = (byte*)op.IntPointer;
				for (int i = 0; i < len; i++)
				{
					int value = ptr[i] + ptr[i + 1] + ptr[i + 2];
					color_b = Convert.ToByte(value / 3);
					ptr[i++] = color_b;
					ptr[i++] = color_b;
					ptr[i++] = color_b;
				}
			}
			result.UnlockBits(bitmapData);
			return result;
		}
		public static Bitmap OptimisationPointer(Bitmap bitmap)
		{
			Bitmap result = new Bitmap(bitmap);
			int len = bitmap.Width * bitmap.Height * 4;//ARGB
			byte color_b = 0;

			BitmapData bitmapData = result.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

			ObjectPointer op = new ObjectPointer(bitmapData);
			op.IntPointer = bitmapData.Scan0.ToInt32();

			byte[] ptr = op.GetArrayPointer<byte>();
			for (int i = 0; i < len; i++)
			{
				int value = ptr[i] + ptr[i + 1] + ptr[i + 2];
				color_b = Convert.ToByte(value / 3);
				ptr[i++] = color_b;
				ptr[i++] = color_b;
				ptr[i++] = color_b;
			}
			result.UnlockBits(bitmapData);
			return result;
		}
#endif

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
