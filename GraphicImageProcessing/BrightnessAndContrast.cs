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

			int min = -255, max = 255;
			trackBarContrast.Minimum = min;
			trackBarContrast.Maximum = max;
			trackBarContrast.Value = 0;
			trackBarBrightness.Minimum = min;
			trackBarBrightness.Maximum = max;
			trackBarBrightness.Value = 0;

			trackBarBrightness.Scroll += trackBar_Scroll;
			trackBarContrast.Scroll += trackBar_Scroll;
			trackBarBrightness.Tag = textBoxBrightness;
			trackBarContrast.Tag = textBoxContrast;

			textBoxBrightness.Tag = trackBarBrightness;
			textBoxContrast.Tag = trackBarContrast;
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
					((TrackBar)temp.Tag).Value = value;
					UpdateBitmap(value);
				}
				_isTextChanged = true;				
			}
			_isScroll = false;
			//throw new NotImplementedException();
		}
		private void UpdateBitmap(int value)
		{
			BitmapChanged(this, GraphicsProcessing.Brightness(_mainForm.OriginalBitmap, value));
		}
		private void trackBar_Scroll(object sender, EventArgs e)
		{
			_isScroll = true;
			var temp = ((TrackBar)sender);
			if (!_isTextChanged)
			{
				((TextBox)temp.Tag).Text = temp.Value.ToString();
				UpdateBitmap(temp.Value);
			}
			_isTextChanged = false;
			//throw new NotImplementedException();
		}
		protected override void OnLoad(EventArgs e)
		{
			_mainForm = (MainForm)this.Owner;
			base.OnLoad(e);
		}
	}
}
