using System;
using System.Runtime.InteropServices;

namespace Win32
{
	[StructLayout(LayoutKind.Sequential)]
	public struct XForm
	{
		public float M11;
		public float M12;
		public float M21;
		public float M22;
		public float Dx;
		public float Dy;
		
		public XForm(float m11, float m12, float m21, float m22, float dx, float dy)
		{
			this.M11 = m11;
			this.M12 = m12;
			this.M21 = m21;
			this.M22 = m22;
			this.Dx = dx;
			this.Dy = dy;
		}
	}
}