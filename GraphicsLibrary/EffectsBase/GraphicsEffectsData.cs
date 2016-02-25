using System;

namespace ImageProcessing.EffectsBase
{
	/// <summary>
	/// Mediator class for GraphicsFunctions
	/// </summary>
	public class GraphicsEffectsData
	{
		private int _amountOfChannel = 4;
		public GraphicsEffectsData(){ }
		public GraphicsEffectsData(GraphicsEffectsData ged)
		{
			this.Length = ged.Length;			
			unsafe
			{
				byte* ptr = stackalloc byte[ged.Length];
				byte* ptrFrom = (byte*)ged.Pointer;
				for (int i = 0; i < ged.Length; i++)
					ptr[i] = ptrFrom[i];
				this.Pointer = (int)ptr;
			}
		}
		/// <summary>
		/// Pointer for every bitmap pixels
		/// </summary>
		public int Pointer { get; set; }
		/// <summary>
		/// Length of array in point
		/// </summary>
		public int Length { get; set; }
		/// <summary>
		/// amount of channel in pointer RGB + Alpha
		/// </summary>
		public int AmountOfChannel
		{
			get { return _amountOfChannel; }
			set { _amountOfChannel = value; }
		}
		
	}
}
