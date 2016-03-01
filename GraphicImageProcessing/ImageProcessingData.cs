using System;
using System.Drawing;

namespace GraphicImageProcessing
{
	public class ImageEffectData
	{
		private Action _appaydFilter;
		private Bitmap _result;
	}
	public class ImageProcessingData
	{
		private Bitmap _originalBitmap;
		private Bitmap _lastBitmap;
		ImageEffectData[] _arrayImageEffectData;

	}
}
