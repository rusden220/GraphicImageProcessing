using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace GraphicImageProcessing
{
	public partial class GrayGradation : Form
	{
		private const string PLOTRED = "red";
		private const string PLOTGREEN = "green";
		private const string PLOTBLUE = "blue";

		private MainForm _mainForm;
		public GrayGradation()
		{
			InitializeComponent();
			chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
			chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

			chart1.Series.Add(PLOTRED);
			chart1.Series.Add(PLOTGREEN);
			chart1.Series.Add(PLOTBLUE);
			chart1.Series[PLOTRED].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
			chart1.Series[PLOTRED].Color = Color.Red;
			chart1.Series[PLOTGREEN].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
			chart1.Series[PLOTGREEN].Color = Color.Green;
			chart1.Series[PLOTBLUE].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
			chart1.Series[PLOTBLUE].Color = Color.Blue;  
		}
		protected override void OnLoad(EventArgs e)
		{
			_mainForm = (MainForm)this.Owner;
			UpdateGraphic();			
			base.OnLoad(e);
		}
		public void UpdateGraphic()
		{
			Bitmap bm =  new Bitmap(_mainForm.MainBitmap);
			BitmapData bitmapData = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

			int len = bm.Width * bm.Height * 4;
			if (shiftToolStripMenuItem.Checked)
			{
				byte[] arrExtr = new byte[] 
				{ //max min
					0,  255, //b
					0,  255, //g
					0,  255  //r
				};
				GetMaxMinFromBitmapArray(bitmapData.Scan0.ToInt32(), len, ref arrExtr);
				ShiftArray(bitmapData.Scan0.ToInt32(), len, arrExtr);
			}
			int[] arrayCountR = new int[byte.MaxValue + 1];
			int[] arrayCountG = new int[byte.MaxValue + 1];
			int[] arrayCountB = new int[byte.MaxValue + 1];
			//count R,G,B color;
			GetArrayCountForRBG(bitmapData.Scan0.ToInt32(), len, ref arrayCountR, ref arrayCountG, ref arrayCountB);
			bm.UnlockBits(bitmapData);

			chart1.Series[PLOTRED].Points.Clear();
			chart1.Series[PLOTGREEN].Points.Clear();
			chart1.Series[PLOTBLUE].Points.Clear();

			chart1.Series[PLOTRED].Points.DataBindY(arrayCountR);
			chart1.Series[PLOTGREEN].Points.DataBindY(arrayCountG);
			chart1.Series[PLOTBLUE].Points.DataBindY(arrayCountB);
		}
		private void GetMaxMinFromBitmapArray(int ptr, int len, ref byte[] arrExtr)
		{
			unsafe
			{
				byte* array = (byte*)ptr;
				for (int i = 0; i < len; i++)
				{
					if (arrExtr[0] < array[i]) arrExtr[0] = array[i];
					if (arrExtr[1] > array[i]) arrExtr[1] = array[i]; //blue
					i++;
					if (arrExtr[2] < array[i]) arrExtr[2] = array[i];
					if (arrExtr[3] > array[i]) arrExtr[3] = array[i]; //green
					i++;
					if (arrExtr[4] < array[i]) arrExtr[4] = array[i];
					if (arrExtr[5] > array[i]) arrExtr[5] = array[i]; //red
					i++;
				}
			}
		}
		private void ShiftArray(int ptr, int len, byte[] arrExtr)
		{
			int c = 0;
			double tempB = 255D / (arrExtr[c++] - arrExtr[c++]);
			double tempG = 255D / (arrExtr[c++] - arrExtr[c++]);
			double tempR = 255D / (arrExtr[c++] - arrExtr[c++]);
			unsafe
			{
				byte* array = (byte*)ptr;
				for (int i = 0; i < len; i++)//take just blue
				{
					array[i] = (byte)(tempB * (array[i] - arrExtr[1]));
					i++;
					array[i] = (byte)(tempG * (array[i] - arrExtr[3]));
					i++;
					array[i] = (byte)(tempR * (array[i] - arrExtr[5]));
					i++;
				}
			}
		}
		private void GetArrayCountForRBG(int ptr, int len, ref int[] arrayR, ref int[] arrayG, ref int[] arrayB)
		{
			unsafe
			{
				byte* array = (byte*)ptr;
				for (int i = 0; i < len; i++)
				{
					arrayB[array[i++]]++;
					arrayG[array[i++]]++;
					arrayR[array[i++]]++;
				}
			}
		}		

		private void shiftToolStripMenuItem_Click(object sender, EventArgs e)
		{
			((ToolStripMenuItem)sender).Checked = !((ToolStripMenuItem)sender).Checked;
			UpdateGraphic();
		}

		
	}
}
