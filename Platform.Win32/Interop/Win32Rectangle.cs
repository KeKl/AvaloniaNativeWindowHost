using System;
using System.Runtime.InteropServices;

namespace Win32
{
	/// <summary>
    /// 	Defines the coordinates of the upper-left and lower-right corners of a rectangle.
    /// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Win32Rectangle
	{
		/// <summary>
		/// 	Specifies the x-coordinate of the upper-left corner of the rectangle.
		/// </summary>
		internal int Left;
		
		/// <summary>
		/// 	Specifies the y-coordinate of the upper-left corner of the rectangle.
		/// </summary>
		internal int Top;

		/// <summary>
		/// 	Specifies the x-coordinate of the lower-right corner of the rectangle.
		/// </summary>
		internal int Right;

		/// <summary>
		/// 	Specifies the y-coordinate of the lower-right corner of the rectangle.
		/// </summary>
		internal int Bottom;

		internal int Width
		{
			get 
			{
				return Right - Left;
			}
		}

		internal int Height 
		{
			get 
			{
				return Bottom - Top;
			}
		}

		public override string ToString()
		{
			return String.Format("({0},{1})-({2},{3})", Left, Top, Right, Bottom);
		}
	}
}