using System;
using ImageProcessing.EffectsBase;

namespace ImageProcessing.Effects
{
	public class NoizeEffect: GraphicsEffectsBase
	{
		private Random _random;
		public NoizeEffect()
		{
			//EffectsDelegat = MakeNoise;
			_random = new Random();
		}
		protected override void Effect(int index)
		{
			unsafe
			{				
				byte* ptrR = (byte*)_graphicsEffectsData.ResultPointer;
				byte* ptrO = (byte*)_graphicsEffectsData.OriginalPointer;
				int num = _random.Next(NoizeLevelDown, NoizeLevelUp);
				ptrR[index] = GraphicsFunctions.ConvertToByte(ptrO[index] + num); index++;
				ptrR[index] = GraphicsFunctions.ConvertToByte(ptrO[index] + num); index++;
				ptrR[index] = GraphicsFunctions.ConvertToByte(ptrO[index] + num);
			}
		}
		public int NoizeLevelDown { get; set; }
		public int NoizeLevelUp { get; set; }
		
	}
}
