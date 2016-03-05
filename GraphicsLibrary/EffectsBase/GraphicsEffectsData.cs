using System;
using System.Drawing;
using System.Drawing.Imaging;

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
			this.OriginalBitmap = new Bitmap(ged.OriginalBitmap);
			BitmapData bitmapData = this.OriginalBitmap.LockBits(new Rectangle(0, 0, this.OriginalBitmap.Width, this.OriginalBitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
			unsafe
			{
				//byte* ptr = stackalloc byte[ged.Length];
				byte* ptr = (byte*)bitmapData.Scan0.ToInt32();
				byte* ptrFrom = (byte*)ged.OriginalPointer;
				for (int i = 0; i < ged.Length; i++)
					ptr[i] = ptrFrom[i];
				this.OriginalPointer = (int)ptr;
			}
			this.OriginalBitmap.UnlockBits(bitmapData);
		}
		/// <summary>
		/// Contain bitmap for processing
		/// </summary>
		public Bitmap OriginalBitmap { get; set; }
		/// <summary>
		/// Contain result bitmap after apply effects
		/// </summary>
		public Bitmap ResultBitmap { get; set; }
		/// <summary>
		/// Contain result pointer after apply effects
		/// </summary>
		public int ResultPointer { get; set; }
		/// <summary>
		/// Pointer for every bitmap pixels
		/// </summary>
		public int OriginalPointer { get; set; }
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
