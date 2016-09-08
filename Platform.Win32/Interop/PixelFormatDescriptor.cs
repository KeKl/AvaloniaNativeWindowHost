using System;
using System.Runtime.InteropServices;

namespace Win32
{
	/// <summary>
	/// Description of PixelFormatDescriptor.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	internal struct PixelFormatDescriptor
	{
		ushort nSize;
		ushort nVersion;
		Flags dwFlags;
		PixelType iPixelType;
		byte cColorBits;
		byte cRedBits;
		byte cRedShift;
		byte cGreenBits;
		byte cGreenShift;
		byte cBlueBits;
		byte cBlueShift;
		byte cAlphaBits;
		byte cAlphaShift;
		byte cAccumBits;
		byte cAccumRedBits;
		byte cAccumGreenBits;
		byte cAccumBlueBits;
		byte cAccumAlphaBits;
		byte cDepthBits;
		byte cStencilBits;
		byte cAuxBuffers;
		LayerTypes iLayerType;
		byte bReserved;
		uint dwLayerMask;
		uint dwVisibleMask;
		uint dwDamageMask;
		
		public void Init()
		{
			nSize = (ushort)Marshal.SizeOf(typeof(PixelFormatDescriptor));
			nVersion = 1;
			dwFlags = Flags.PFD_DRAW_TO_WINDOW | Flags.PFD_SUPPORT_OPENGL | Flags.PFD_DOUBLEBUFFER;
			iPixelType = PixelType.PFD_TYPE_RGBA;
			cColorBits = 32;
			cRedBits = cRedShift = cGreenBits = cGreenShift = cBlueBits = cBlueShift = 0;
			cAlphaBits = cAlphaShift = 0;
			cAccumBits = cAccumRedBits = cAccumGreenBits = cAccumBlueBits = cAccumAlphaBits = 0;
			cDepthBits = 32;
			cStencilBits = cAuxBuffers = 0;
			iLayerType = LayerTypes.PFD_MAIN_PLANE;
			bReserved = 0;
			dwLayerMask = dwVisibleMask = dwDamageMask = 0;
		}
		
		[Flags]
		public enum Flags : uint 
		{
			PFD_DOUBLEBUFFER = 0x00000001,
			PFD_STEREO = 0x00000002,
			PFD_DRAW_TO_WINDOW = 0x00000004,
			PFD_DRAW_TO_BITMAP = 0x00000008,
			PFD_SUPPORT_GDI = 0x00000010,
			PFD_SUPPORT_OPENGL = 0x00000020,
			PFD_GENERIC_FORMAT = 0x00000040,
			PFD_NEED_PALETTE = 0x00000080,
			PFD_NEED_SYSTEM_PALETTE = 0x00000100,
			PFD_SWAP_EXCHANGE = 0x00000200,
			PFD_SWAP_COPY = 0x00000400,
			PFD_SWAP_LAYER_BUFFERS = 0x00000800,
			PFD_GENERIC_ACCELERATED = 0x00001000,
			PFD_SUPPORT_DIRECTDRAW = 0x00002000,
			PFD_DIRECT3D_ACCELERATED = 0x00004000,
			PFD_SUPPORT_COMPOSITION = 0x00008000,
			PFD_DEPTH_DONTCARE = 0x20000000,
			PFD_DOUBLEBUFFER_DONTCARE = 0x40000000,
			PFD_STEREO_DONTCARE = 0x80000000
		}
		
		public enum LayerTypes : byte
		{
			PFD_MAIN_PLANE = 0,
			PFD_OVERLAY_PLANE = 1,
			PFD_UNDERLAY_PLANE = 255
		}
		
		public enum PixelType : byte
		{
			PFD_TYPE_RGBA = 0,
			PFD_TYPE_COLORINDEX = 1
		}
	}
}
