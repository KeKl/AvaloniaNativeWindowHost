using System;
using System.Runtime.InteropServices;

namespace Win32
{
	internal static class OpenGL
	{
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
		public static extern bool SetPixelFormat(IntPtr hdc, int iPixelFormat, ref PixelFormatDescriptor ppfd);
		
		[DllImport("gdi32.dll", SetLastError = true, ExactSpelling = true)]
		public static extern int ChoosePixelFormat(IntPtr hdc, [In] ref PixelFormatDescriptor ppfd);
		
		[DllImport("gdi32.dll", ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
		public static extern void SwapBuffers(IntPtr hDC);
		
		[DllImport("opengl32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
		public static extern IntPtr wglCreateContext(IntPtr hDC);
		
		[DllImport("opengl32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
		public static extern bool wglMakeCurrent(IntPtr hDC, IntPtr hRC);
		
		[DllImport("opengl32.dll", ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
		public static extern void glClearColor(Single red, Single green, Single blue, Single alpha);
		
		[DllImport("opengl32.dll", ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
		public static extern void glViewport(int x, int y, int width, int height);
		
		[DllImport("opengl32.dll", EntryPoint = "glBegin", ExactSpelling = true)] 
		public extern static void glBegin(int mode);
		
		[DllImport("opengl32.dll", EntryPoint = "glEnd", ExactSpelling = true)]
		public extern static void glEnd();
		
		[DllImport("opengl32.dll", EntryPoint = "glColor3f", ExactSpelling = true)]
		public extern static void glColor3f(Single red, Single green, Single blue);

		[DllImport("opengl32.dll", EntryPoint = "glVertex3f", ExactSpelling = true)]
		public extern static void glVertex3f(Single x, Single y, Single z);
		
		[DllImport("opengl32.dll", ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
		public static extern void glClear(int mask);
	}
}
