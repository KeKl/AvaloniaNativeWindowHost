using System;

namespace Platform
{
	/// <summary>
	/// 	A dummy window.
	/// </summary>
	public class DummyWindow : INativeWindow
	{
		public DummyWindow()
		{
		}
		
		public event Action<object, bool> TabOut;

		public IntPtr Handle
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public int X
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		
		public int Y
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		
		public int Width
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		
		public int Height
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public bool HasKeyboardFocus
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}
		
		public void GetWindowRectangle(out int x, out int y, out int width, out int height)
		{
			throw new NotImplementedException();
		}
		
		public void Resize(int x, int y, int width, int height)
		{
			throw new NotImplementedException();
		}

		public void Transform(double m11, double m12, double m21, double m22, double m31, double m32)
		{
			throw new NotImplementedException();
		}
		
		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
