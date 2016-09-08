using System;
using System.Runtime.InteropServices;
using Platform;

namespace Win32
{
	public abstract class Win32Window
	{
		protected Win32Window(IntPtr parent)
		{
			WindowProcedureDelegate = WindowProcedure;
			this.Handle = CreateWindow(50, 20, 600, 500, "", parent);
		}
		
		protected readonly IntPtr Instance =
			Marshal.GetHINSTANCE(typeof(Win32AvaloniaWindow).Module);
		
		protected readonly IntPtr ClassName = 
			Marshal.StringToHGlobalAuto(Guid.NewGuid().ToString());		
		
		internal readonly WindowProcedure WindowProcedureDelegate;
							
		public IntPtr Handle { get; set; }
				
		public bool HasKeyboardFocus
		{
			get
			{
				return (Functions.GetFocus() == Handle);
			}
			set
			{
				if(value)
				{
					if (Functions.GetFocus() != Handle)
						Functions.SetFocus(this.Handle);
				}
				else
				{
					Functions.SetFocus(Functions.GetParent(Handle));
				}
			}
		}
				
		protected abstract IntPtr CreateWindow(
			int x, int y, int width, int height, string title, IntPtr parentHandle);
		
		protected abstract IntPtr WindowProcedure(
			IntPtr handle, WindowMessage message, IntPtr wParam, IntPtr lParam);
		
		protected virtual void Draw()
		{
			
		}
		
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		
		protected virtual void Dispose(bool disposing)
		{
			if(disposing)
			{
				// free managed resources
			}

			// free native resources if there are any.
			if(Handle != IntPtr.Zero)
			{
				Functions.DestroyWindow(Handle);
			}
		}
		
		~Win32Window()
		{
			// Finalizer calls Dispose(false)
			Dispose(false);
		}
	}
}