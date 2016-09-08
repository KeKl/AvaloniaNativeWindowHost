using System;
using System.Runtime.InteropServices;
using Platform;

namespace Win32
{
	/// <summary>
	/// Description of Win32AvaloniaWindow.
	/// </summary>
	public class Win32AvaloniaWindow : Win32Window, INativeWindow
	{
		public Win32AvaloniaWindow(IntPtr parent)
			: base(parent)
		{
			
		}
		
		private bool _mouseTracking;
		private bool _isShiftDown;
		
		public event Action<object, bool> TabOut;
		
		public int X
		{
			get;
			private set;
		}
		
		public int Y
		{
			get;
			private set;
		}

		public int Width
		{
			get;
			private set;
		}

		public int Height
		{
			get;
			private set;
		}
		
		protected override IntPtr CreateWindow(int x, int y, int width, int height, string title, IntPtr parentHandle)
		{
            // Set ex style similiar to wpf
            var value1 = (ExtendedWindowStyle)Functions.SetWindowLong(
                parentHandle, -20, (int)(ExtendedWindowStyle.WindowEdge | ExtendedWindowStyle.ApplicationWindow));
            
            // Set style similiar to wpf, without ClipChildren -> flicker
            var value2 = (WindowStyle)Functions.SetWindowLong(
                parentHandle, -16, (int)(WindowStyle.ClipSiblings | WindowStyle.ClipChildren | WindowStyle.Caption | WindowStyle.SystemMenu | WindowStyle.ThickFrame | WindowStyle.Group | WindowStyle.TabStop));
            
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
					//HasKeyboardFocus = true;
					System.Diagnostics.Debug.WriteLine("LBUTTONDOWN.");
					break;				
				
				#endregion
				
				#region keyBoard
								
				case WindowMessage.KEYDOWN:
				case WindowMessage.SYSKEYDOWN:
					System.Diagnostics.Debug.WriteLine("KEYDOWN.");
					OnKeyDown(VirtualKeyToKeyTranslator.Translate((VirtualKey)wParam.ToInt32()));					
					break;
				case WindowMessage.KEYUP:
				case WindowMessage.SYSKEYUP:
					System.Diagnostics.Debug.WriteLine("KEYUP.");
					OnKeyUp(VirtualKeyToKeyTranslator.Translate((VirtualKey)wParam.ToInt32()));					
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
					return IntPtr.Zero;
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
		
		protected void OnKeyDown(Keys key)
		{
			if (key == Keys.Shift)
				_isShiftDown = true;
						
			if(key == Keys.Tab)
			{
				this.HasKeyboardFocus = false;
				
				var to = TabOut;
				if(to != null)
					to(this, _isShiftDown);
			}
		}
		
		protected void OnKeyUp(Keys key)
		{
			if (key == Keys.Shift)
				_isShiftDown = false;			
		}
				
		public void GetWindowRectangle(out int x, out int y, out int width, out int height)
		{
			var rect = new Win32Rectangle();
			Functions.GetWindowRect(this.Handle, ref rect);
			var parent = Functions.GetParent(this.Handle);
			
			var point = new Point(rect.Left, rect.Top);
			Functions.ScreenToClient(parent, ref point);
						
			x = point.X;
			y = point.Y;
			width = rect.Width;
			height = rect.Height;
		}
		
		public virtual void Resize(int x, int y, int width, int height)
		{
			this.X = x;
			this.Y = y;
			this.Width = width;
			this.Height = height;
						
			Functions.SetWindowPos(
				Handle, IntPtr.Zero, 
				x, y, 
				width, height,
				0x4000 | 0x0010 | 0x0100 | 0x0004);
		}

		public void Transform(double m11, double m12, double m21, double m22, double m31, double m32)
		{
			var form = new XForm((float)m11, (float)m12, (float)m21, (float)m22, (float)m31, (float)m32);
			
			var region = Functions.CreateRectRgn(this.X, this.Y, this.X + Width, this.Y + Height);
			var size = Functions.GetRegionData(region, 0, IntPtr.Zero);
			
			var data = Marshal.AllocHGlobal(size);
			size = Functions.GetRegionData(region, size, data);
			
			var rgn = Functions.ExtCreateRegion(ref form, size, data);
			
			//var ok = Functions.SetWindowRgn(Handle, rgn, true);
		}
	}
}
