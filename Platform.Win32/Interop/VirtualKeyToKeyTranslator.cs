using System;
using Platform;

namespace Win32
{
	/// <summary>
	/// Description of VirtualKeyToKeyTranslator.
	/// </summary>
	internal static class VirtualKeyToKeyTranslator
	{
		public static Keys Translate(VirtualKey vKey)
		{
			switch(vKey)
			{
				case VirtualKey.VK_TAB:
					return Keys.Tab;
				case VirtualKey.VK_SHIFT:
					return Keys.Shift;
				case VirtualKey.VK_MENU:
					return Keys.Alt;
				default:
					return Keys.No;
			}
		}
	}
}
