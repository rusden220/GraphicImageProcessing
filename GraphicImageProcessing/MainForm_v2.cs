using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageProcessing_new = ImageProcessing;

namespace GraphicImageProcessing
{
	public partial class MainForm_v2 : Form
	{
		private Bitmap _mainBitmap;
		private Bitmap _originalBitmap;

		private int _mainBitmapPointX;
		private int _mainBitmapPointY;

		public MainForm_v2()
		{
			InitializeComponent();
			InitializeComponent();
			this.DoubleBuffered = true;
			_mainBitmapPointX = 0;
			_mainBitmapPointY = menuStrip1.Height;
			_originalBitmap = GraphicImageProcessing.ImageProcessing.GraphicsProcessing.GetTestBitmap(400, 200);			
			_mainBitmap = new Bitmap(_originalBitmap);
		}

		private void MainForm_v2_Load(object sender, EventArgs e)
		{

		}
		
		private void MainMenuEffectsBuilder()
		{
			//GetAllGraphicsFunction
			var effects = new MainMenuEffectsProvider().GetEffects();
			foreach (var item in effects)
			{
				var tsmi = new ToolStripMenuItem();
				tsmi.Tag = item;
				tsmi.Name = item.Name;
				tsmi.Click += EffectsToolStripMenuItem_Click;
				editToolStripMenuItem.DropDownItems.Add(tsmi);
			}
		}

		void EffectsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//everything is very bad
			MainMenuEffectsProvider.ApplyEffect(_mainBitmap, (ImageProcessing_new.EffectsBase.GraphicsEffectsBase)((ToolStripMenuItem)sender).Tag);
		}
		
	}
}
