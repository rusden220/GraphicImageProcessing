using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using GraphicImageProcessing.Pointer;


namespace TestPerformance
{
	/// <summary>
	/// class for test Performance
	/// </summary>
	class FunctionsToTest
	{
		#region BitmapPerformanceTest
		[FunctionsForTest("BitmapPerformanceTest")]
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
		[FunctionsForTest("BitmapPerformanceTest")]
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
		[FunctionsForTest("BitmapPerformanceTest")]
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
		#endregion

		#region LambdaPerformanceTest
		[FunctionsForTest("LambdaPerformanceTest")]
		public static Bitmap BrightnessLambda(Bitmap bitmap)
		{
			int value = 100;
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
		[FunctionsForTest("LambdaPerformanceTest")]
		public static Bitmap BrightnessFunction(Bitmap bitmap)
		{
			int value = 100;
			Bitmap result = new Bitmap(bitmap);
			int len = bitmap.Width * bitmap.Height * 4;//ARGB
			//get pointer of byte array in Bitmpa
			BitmapData bitmapData = result.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
			unsafe
			{
				byte* ptr = (byte*)bitmapData.Scan0.ToInt32();
				for (int i = 0; i < len; i++)
				{
					ptr[i] = ConvertIntToByte(ptr[i] + value); i++;
					ptr[i] = ConvertIntToByte(ptr[i] + value); i++;
					ptr[i] = ConvertIntToByte(ptr[i] + value); i++;
				}
			}
			result.UnlockBits(bitmapData);
			return result;
		}
		public static byte ConvertIntToByte(int num)
		{
			if (num > 255) num = 255;
			else if (num < 0) num = 0;
			return (byte)num;
		}
		//public static Bitmap BrightnessUnsafeLambda(Bitmap bitmap, int value)
		//{
		//	Bitmap result = new Bitmap(bitmap);
		//	int len = bitmap.Width * bitmap.Height * 4;//ARGB
		//	byte color_b = 0;
		//	//get pointer of byte array in Bitmpa
		//	BitmapData bitmapData = result.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

		//	Func<int, byte> intToByte = (int num) =>
		//	{
		//		if (num > 255) num = 255;
		//		else if (num < 0) num = 0;
		//		return (byte)num;
		//	};
		//	unsafe
		//	{
		//		byte* ptr = (byte*)bitmapData.Scan0.ToInt32();
		//		for (int i = 0; i < len; i++)
		//		{
		//			ptr[i] = intToByte(ptr[i] + value); i++;
		//			ptr[i] = intToByte(ptr[i] + value); i++;
		//			ptr[i] = intToByte(ptr[i] + value); i++;
		//		}
		//	}
		//	result.UnlockBits(bitmapData);
		//	return result;
		//}
		#endregion


	}
}
