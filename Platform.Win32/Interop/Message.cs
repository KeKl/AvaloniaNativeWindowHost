using System;
using System.Runtime.InteropServices;

namespace Win32
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct Message
	{
		public IntPtr Handle;
		public int Msg;
		public IntPtr WParam;
		public IntPtr LParam;
		public int Time;
		public Point Point;
	}
}