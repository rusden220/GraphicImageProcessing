using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace GraphicImageProcessing
{
	public class ImageProcessingProvider
	{
		private object _lockObject;	
		private Queue<Action> _taskCollection;//contain all task, should be run
		private Thread _mainThread;

		public void AddToRun(Action action)
		{
			lock (_lockObject)
			{
				_taskCollection.Enqueue(action);
			}
			TaskRunner();
		}
		private void TaskRunner()
		{
			
		}
		/// <summary>
		/// Stop running function
		/// </summary>
		public void Brake()
		{

		}

		
	}
}
