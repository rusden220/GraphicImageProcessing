using System;
using System.Drawing;
using System.Linq;

namespace GraphicImageProcessing.ImageProcessing
{
	[Flags]
	public enum BitmapChanel
	{
		None = 0,
		Red = 0x1,
		Green = 0x2,
		Blue = 0x4
	}
	public class GraphicsProcessing
	{
		private Bitmap _mainBitmap;

		public Bitmap MainBitmap
		{
			get { return _mainBitmap; }
			set { _mainBitmap = value; }
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
