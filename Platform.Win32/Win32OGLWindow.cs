using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Win32
{
	/// <summary>
	/// Description of Win32OGLWindow.
	/// </summary>
	public class Win32OGLWindow : Win32AvaloniaWindow
	{
		public Win32OGLWindow(IntPtr parent)
			: base(parent)
		{
			this.RenderingContext = CreateContext();
		}
		
		public IntPtr DeviceContext { get { return Functions.GetDC(this.Handle); } }
		
		public IntPtr RenderingContext { get; private set; }
		
		private IntPtr CreateContext()
		{
			Functions.LoadLibrary("opengl32.dll");
			
			var pf = new PixelFormatDescriptor();
			pf.Init();
			
			var pixelFormat = OpenGL.ChoosePixelFormat(DeviceContext, ref pf);
			if(!OpenGL.SetPixelFormat(DeviceContext, pixelFormat, ref pf))
				throw new Win32Exception(Marshal.GetLastWin32Error());
			
			IntPtr hglrc;
			if((hglrc = OpenGL.wglCreateContext(DeviceContext)) == IntPtr.Zero)
				throw new Win32Exception(Marshal.GetLastWin32Error());
			
			return hglrc;
		}
		
		protected override void Draw()
		{
			if(!OpenGL.wglMakeCurrent(this.DeviceContext, this.RenderingContext))
				throw new Win32Exception(Marshal.GetLastWin32Error());
			
			OpenGL.glClearColor(1.0f, 0.3f, 0.1f, 0.0f);			
			OpenGL.glClear(0x00004000);
			
			OpenGL.glBegin(0x0004);
			
			OpenGL.glColor3f(1.0f, 0.0f, 0.0f);
			OpenGL.glVertex3f(-1.0f, -1.0f, 0.0f);

			OpenGL.glColor3f(0.0f, 1.0f, 0.0f);
			OpenGL.glVertex3f(0.0f, 1.0f, 0.0f);

    		OpenGL.glColor3f(0.0f, 0.0f, 1.0f);
    		OpenGL.glVertex3f(1.0f, -1.0f, 0.0f);
			
			OpenGL.glEnd();
			
			OpenGL.SwapBuffers(DeviceContext);
		}
		
		public override void Resize(int x, int y, int width, int height)
		{
			base.Resize(x, y, width, height);
			
			OpenGL.glViewport(0, 0, this.Width, this.Height);
		}
	}
}
