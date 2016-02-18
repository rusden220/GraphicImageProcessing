using System;
using System.Drawing;
using System.Diagnostics;

namespace TestPerformance
{
	class TimeTestPerformance
	{
		private int _amountTest;
		private Action _testedFunction;
		public TimeTestPerformance() {
			_amountTest = 1000;
		}
		public TimeTestPerformance(Action action):this() { _testedFunction = action; }

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
		public double Start(Func<object, object[], object> action, int imageSize,object owner, object[] _params)
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

				var temp = action(owner, new object[]{bm});

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
}
