using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Platform;
using Win32;

namespace WPFComparison
{
	public class NativeControl : HwndHost
	{
		public NativeControl()
		{
			
		}
		        
		private static Win32WPFWindow _window;
				
		protected override HandleRef BuildWindowCore(HandleRef hwndParent)
		{
			_window = new Win32WPFWindow(hwndParent.Handle);
			
			return new HandleRef(this, _window.Handle);
		}
		
		protected override void DestroyWindowCore(HandleRef hwnd)
		{
			_window.Dispose();
		}
		
		protected override bool TabIntoCore(TraversalRequest request)
		{
			if(request.FocusNavigationDirection == FocusNavigationDirection.Next)
			{
				// SetFocus for first control in native window
				_window.HasKeyboardFocus = true;
			}
			else
			{
				// SetFocus for last control in native window
				_window.HasKeyboardFocus = true;
			}
			
			return true;
		}
		
		[DllImport("user32.dll", SetLastError = true)]
		private static extern short GetKeyState(int nVirtKey);
		
		private const int WM_KEYDOWN = 0x0100;
		private const int SYSCHAR = 0x0106;
		private const int VK_TAB = 0x09;
		private const int VK_SHIFT = 0x10;
		private const int KEY_PRESSED = 0x8000;
		
		protected override bool TranslateAcceleratorCore(ref MSG msg, ModifierKeys modifiers)
		{
			if(msg.message == WM_KEYDOWN && msg.wParam == new IntPtr(VK_TAB))
			{
				if(Convert.ToBoolean(GetKeyState(VK_SHIFT) & KEY_PRESSED))
					return ((IKeyboardInputSink)this)
						.KeyboardInputSite
						.OnNoMoreTabStops(new TraversalRequest(FocusNavigationDirection.Previous));
				else
					return ((IKeyboardInputSink)this)
						.KeyboardInputSite
						.OnNoMoreTabStops(new TraversalRequest(FocusNavigationDirection.Next));
			}
			return base.TranslateAcceleratorCore(ref msg, modifiers);
		}
		
		protected override bool OnMnemonicCore(ref MSG msg, ModifierKeys modifiers)
		{
			if(msg.message == SYSCHAR && (modifiers == ModifierKeys.Alt))
			{
				var key = (char)msg.wParam.ToInt32();
					if (key == 'a')
						return _window.HasKeyboardFocus = true;
			}
			
			return base.OnMnemonicCore(ref msg, modifiers);
		}
	}
}