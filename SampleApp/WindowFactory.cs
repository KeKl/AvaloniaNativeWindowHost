using System;
using Platform;

namespace SampleApp
{
	/// <summary>
	/// 	Creates the platform specific window.
	/// </summary>
	public static class WindowFactory
	{
		public static INativeWindow CreatePlatformWindow(IntPtr parent)
		{
			switch (Environment.OSVersion.Platform) 
			{
				case PlatformID.Win32NT:
					return new Win32.Win32OGLWindow(parent);
				default:
					return new DummyWindow();
			}
		}
	}
}