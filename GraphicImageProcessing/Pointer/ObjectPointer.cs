using System;
using System.Runtime.InteropServices;

namespace GraphicImageProcessing.Pointer
{
	[StructLayout(LayoutKind.Explicit)]
	public class ObjectPointer
	{
		[FieldOffset(0)]
		private ObjectWrapper _object;
		[FieldOffset(0)]
		private IntPointerWrapper _intPointer;
		[FieldOffset(0)]
		private object _emptyObject;

		class ObjectWrapper
		{
			public object _object;
		}
		class IntPointerWrapper
		{
			public int _intPointer;
		}
		class ArrayPointerWrapper<T>
		{
			public T[] _arrayPointer;
		}

		public int IntPointer
		{
			get { return _intPointer._intPointer; }
			set { _intPointer._intPointer = value; }
		}
		
		public ObjectPointer(object obj)
		{
			if (obj == null) throw new NullReferenceException();
			_object = new ObjectWrapper();
			_object._object = obj;
		}
		public T[] GetArrayPointer<T>()
		{
			int ptr = _intPointer._intPointer;
			_emptyObject = new ArrayPointerWrapper<T>();
			_intPointer._intPointer = ptr;
			return ((ArrayPointerWrapper<T>)_emptyObject)._arrayPointer;
		}
		
	}
}
