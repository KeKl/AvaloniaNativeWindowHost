using System;
using System.Runtime.InteropServices;
using Platform;
using Win32;

namespace WPFComparison
{
	public class Win32WPFWindow : Win32Window
	{
		public Win32WPFWindow(IntPtr parent)
			: base(parent)
		{
			
		}
		
		private bool _mouseTracking;
						
		protected override IntPtr CreateWindow(int x, int y, int width, int height, string title, IntPtr parentHandle)
		{            
            var wc = new ExtendedWindowClass();
			wc.Size = ExtendedWindowClass.SizeInBytes;
			wc.Style = ClassStyle.OwnDC | ClassStyle.HorizontalRedraw | ClassStyle.VerticalRedraw;
			wc.Instance = Instance;
			wc.WndProc = WindowProcedureDelegate;
			wc.ClassName = ClassName;
			wc.Icon = IntPtr.Zero;
			wc.IconSm = IntPtr.Zero;
			//wc.Background = IntPtr.Zero;
			wc.Background = Functions.CreateSolidBrush((uint)System.Drawing.ColorTranslator.ToWin32(System.Drawing.Color.Green));
			wc.Cursor = Functions.LoadCursor();
			ushort atom = Functions.RegisterClassEx(ref wc);
			
			if(atom == 0)
				throw new Exception(String.Format("Failed to register window class. Error: {0}", Marshal.GetLastWin32Error()));

			IntPtr window_name = Marshal.StringToHGlobalAuto(title);
			IntPtr handle = Functions.CreateWindowEx(
				0, ClassName, window_name, WindowStyle.Visible | WindowStyle.Child | WindowStyle.ClipChildren | WindowStyle.ClipSiblings,
				x, y, width, height,
				parentHandle, IntPtr.Zero, Instance, IntPtr.Zero);

			if(handle == IntPtr.Zero)
				throw new Exception(String.Format("Failed to create window. Error: {0}", Marshal.GetLastWin32Error())); 
						
			return handle;
		}
		
		protected override IntPtr WindowProcedure(IntPtr handle, WindowMessage message, IntPtr wParam, IntPtr lParam)
		{
			switch(message)
			{
				#region mouse
				
				case WindowMessage.MOUSEMOVE:
					{
						if (!_mouseTracking)
						{
							var tme = new TrackMouseEvent(TrackMouseEventFlags.TME_LEAVE, Handle);
							if (Functions.TrackMouseEvent(ref tme)) 
								_mouseTracking = true;
						}
					}
					break;
				case WindowMessage.MOUSEACTIVATE:
					System.Diagnostics.Debug.WriteLine("MOUSEACTIVATE.");
					break;
				case WindowMessage.MOUSELEAVE:
					_mouseTracking = false;
					System.Diagnostics.Debug.WriteLine("MOUSELEAVE.");
					break;
				case WindowMessage.LBUTTONDOWN:
					HasKeyboardFocus = true;
					System.Diagnostics.Debug.WriteLine("LBUTTONDOWN.");
					break;				
				
				#endregion
				
				#region keyBoard
				
				case WindowMessage.KEYDOWN:
				case WindowMessage.SYSKEYDOWN:
					System.Diagnostics.Debug.WriteLine("KEYDOWN.");			
					break;
				case WindowMessage.KEYUP:
				case WindowMessage.SYSKEYUP:
					System.Diagnostics.Debug.WriteLine("KEYUP.");				
					break;
				
				#endregion
								
				case WindowMessage.SETFOCUS:
					System.Diagnostics.Debug.WriteLine("SETFOCUS.");
					break;
				case WindowMessage.KILLFOCUS:
					System.Diagnostics.Debug.WriteLine("KILLFOCUS.");
					break;					
				case WindowMessage.ACTIVATE:
					System.Diagnostics.Debug.WriteLine("ACTIVATE.");
					break;
				
				case WindowMessage.ERASEBKGND:					
					//return IntPtr.Zero;
					break;
				case WindowMessage.PAINT:
					Draw();
					break;
				
				case WindowMessage.WINDOWPOSCHANGING:
					break;
				
				case WindowMessage.CLOSE:
					Functions.DestroyWindow(Handle);
					break;				
				case WindowMessage.DESTROY:
					Functions.PostQuitMessage(0);
					break;
			}
			
			return Functions.DefWindowProc(handle, message, wParam, lParam);
		}
	}
}