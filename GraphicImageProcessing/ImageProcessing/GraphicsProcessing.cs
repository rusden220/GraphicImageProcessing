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
		public static bool isRelease()
		{
#if DEBUG
			return false;
#else
			return true;
#endif

		}
		public static Bitmap OptimisationEasy(Bitmap bitmap)
		{
			Bitmap result = new Bitmap(bitmap);
			//get pointer via BitmapData
			BitmapData bmpData = result.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
			IntPtr ptr = bmpData.Scan0;
			// Задаём массив из Byte и помещаем в него надор данных.
			// int numBytes = bmp.Width * bmp.Height * 3; 
			//На 3 умножаем - поскольку RGB цвет кодируется 3-мя байтами
			//Либо используем вместо Width - Stride
			int numBytes = bmpData.Stride * bitmap.Height;
			int widthBytes = bmpData.Stride;
			byte[] rgbValues = new byte[numBytes];
			// Копируем значения в массив.
			Marshal.Copy(ptr, rgbValues, 0, numBytes);
			byte color_b = 0;
			// Перебираем пикселы по 3 байта на каждый и меняем значения
			for (int i = 0; i < rgbValues.Length; i++)
			{
				int value = rgbValues[i] + rgbValues[i + 1] + rgbValues[i + 2];	
				color_b = Convert.ToByte(value / 3);
				rgbValues[i] = color_b;
				rgbValues[++i] = color_b;
				rgbValues[++i] = color_b;
				i++;
			}
			// Копируем набор данных обратно в изображение
			Marshal.Copy(rgbValues, 0, ptr, numBytes);
			// Разблокируем набор данных изображения в памяти.
			result.UnlockBits(bmpData);
			return result;
		}
//#if DEBUG
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
					ptr[i] = color_b;
					ptr[++i] = color_b;
					ptr[++i] = color_b;
					i++;
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

			var ptr = op.GetArrayPointer<byte>();
			for (int i = 0; i < len; i++)
			{
				int value = ptr[i] + ptr[i + 1] + ptr[i + 2];
				color_b = Convert.ToByte(value / 3);
				ptr[i] = color_b;
				ptr[++i] = color_b;
				ptr[++i] = color_b;
				i++;
			}

			result.UnlockBits(bitmapData);
			return result;
		}
//#endif



		/// <summary>
		/// Make from a color image black and white 
		/// </summary>
		/// <param name="bitmap"></param>
		/// <param name="gradation"></param>
		/// <returns></returns>
		public static Bitmap MakeBlackWhite(Bitmap bitmap, int gradation)
		{
			Bitmap result = new Bitmap(bitmap);
			int gray = 0;
			Color color;
			for (int i = 0; i < bitmap.Width; i++)
			{
				for (int j = 0; j < bitmap.Height; j++)
				{
					color = bitmap.GetPixel(i, j);
					gray = (color.R + color.G + color.B) / 3;
					result.SetPixel(i,j, Color.FromArgb(gray, gray, gray));
				}
			}
			return result;
		}
		/// <summary>
		/// Make TestBitmap, 10 gray rectangles 
		/// rgb spectrum
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
				new int[]{1,1,1},
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
			//int that was not unboxing in Color.FromArgb(red, green, blue);
			int red = 0, green = 0, blue = 0;
			Color color;
			for (int i = 0; i < bitmap.Width; i++)
			{
				for (int j = 0; j < bitmap.Height; j++)
				{
					color = bitmap.GetPixel(i, j);
					red = color.R; green = color.G; blue = color.B;
					if ((bitmapChanel & BitmapChanel.Red) != BitmapChanel.Red) red = 0;
					if ((bitmapChanel & BitmapChanel.Green) != BitmapChanel.Green) green = 0;
					if ((bitmapChanel & BitmapChanel.Blue) != BitmapChanel.Blue) blue = 0;
					result.SetPixel(i, j, Color.FromArgb(red, green, blue));
				}
			}
			return result;
		}
	}
}
