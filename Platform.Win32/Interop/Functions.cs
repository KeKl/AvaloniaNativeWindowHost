using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Win32
{
	internal static class Functions
	{
		[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AdjustWindowRectEx(
			ref Win32Rectangle lpRect,
			WindowStyle dwStyle,
			[MarshalAs(UnmanagedType.Bool)] bool bMenu,
			ExtendedWindowStyle dwExStyle);
		
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern ushort RegisterClassEx(ref ExtendedWindowClass window_class);
		
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern IntPtr CreateWindowEx(
			ExtendedWindowStyle ExStyle,
			IntPtr ClassAtom,
			IntPtr WindowName,
			WindowStyle Style,
			int X, int Y,
			int Width, int Height,
			IntPtr HandleToParentWindow,
			IntPtr Menu,
			IntPtr Instance,
			IntPtr Param);
		
		[DllImport("user32.dll", SetLastError=true)]
 		public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        public static long SetWindowLong(IntPtr hwnd, int index, long newLong)
        {
            if (IntPtr.Size == 8)
                return SetWindowLongPtr(hwnd, index, new IntPtr(newLong)).ToInt64();
            else
            {
                var intlong = (int)newLong;
                return SetWindowLong(hwnd, index, intlong);
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int newlong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr newlong);
        
        [DllImport("user32.dll", SetLastError = true)]
		public static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommand nCmdShow);
		
		[DllImport("user32.dll", SetLastError=true)]
 		public static extern bool SetWindowPos(
			IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint flags);
		
		[DllImport("user32.dll", SetLastError=true)]
 		public static extern bool ScreenToClient(IntPtr hWnd, ref Point point);
		
		[DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public extern static IntPtr DefWindowProc(IntPtr hWnd, WindowMessage msg, IntPtr wParam, IntPtr lParam);
				
		[DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref Win32Rectangle rect);
        
		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DestroyWindow(IntPtr windowHandle);
		
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr GetParent(IntPtr windowHandle);
		
		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr LoadCursor(IntPtr hInstance, IntPtr lpCursorName);
		
		public static IntPtr LoadCursor()
		{
			return LoadCursor(IntPtr.Zero, new IntPtr(32512));
		}
		
		[DllImport("user32.dll", SetLastError=true)]
 		public static extern bool TrackMouseEvent(ref TrackMouseEvent lpEventTrack);
		
		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateSolidBrush(uint crColor);

		[DllImport("gdi32.dll")]
		public static extern int FillRect(IntPtr hdc, ref Win32Rectangle rect, IntPtr intPtr);
		
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr GetFocus();
		
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SetFocus(IntPtr hWnd);
		
		
		
		[DllImport("user32.dll", SetLastError = true)]
		public static extern int GetMessage(out Message lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);
		
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool PeekMessage(out Message lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);
		
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool TranslateMessage([In] ref Message lpMsg);
		
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr DispatchMessage([In] ref Message lpmsg);
		
		[DllImport("user32.dll", SetLastError = true)]
		public static extern void PostQuitMessage(int exitCode);
		
		
				
		[DllImport("gdi32.dll", SetLastError = true, ExactSpelling = true)]
		public static extern IntPtr CreateRectRgn(int left, int top, int right, int bottom);
				
		[DllImport("gdi32.dll", SetLastError = true, ExactSpelling = true)]
		public static extern int GetRegionData(IntPtr region, int count, IntPtr regionData);
		
		[DllImport("gdi32.dll", SetLastError = true, ExactSpelling = true)]
		public static extern IntPtr ExtCreateRegion(ref XForm form, int count, IntPtr data);
		
		[DllImport("gdi32.dll", SetLastError = true, ExactSpelling = true)]
		public static extern int SetWindowRgn(IntPtr window, IntPtr rgn, bool redraw);
				
		[DllImport("kernel32", SetLastError=true, CharSet = CharSet.Ansi)]
		public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)]string lpFileName);
		
		
		
		[DllImport("user32.dll")]
		public static extern IntPtr GetDC(IntPtr hWnd);
	}
}