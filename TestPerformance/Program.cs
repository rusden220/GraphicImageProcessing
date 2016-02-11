using System;
using System.Drawing;
using System.Diagnostics;
using GraphicImageProcessing.ImageProcessing;

namespace TestPerformance
{
	class TimeTestPerformance
	{
		private int _amountTest;
		private Action _testedFunction;
	
		public TimeTestPerformance() { }
		public TimeTestPerformance(Action action) { _testedFunction = action; }

		public int AmountTest
		{
			get { return _amountTest; }
			set { _amountTest = value; }
		}
		public Action TestedFunction
		{
			get { return _testedFunction; }
			set { _testedFunction = value; }
		}
		public double Start()
		{
			if (_testedFunction == null) throw new NullReferenceException();
			double avgTime = 0D;
			Stopwatch sw = new Stopwatch();
			for (int i = 0; i < _amountTest; i++)
			{
				sw.Start();
				_testedFunction();
				sw.Stop();
				avgTime += sw.ElapsedTicks / _amountTest;
			}
			return avgTime;
		}
		
		
	}
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Choose Processor");
			Console.ReadLine();
			if (GraphicsProcessing.isRelease())
			{
				Console.WriteLine("Release");
				int amountTest = 1000;
				int imageSize = 1000;
				Bitmap bm = new Bitmap(imageSize, imageSize);
				Graphics.FromImage(bm).Clear(Color.LightGreen);
				Console.WriteLine("Start timeEasy");
				var timeEasy = new TimeTestPerformance() { AmountTest = amountTest, TestedFunction = () => { GraphicsProcessing.OptimisationEasy(bm); } }.Start();
				Console.WriteLine("timeEasy: " + timeEasy.ToString());

				bm = new Bitmap(imageSize, imageSize);
				Graphics.FromImage(bm).Clear(Color.LightGreen);
				Console.WriteLine("Start timePointer");
				var timePointer = new TimeTestPerformance() { AmountTest = amountTest, TestedFunction = () => { GraphicsProcessing.OptimisationPointer(bm); } }.Start();
				Console.WriteLine("timePointer: " + timePointer.ToString());

				bm = new Bitmap(imageSize, imageSize);
				Graphics.FromImage(bm).Clear(Color.LightGreen);
				Console.WriteLine("Start timeUnsafe");
				var timeUnsafe = new TimeTestPerformance() { AmountTest = amountTest, TestedFunction = () => { GraphicsProcessing.OptimisationUnsafe(bm); } }.Start();
				Console.WriteLine("timeUnsafe: " + timeUnsafe.ToString());

			}
			Console.WriteLine("End");
			Console.ReadLine();
		}
	}
}
