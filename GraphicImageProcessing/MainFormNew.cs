using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using GraphicImageProcessing.ImageProcessing;

namespace GraphicImageProcessing
{
	public partial class MainFormNew : Form
	{
		private Bitmap _mainBitmap;
		private Bitmap _originalBitmap;

		private int _mainBitmapPointX;
		private int _mainBitmapPointY;

		//color gradation window
		private ColorHistogram _colorHistogram;
		//Brightness and contrast window
		private BrightnessAndContrast _brightnessAndContrast;

		//future
		private Thread _workingThread;

		public MainFormNew()
		{
			InitializeComponent();
			this.DoubleBuffered = true;
			_mainBitmapPointX = 0;
			_mainBitmapPointY = menuStrip1.Height;
			_originalBitmap = GraphicsProcessing.GetTestBitmap(400, 200);			
			_mainBitmap = new Bitmap(_originalBitmap);
		}
		
		public Bitmap MainBitmap
		{
			get { return _mainBitmap; }
		}
		public Bitmap OriginalBitmap
		{
			get { return _originalBitmap; }
		}

		/// <summary>
		/// ReDrawFunction
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.DrawImage(_mainBitmap, _mainBitmapPointX, _mainBitmapPointY);
			//base.OnPaint(e);
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
				//var bm = new Bitmap(ofd.FileName);
				//_mainBitmap = new Bitmap(bm.Width, bm.Height);
				//Graphics.FromImage(_mainBitmap).DrawImage(bm, 0, 0, _mainBitmap.Width, _mainBitmap.Height);

				_mainBitmap = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);
				Graphics.FromImage(_mainBitmap).DrawImage(new Bitmap(ofd.FileName), 0, 0, _mainBitmap.Width, _mainBitmap.Height);
				
				_originalBitmap = new Bitmap(_mainBitmap);
				this.Invalidate();
			}
		}

		private void ChanelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var tsm = (ToolStripMenuItem)sender;
			//invert check/uncheck
 			tsm.Checked = !tsm.Checked;

			BitmapChanel bc = BitmapChanel.None;
			if (redToolStripMenuItem.Checked) bc = bc | BitmapChanel.Red;
			if (greenToolStripMenuItem.Checked) bc = bc | BitmapChanel.Green;
			if (blueToolStripMenuItem.Checked) bc = bc | BitmapChanel.Blue;

			//make in thread
			_mainBitmap = GraphicsProcessing.ChooseChannel(_originalBitmap, bc);
			
			this.Invalidate();
		}

		private void makeBlackWhiteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_mainBitmap = GraphicsProcessing.MakeBlackWhite(_originalBitmap);
			this.Invalidate();
		}

		private void histogramsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_colorHistogram = new ColorHistogram();
			_colorHistogram.Show(this);
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.ShowDialog();
			_mainBitmap.Save(sfd.FileName);
		}

		private void brightnessAndContrastToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_brightnessAndContrast = new BrightnessAndContrast();
			//should be 
			_brightnessAndContrast.BitmapChanged += _brightnessAndContrast_BitmapChanged;
			_brightnessAndContrast.Show(this);
		}
		private void _brightnessAndContrast_BitmapChanged(object sender, Bitmap e)
		{
			_mainBitmap = e;
			this.Invalidate();
			//throw new NotImplementedException();
		}
		public void ApplyChanges()
		{
			_originalBitmap = new Bitmap(_mainBitmap);
		}

		private void applyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ApplyChanges();
		}
	}
}
