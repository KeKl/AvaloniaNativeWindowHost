using System;
using System.Runtime.InteropServices;

namespace Win32
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct Point
	{
		public int X;
		public int Y;
		
		public Point(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}
	}
}