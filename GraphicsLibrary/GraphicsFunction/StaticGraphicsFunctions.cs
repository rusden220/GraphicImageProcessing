using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Collections.Generic;

using ImageProcessing.Effects;
using ImageProcessing.EffectsBase;

namespace ImageProcessing
{
	/// <summary>
	/// Graphics Function for Bitmap
	/// </summary>
    public static class StaticGraphicsFunctions
    {
		/// <summary>
		/// Convert to byte
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static byte ConvertToByte(double num)
		{
			if (num > byte.MaxValue) return byte.MaxValue;
			else if (num < byte.MinValue) return byte.MinValue;
			return (byte)num;
		}
		/// <summary>
		/// Convert to byte
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public static byte ConvertToByte(int num)
		{
			if (num > byte.MaxValue) return byte.MaxValue;
			else if (num < byte.MinValue) return byte.MinValue;
			return (byte)num;
		}

		public delegate Bitmap GraphicsFunctionsDelegate(Bitmap bitmap, params object[] param);
		/// <summary>
		/// GetAllFunctions
		/// </summary>
		/// <returns></returns>
		public static GraphicsFunctionsDelegate[] GetAllFunctions()
		{
			var methods = typeof(StaticGraphicsFunctions).GetMethods(BindingFlags.Static | BindingFlags.Public);
			List<GraphicsFunctionsDelegate> result = new List<GraphicsFunctionsDelegate>();
			foreach (var method in methods)
			{
				//if (method.GetCustomAttributes(typeof(EffectAttribute)) != null)
					//result.Add((GraphicsFunctionsDelegate)method.Invoke);
				
			}
			return result.ToArray();
		}
		/// <summary>
		/// Make noise on bitmap
		/// </summary>
		/// <param name="bitmap"></param>
		/// <param name="isParallel"></param>
		/// <param name="noizeLevelUp"></param>
		/// <param name="noizeLevelDown"></param>		
		/// <returns></returns>
		
		public static Bitmap Noize(Bitmap bitmap, params object[] param)
		{
			bool isParallel = (bool)param[0];
			int noizeLevelUp = (int)param[1];
			int noizeLevelDown = (int)param[2];

			Bitmap result = new Bitmap(bitmap);
			int len = bitmap.Width * bitmap.Height * 4;//ARGB
			//get pointer of byte array in Bitmpa
			BitmapData bitmapData = result.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
			NoizeEffect ne = new NoizeEffect()
			{
				NoizeLevelDown = noizeLevelDown,
				NoizeLevelUp = noizeLevelUp,
				isParallel = isParallel,			
				GraphicsEffectsData = new GraphicsEffectsData()
				{
					AmountOfChannel = 4,
					Length = len,
					Pointer = bitmapData.Scan0.ToInt32()
				}
			};
			ne.ApplyEffects();
			result.UnlockBits(bitmapData);
			return result;
		}
    }	
}
