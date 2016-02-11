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
				if (i % 100 == 0) Console.Write("#");
			}
			Console.WriteLine();
			return avgTime;
		}
		public double Start(Func<Bitmap, Bitmap> action, int imageSize)
		{
			double avgTime = 0D;
			Stopwatch sw = new Stopwatch();
			Bitmap bm = new Bitmap(imageSize, imageSize);
			for (int i = 0; i < _amountTest; i++)
			{
				bm = new Bitmap(imageSize, imageSize);
				Graphics.FromImage(bm).Clear(Color.LightGreen);
				sw.Reset();
				sw.Start();

				var temp = action(bm);

				sw.Stop();
				temp = null;
				bm = null;
				GC.Collect(2);
				avgTime += sw.ElapsedTicks / _amountTest;

				if (i % 100 == 0) Console.Write("#");
			}
			Console.WriteLine();
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
				int amountTest = 2000;
				int imageSize = 500;
				Bitmap bm = new Bitmap(imageSize, imageSize);
				Graphics.FromImage(bm).Clear(Color.LightGreen);
				Console.WriteLine("Start timeEasy");
				//var timeEasy = new TimeTestPerformance() { AmountTest = amountTest, TestedFunction = () => { GraphicsProcessing.OptimisationEasy(bm); } }.Start();
				var timeEasy = new TimeTestPerformance() { AmountTest = amountTest }.Start(GraphicsProcessing.OptimisationEasy, imageSize);
				Console.WriteLine("timeEasy: " + timeEasy.ToString());

				bm = new Bitmap(imageSize, imageSize);
				Graphics.FromImage(bm).Clear(Color.LightGreen);
				Console.WriteLine("Start timePointer");
				//var timePointer = new TimeTestPerformance() { AmountTest = amountTest, TestedFunction = () => { var bm1 = GraphicsProcessing.OptimisationPointer(bm); bm1 = null;} }.Start();
				var timePointer = new TimeTestPerformance() { AmountTest = amountTest }.Start(GraphicsProcessing.OptimisationPointer, imageSize);
				Console.WriteLine("timePointer: " + timePointer.ToString());

				bm = new Bitmap(imageSize, imageSize);
				Graphics.FromImage(bm).Clear(Color.LightGreen);
				Console.WriteLine("Start timeUnsafe");
				//var timeUnsafe = new TimeTestPerformance() { AmountTest = amountTest, TestedFunction = () => { GraphicsProcessing.OptimisationUnsafe(bm); } }.Start();
				var timeUnsafe = new TimeTestPerformance() { AmountTest = amountTest }.Start(GraphicsProcessing.OptimisationUnsafe, imageSize);				
				Console.WriteLine("timeUnsafe: " + timeUnsafe.ToString());

			}
			Console.WriteLine("End");
			Console.ReadLine();
		}
	}
}
