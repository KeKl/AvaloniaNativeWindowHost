using System;
using System.Runtime.InteropServices;

namespace Win32
{
	/// <summary>
	/// 	Contains window class information.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	internal struct ExtendedWindowClass
	{
		/// <summary>
		/// 	The size, in bytes, of this structure.
		/// </summary>
		public int Size;
		
		/// <summary>
		/// 	The class style(s).
		/// </summary>
		public ClassStyle Style;
		
		/// <summary>
		/// 	A pointer to the window procedure.
		/// </summary>
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public WindowProcedure WndProc;
		
		/// <summary>
		/// 	The number of extra bytes to allocate following the window-class structure.
		/// </summary>
		public int ClassExtra;
		
		/// <summary>
		/// 	The number of extra bytes to allocate following the window instance.
		/// </summary>
		public int WndExtra;
		
		/// <summary>
		/// 	A handle to the instance that contains the window procedure for the class. 
		/// </summary>
		public IntPtr Instance;
		
		/// <summary>
		/// 	A handle to the class icon.
		/// </summary>
		public IntPtr Icon;
		
		/// <summary>
		/// 	A handle to the class cursor.
		/// </summary>
		public IntPtr Cursor;
		
		/// <summary>
		/// 	A handle to the class background brush.
		/// </summary>
		public IntPtr Background;
		
		/// <summary>
		/// 	Pointer to a null-terminated character string that specifies the resource name 
		/// 	of the class menu, as the name appears in the resource file.
		/// </summary>
		public IntPtr MenuName;
		
		/// <summary>
		/// 	A pointer to a null-terminated string or is an atom.
		/// </summary>
		public IntPtr ClassName;
		
		/// <summary>
		/// 	A handle to a small icon that is associated with the window class.
		/// </summary>
		public IntPtr IconSm;
		
		/// <summary>
		/// 	The size, in bytes, of this structure.
		/// </summary>
		public static int SizeInBytes = Marshal.SizeOf(default(ExtendedWindowClass));
	}
}