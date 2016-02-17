using System;

namespace TestPerformance
{
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
	sealed class FunctionsForTestAttribute : Attribute
	{
		/// <summary>
		/// The name of a tests. Each tests block has a unique name 
		/// </summary>
		readonly string _testName;
		public FunctionsForTestAttribute(string blockNumber)
		{
			this._testName = blockNumber;
		}

		public string TestName
		{
			get { return _testName; }
		}
	}
}
