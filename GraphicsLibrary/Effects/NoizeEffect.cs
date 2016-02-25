using System;
using ImageProcessing.EffectsBase;

namespace ImageProcessing.Effects
{
	public class NoizeEffect: GraphicsEffectsBase
	{
		private Random _random;
		public NoizeEffect()
		{
			EffectsDelegat = MakeNoise;
			_random = new Random();
		}
		private void MakeNoise(int pointer, int index)
		{
			unsafe
			{
				byte* ptr = (byte*)pointer;
				int num = _random.Next(NoizeLevelDown, NoizeLevelUp);
				ptr[index] = StaticGraphicsFunctions.ConvertToByte(ptr[index] + num); index++;
				ptr[index] = StaticGraphicsFunctions.ConvertToByte(ptr[index] + num); index++;
				ptr[index] = StaticGraphicsFunctions.ConvertToByte(ptr[index] + num);
			}
		}
		public int NoizeLevelDown { get; set; }
		public int NoizeLevelUp { get; set; }
		
	}
}
