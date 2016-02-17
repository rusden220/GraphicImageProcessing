using System;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;
using GraphicImageProcessing.ImageProcessing;

namespace TestPerformance
{
	class InvokeFunctionSet
	{
		public string FunctionName { get; set; }
		public object Owner { get; set; }
		public Func<object, object[], object> InvokeFunc { get; set; }
	}
	class Program
	{
		static void Main(string[] args)
		{
			var dictionary = GetFunctionsToTest();
			while (true)
			{
				Console.WriteLine("Choose Processor in task manager only one process should be");
				Console.ReadLine();
#if DEBUG
				Console.WriteLine("Debug Mod. will be more correct in Release mod");
#else
				Console.WriteLine("Release Mod");
#endif
				var array = new KeyValuePair<string,List<InvokeFunctionSet>>[dictionary.Count];
				for (int i = 0; i < dictionary.Count; i++)
				{
					Console.WriteLine(i.ToString() + ": " + array[i].Key);
				}				
				Console.Write("Insert number of test: ");
				string enteredTestNum = Console.ReadLine();
				int numberOfTest = 0;

				if (int.TryParse(enteredTestNum, out numberOfTest))				
					if (-1 < numberOfTest && numberOfTest < array.Length)					
						TestInvokeMethod(array[numberOfTest].Value);					
					else
						Console.WriteLine("Not supported");				
				else
					Console.WriteLine("NaN not a number");

				Console.WriteLine("the end of the tests");
				Console.ReadLine();
			}
			
		}
		/// <summary>
		/// Get dictionary with blocks of the functions to test
		/// </summary>
		/// <returns></returns>
		private static Dictionary<string, List<InvokeFunctionSet>> GetFunctionsToTest()
		{
			var methonds = typeof(FunctionsToTest).GetMethods();
			Dictionary<string, List<InvokeFunctionSet>> result = new Dictionary<string, List<InvokeFunctionSet>>(methonds.Length);
			foreach (var method in methonds)
			{
				foreach (var attribute in method.GetCustomAttributes(typeof(FunctionsForTestAttribute), false))
				{
					string key = ((FunctionsForTestAttribute)attribute).TestName;
					if (result.ContainsKey(key))
						result[key].Add(new InvokeFunctionSet() { FunctionName = method.Name, Owner = null, InvokeFunc = method.Invoke });
					else
						result.Add(key, new List<InvokeFunctionSet>() { new InvokeFunctionSet() { FunctionName = method.Name, Owner = null, InvokeFunc = method.Invoke } });
				}
			}
			return result;
		}
		private static void BitmapTest()
		{
			//int amountTest = 2000;
			//int imageSize = 500;
			//Bitmap bm = new Bitmap(imageSize, imageSize);
			//Graphics.FromImage(bm).Clear(Color.LightGreen);
			//Console.WriteLine("Start timeEasy");
			////var timeEasy = new TimeTestPerformance() { AmountTest = amountTest, TestedFunction = () => { GraphicsProcessing.OptimisationEasy(bm); } }.Start();
			//var timeEasy = new TimeTestPerformance() { AmountTest = amountTest }.Start(FunctionsToTest.OptimisationEasy, imageSize);
			//Console.WriteLine("timeEasy: " + timeEasy.ToString());

			//bm = new Bitmap(imageSize, imageSize);
			//Graphics.FromImage(bm).Clear(Color.LightGreen);
			//Console.WriteLine("Start timePointer");
			////var timePointer = new TimeTestPerformance() { AmountTest = amountTest, TestedFunction = () => { var bm1 = GraphicsProcessing.OptimisationPointer(bm); bm1 = null;} }.Start();
			//var timePointer = new TimeTestPerformance() { AmountTest = amountTest }.Start(FunctionsToTest.OptimisationPointer, imageSize);
			//Console.WriteLine("timePointer: " + timePointer.ToString());

			//bm = new Bitmap(imageSize, imageSize);
			//Graphics.FromImage(bm).Clear(Color.LightGreen);
			//Console.WriteLine("Start timeUnsafe");
			////var timeUnsafe = new TimeTestPerformance() { AmountTest = amountTest, TestedFunction = () => { GraphicsProcessing.OptimisationUnsafe(bm); } }.Start();
			//var timeUnsafe = new TimeTestPerformance() { AmountTest = amountTest }.Start(FunctionsToTest.OptimisationUnsafe, imageSize);
			//Console.WriteLine("timeUnsafe: " + timeUnsafe.ToString());

		}
		private static void TestInvokeMethod (List<InvokeFunctionSet> functions)
		{
			int amountTest = 2000;
			int imageSize = 500;
			Bitmap bm = new Bitmap(imageSize, imageSize);
			foreach (var fn in functions)
			{
				bm = new Bitmap(imageSize, imageSize);
				Graphics.FromImage(bm).Clear(Color.LightGreen);
				Console.WriteLine("Start: " + fn.FunctionName);
				var time = new TimeTestPerformance() { AmountTest = amountTest }.Start();
				Console.WriteLine(fn.FunctionName + ": " + time.ToString());
			}
		}
		private static void LambdaFunctionTest()
		{
			//int amountTest = 2000;
			//int imageSize = 500;
			//Bitmap bm = new Bitmap(imageSize, imageSize);
			//Graphics.FromImage(bm).Clear(Color.LightGreen);
			//Console.WriteLine("Start timeEasy");
			////var timeEasy = new TimeTestPerformance() { AmountTest = amountTest, TestedFunction = () => { GraphicsProcessing.OptimisationEasy(bm); } }.Start();
			//var timeEasy = new TimeTestPerformance() { AmountTest = amountTest }.Start(FunctionsToTest.OptimisationEasy, imageSize);
			//Console.WriteLine("timeEasy: " + timeEasy.ToString());

			//bm = new Bitmap(imageSize, imageSize);
			//Graphics.FromImage(bm).Clear(Color.LightGreen);
			//Console.WriteLine("Start timePointer");
			////var timePointer = new TimeTestPerformance() { AmountTest = amountTest, TestedFunction = () => { var bm1 = GraphicsProcessing.OptimisationPointer(bm); bm1 = null;} }.Start();
			//var timePointer = new TimeTestPerformance() { AmountTest = amountTest }.Start(FunctionsToTest.OptimisationPointer, imageSize);
			//Console.WriteLine("timePointer: " + timePointer.ToString());
		}
	}
}
