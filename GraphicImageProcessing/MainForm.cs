using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using GraphicImageProcessing.ImageProcessing;

namespace GraphicImageProcessing
{
	public partial class MainForm : Form
	{
		private Bitmap _mainBitmap;
		private Bitmap _originalBitmap;
		private int _mainBitmapPointX;
		private int _mainBitmapPointY;
		//Gray Gradation window
		private GrayGradation _grayGradation;
		//future
		private Thread _workingThread;

		public MainForm()
		{			
			InitializeComponent();
			_mainBitmapPointX = 0;
			_mainBitmapPointY = menuStrip1.Height;
			_originalBitmap = GraphicsProcessing.GetTestBitmap(400, 200);
			_mainBitmap = new Bitmap(_originalBitmap);
		}
		public Bitmap MainBitmap
		{
			get { return _mainBitmap; }
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
			_grayGradation = new GrayGradation();
			_grayGradation.Show(this);
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.ShowDialog();
			_mainBitmap.Save(sfd.FileName);
		}		

	}
}
