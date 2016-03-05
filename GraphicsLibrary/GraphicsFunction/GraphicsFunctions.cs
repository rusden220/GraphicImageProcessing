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
	/// Graphics function for bitmap
	/// </summary>
    public class GraphicsFunctions
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
			var methods = typeof(GraphicsFunctions).GetMethods(BindingFlags.Static | BindingFlags.Public);
			List<GraphicsFunctionsDelegate> result = new List<GraphicsFunctionsDelegate>();
			foreach (var method in methods)
			{
				//if (method.GetCustomAttributes(typeof(EffectAttribute)) != null)
					//result.Add((GraphicsFunctionsDelegate)method.Invoke);				
			}
			return result.ToArray();
		}
		public Bitmap Noize(Bitmap bitmap, params object[] param)
		{
			bool isParallel = (bool)param[0];
			int noizeLevelUp = (int)param[1];
			int noizeLevelDown = (int)param[2];
			NoizeEffect ne = new NoizeEffect()
			{
				NoizeLevelDown = noizeLevelDown,
				NoizeLevelUp = noizeLevelUp,
				isParallel = isParallel,
				GraphicsEffectsData = new GraphicsEffectsData()
				{ OriginalBitmap = bitmap }
			};
			ne.ApplyEffects();
			return ne.GraphicsEffectsData.ResultBitmap;
		}
		/// <summary>
		/// Apply graphics effect
		/// </summary>
		/// <param name="bitmap"></param>
		/// <param name="geb"></param>
		/// <returns></returns>
		public static Bitmap ApplyEffect(Bitmap bitmap, GraphicsEffectsBase geb)
		{
			geb.GraphicsEffectsData = new GraphicsEffectsData()
			{ OriginalBitmap = bitmap };
			geb.ApplyEffects();
			return geb.GraphicsEffectsData.ResultBitmap;
		}
    }	
}
