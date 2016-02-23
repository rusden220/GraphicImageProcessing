using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GraphicImageProcessing.ImageProcessing;

namespace GraphicImageProcessing
{
	public partial class BrightnessAndContrast : Form
	{
		private MainForm _mainForm;
		private bool _isScroll;
		private bool _isTextChanged;

		public delegate void BitmapChangedEvent(object sender, Bitmap e);
		public event BitmapChangedEvent BitmapChanged;

		public BrightnessAndContrast()
		{
			InitializeComponent();
			trackBarContrast.Minimum = -100;
			trackBarContrast.Maximum = 100;
			trackBarContrast.Value = 0;
			trackBarBrightness.Minimum = -255;
			trackBarBrightness.Maximum = 255;
			trackBarBrightness.Value = 0;

			trackBarBrightness.Scroll += trackBar_Scroll;
			trackBarContrast.Scroll += trackBar_Scroll;

			textBoxBrightness.Tag = new BitmapChanchFunctionData() { BitmapChangeFunction = GraphicsProcessing.Brightness, Control = trackBarBrightness };
			textBoxContrast.Tag = new BitmapChanchFunctionData() { BitmapChangeFunction = GraphicsProcessing.Contrast, Control = trackBarContrast };

			trackBarBrightness.Tag = new BitmapChanchFunctionData() { BitmapChangeFunction = GraphicsProcessing.Brightness, Control = textBoxBrightness };
			trackBarContrast.Tag = new BitmapChanchFunctionData() { BitmapChangeFunction = GraphicsProcessing.Contrast, Control = textBoxContrast };

			textBoxBrightness.TextChanged += textBox_TextChanged;
			textBoxContrast.TextChanged += textBox_TextChanged;
		}

		private void textBox_TextChanged(object sender, EventArgs e)
		{
			if (!_isScroll)
			{
				int value = 0;
				var temp = ((TextBox)sender);
				if (int.TryParse(temp.Text, out value))
				{
					((TrackBar)(((BitmapChanchFunctionData)(temp.Tag)).Control)).Value = value;
					UpdateBitmap((BitmapChanchFunctionData)(temp.Tag), value);
				}
				_isTextChanged = true;
			}
			_isScroll = false;
		}
		private void UpdateBitmap(BitmapChanchFunctionData control, int value)
		{
			BitmapChanged(this, control.BitmapChangeFunction(_mainForm.OriginalBitmap, value));
		}
		private void trackBar_Scroll(object sender, EventArgs e)
		{
			_isScroll = true;
			var temp = ((TrackBar)sender);
			if (!_isTextChanged)
			{
				((TextBox)(((BitmapChanchFunctionData)(temp.Tag)).Control)).Text = temp.Value.ToString();
				UpdateBitmap((BitmapChanchFunctionData)(temp.Tag), temp.Value);
			}
			_isTextChanged = false;
		}
		protected override void OnLoad(EventArgs e)
		{
			_mainForm = (MainForm)this.Owner;
			base.OnLoad(e);
		}

		private void autoContrastToolStripMenuItem_Click(object sender, EventArgs e)
		{
			BitmapChanged(this, GraphicsProcessing.AutoContrast(_mainForm.OriginalBitmap));
		}

		private void applyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_mainForm.ApplyChanges();
		}
	}
	public class BitmapChanchFunctionData
	{
		public Func<Bitmap, int, Bitmap> BitmapChangeFunction { get; set; }
		public object Control { get; set; }

	}

}
