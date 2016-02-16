namespace GraphicImageProcessing
{
	partial class BrightnessAndContrast
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.trackBarBrightness = new System.Windows.Forms.TrackBar();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.labelBrightness = new System.Windows.Forms.Label();
			this.trackBarContrast = new System.Windows.Forms.TrackBar();
			this.labelContrast = new System.Windows.Forms.Label();
			this.textBoxBrightness = new System.Windows.Forms.TextBox();
			this.textBoxContrast = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.trackBarBrightness)).BeginInit();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBarContrast)).BeginInit();
			this.SuspendLayout();
			// 
			// trackBarBrightness
			// 
			this.trackBarBrightness.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.trackBarBrightness.Location = new System.Drawing.Point(0, 53);
			this.trackBarBrightness.Name = "trackBarBrightness";
			this.trackBarBrightness.Size = new System.Drawing.Size(505, 45);
			this.trackBarBrightness.TabIndex = 0;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.aboutToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(505, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
			this.aboutToolStripMenuItem.Text = "About";
			// 
			// labelBrightness
			// 
			this.labelBrightness.AutoSize = true;
			this.labelBrightness.Location = new System.Drawing.Point(9, 30);
			this.labelBrightness.Name = "labelBrightness";
			this.labelBrightness.Size = new System.Drawing.Size(56, 13);
			this.labelBrightness.TabIndex = 2;
			this.labelBrightness.Text = "Brightness";
			// 
			// trackBarContrast
			// 
			this.trackBarContrast.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.trackBarContrast.Location = new System.Drawing.Point(0, 130);
			this.trackBarContrast.Name = "trackBarContrast";
			this.trackBarContrast.Size = new System.Drawing.Size(505, 45);
			this.trackBarContrast.TabIndex = 3;
			// 
			// labelContrast
			// 
			this.labelContrast.AutoSize = true;
			this.labelContrast.Location = new System.Drawing.Point(9, 107);
			this.labelContrast.Name = "labelContrast";
			this.labelContrast.Size = new System.Drawing.Size(46, 13);
			this.labelContrast.TabIndex = 4;
			this.labelContrast.Text = "Contrast";
			// 
			// textBoxBrightness
			// 
			this.textBoxBrightness.Location = new System.Drawing.Point(71, 27);
			this.textBoxBrightness.Name = "textBoxBrightness";
			this.textBoxBrightness.Size = new System.Drawing.Size(45, 20);
			this.textBoxBrightness.TabIndex = 5;
			// 
			// textBoxContrast
			// 
			this.textBoxContrast.Location = new System.Drawing.Point(71, 104);
			this.textBoxContrast.Name = "textBoxContrast";
			this.textBoxContrast.Size = new System.Drawing.Size(45, 20);
			this.textBoxContrast.TabIndex = 6;
			// 
			// BrightnessAndContrast
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(505, 214);
			this.Controls.Add(this.textBoxContrast);
			this.Controls.Add(this.textBoxBrightness);
			this.Controls.Add(this.labelContrast);
			this.Controls.Add(this.trackBarContrast);
			this.Controls.Add(this.labelBrightness);
			this.Controls.Add(this.trackBarBrightness);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "BrightnessAndContrast";
			this.Text = "BrightnessAndContrast";
			((System.ComponentModel.ISupportInitialize)(this.trackBarBrightness)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBarContrast)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TrackBar trackBarBrightness;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.Label labelBrightness;
		private System.Windows.Forms.TrackBar trackBarContrast;
		private System.Windows.Forms.Label labelContrast;
		private System.Windows.Forms.TextBox textBoxBrightness;
		private System.Windows.Forms.TextBox textBoxContrast;
	}
}