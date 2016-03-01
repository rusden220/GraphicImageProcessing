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
			this.Bitmap = new Bitmap(ged.Bitmap);
			BitmapData bitmapData = this.Bitmap.LockBits(new Rectangle(0, 0, this.Bitmap.Width, this.Bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
			unsafe
			{
				//byte* ptr = stackalloc byte[ged.Length];
				byte* ptr = (byte*)bitmapData.Scan0.ToInt32();
				byte* ptrFrom = (byte*)ged.Pointer;
				for (int i = 0; i < ged.Length; i++)
					ptr[i] = ptrFrom[i];
				this.Pointer = (int)ptr;
			}
			this.Bitmap.UnlockBits(bitmapData);
		}
		/// <summary>
		/// Contain bitmap for processing
		/// </summary>
		public Bitmap Bitmap { get; set; }
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
