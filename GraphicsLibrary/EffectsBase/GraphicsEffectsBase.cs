using System;
using System.Threading;

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

		/// <summary>
		/// Apply effect 
		/// </summary>
		public virtual void ApplyEffects()
		{
			Check();
			if (_isParallel)
				ApplyEffectsBlock(0, _graphicsEffectsData.Length);
			else
				ApplyEffectsParallel();
		}
		/// <summary>
		/// parallel applying
		/// </summary>
		private void ApplyEffectsParallel()
		{
			//Check();
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
		protected abstract void Effect(int ptr, int index);
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
				Effect(_graphicsEffectsData.Pointer, i);
			}
		}
		/// <summary>
		/// check the variable before call
		/// </summary>
		private void Check()
		{
			//if (_effectsDelegat == null) throw new NullReferenceException("EffectsDelegat is Null");
			if (_graphicsEffectsData == null) throw new NullReferenceException("GraphicsEffectsData is Null");
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
