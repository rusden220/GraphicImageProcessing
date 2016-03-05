using System;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageProcessing.EffectsBase 
{
	/// <summary>
	/// Base class for every Graphics effect class 
	/// </summary>
	public abstract class GraphicsEffectsBase
	{
		/// <summary>
		/// use for transfer data in ThreadPool
		/// </summary>
		private class ThreadPoolParamsWrapper
		{
			public int Index { get; set; }
			public int Length { get; set; }
			public CountdownEvent CountdownEvent { get; set; }
		}

		//public delegate void ApplayEffectsDelegat(int ptr, int index);
		//protected ApplayEffectsDelegat _effectsDelegat;
		protected GraphicsEffectsData _graphicsEffectsData;
		private bool _isParallel;
		private BitmapData _bitmapDataResult;// = _graphicsEffectsData.ResultBitmap.LockBits(new Rectangle(0, 0, _graphicsEffectsData.ResultBitmap.Width, _graphicsEffectsData.ResultBitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
		private BitmapData _bitmapDataOrign;
		/// <summary>
		/// Apply effect 
		/// </summary>
		public virtual void ApplyEffects()
		{
			if (_isParallel)
			{
				SetGraphicsData();
				ApplyEffectsBlock(0, _graphicsEffectsData.Length);
				UnlockGraphicsData();
			}
			else
				ApplyEffectsParallel();					
		}
		/// <summary>
		/// parallel applying
		/// </summary>
		public void ApplyEffectsParallel()
		{
			SetGraphicsData();
			int processorCount = Environment.ProcessorCount;
			CountdownEvent cde = new CountdownEvent(processorCount);
			int[] arrayLength = GetArrayLength(_graphicsEffectsData.Length, processorCount);
			for (int i = 0; i < processorCount; i++)
			{
				ThreadPool.QueueUserWorkItem((object param) =>
				{
					var tppw = ((ThreadPoolParamsWrapper)param);
					ApplyEffectsBlock(tppw.Index, tppw.Length);
					tppw.CountdownEvent.Signal();
				}, new ThreadPoolParamsWrapper() { Index = i, Length = arrayLength[i], CountdownEvent = cde });
			}

		}
		/// <summary>
		/// Unlock Bitmap after using
		/// </summary>
		private void UnlockGraphicsData()
		{
			_graphicsEffectsData.OriginalBitmap.UnlockBits(_bitmapDataOrign);
			_graphicsEffectsData.ResultBitmap.UnlockBits(_bitmapDataResult);
		}
		/// <summary>
		/// check and setup graphics data
		/// </summary>
		private void SetGraphicsData()
		{
			//if (_effectsDelegat == null) throw new NullReferenceException("EffectsDelegat is Null");
			if (_graphicsEffectsData == null) throw new NullReferenceException("GraphicsEffectsData is Null");
			if (_graphicsEffectsData.OriginalBitmap == null) throw new NullReferenceException("OriginalBitmap is Null");
					
			_graphicsEffectsData.ResultBitmap = new Bitmap(_graphicsEffectsData.OriginalBitmap);
			_graphicsEffectsData.Length = _graphicsEffectsData.OriginalBitmap.Width * _graphicsEffectsData.OriginalBitmap.Height * 4;//ARGB

			_bitmapDataResult = _graphicsEffectsData.ResultBitmap.LockBits(new Rectangle(0, 0, _graphicsEffectsData.ResultBitmap.Width, _graphicsEffectsData.ResultBitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
			_bitmapDataOrign = _graphicsEffectsData.OriginalBitmap.LockBits(new Rectangle(0, 0, _graphicsEffectsData.OriginalBitmap.Width, _graphicsEffectsData.OriginalBitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

			_graphicsEffectsData.OriginalPointer = _bitmapDataOrign.Scan0.ToInt32();
			_graphicsEffectsData.ResultPointer = _bitmapDataResult.Scan0.ToInt32();
		}
		protected abstract void Effect(int index);
		//public ApplayEffectsDelegat EffectsDelegat
		//{
		//	get { return _effectsDelegat; }
		//	set { _effectsDelegat = value; }
		//}
		public GraphicsEffectsData GraphicsEffectsData
		{
			get { return _graphicsEffectsData; }
			set { _graphicsEffectsData = value; }
		}
		/// <summary>
		/// whether to run the algorithm as parallel
		/// </summary>
		public bool isParallel
		{
			get { return _isParallel; }
			set { _isParallel = value; }
		}		
		/// <summary>
		/// apply filter for block in pointer
		/// </summary>
		/// <param name="index"></param>
		/// <param name="length"></param>
		private void ApplyEffectsBlock(int index, int length)
		{
			for (int i = index; i < length; i += 4)
			{
				Effect(i);
			}
		}
		/// <summary>
		/// return array of len for ApplyEffectsParallel 
		/// </summary>
		/// <param name="length"></param>
		/// <param name="processorCount"></param>
		/// <returns></returns>
		private int[] GetArrayLength(int length, int processorCount)
		{
			int step = length / processorCount;
			step += _graphicsEffectsData.AmountOfChannel + step % _graphicsEffectsData.AmountOfChannel;
			int stepTemp = 0;
			int[] result = new int[processorCount];
			for (int i = 0; i < processorCount; i++)
			{
				stepTemp += step;
				result[i] = stepTemp;
			}
			result[processorCount - 1] = length;
			return result;
		}
	}
}
