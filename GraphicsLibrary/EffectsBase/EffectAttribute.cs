using System;

namespace ImageProcessing.EffectsBase
{
	[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
	public sealed class EffectAttribute : Attribute
	{
		//readonly string[] _path;

		//public string[] Path
		//{
		//	get { return _path; }
		//}
		public EffectAttribute(/*string[] path*/)
		{
			//this._path = path;
		}
	}

}
