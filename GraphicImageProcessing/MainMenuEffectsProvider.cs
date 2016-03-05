using System;
using System.Drawing;
using ImageProcessing;
using ImageProcessing.EffectsBase;


namespace GraphicImageProcessing
{
	public class MainMenuEffectsData
	{
		public string Name { get; set; }
		public GraphicsEffectsBase GraphicsEffectsBase { get; set; }
	}
	public class MainMenuEffectsProvider
	{
		public MainMenuEffectsData[] GetEffects()
		{
			//Get all Graphics Function
			return null;
		}
		public static void ApplyEffect(Bitmap bitmap, GraphicsEffectsBase geb)
		{
			//Should run via ImageProcessingProvider
			GraphicsFunctions.ApplyEffect(bitmap, geb);
		}
	}
}
