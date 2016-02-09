using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using GraphicImageProcessing.ImageProcessing;

namespace GraphicImageProcessing
{
	public partial class Form1 : Form
	{
		private Bitmap _mainBitmap;
		private Bitmap _originalBitmap;
		private int _mainBitmapPointX;
		private int _mainBitmapPointY;
		//future
		private Thread _workingThread;

		public Form1()
		{
			
			InitializeComponent();
			_mainBitmapPointX = 0;
			_mainBitmapPointY = menuStrip1.Height;
			_originalBitmap = GraphicsProcessing.GetTestBitmap(400, 200);
			_mainBitmap = (Bitmap)_originalBitmap.Clone();
		}
		/// <summary>
		/// ReDrawFunction
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.DrawImage(_mainBitmap, _mainBitmapPointX, _mainBitmapPointY);
			base.OnPaint(e);
		}
		
		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog() 
			{
				Multiselect = false,
				Filter = "Image Files(*.bmp;*.jpg;*.png)|*.bmp;*.jpg;*.png"
			};			
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				_mainBitmap = new Bitmap(ofd.FileName);
				this.Invalidate();
			}
		}

		private void ChanelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var tsm = (ToolStripMenuItem)sender;
			//invert check/uncheck
 			tsm.Checked = tsm.Checked ? false : true;

			BitmapChanel bc = BitmapChanel.None;
			if (redToolStripMenuItem.Checked) bc = bc | BitmapChanel.Red;
			if (greenToolStripMenuItem.Checked) bc = bc | BitmapChanel.Green;
			if (blueToolStripMenuItem.Checked) bc = bc | BitmapChanel.Blue;

			//make in thread
			_mainBitmap = GraphicsProcessing.ChooseChannel(_originalBitmap, bc);
			
			this.Invalidate();
		}		

	}
}
