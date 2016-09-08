using System;

namespace Win32
{
	internal delegate IntPtr WindowProcedure(IntPtr handle, WindowMessage message, IntPtr wParam, IntPtr lParam);
}
