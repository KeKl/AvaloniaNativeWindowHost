using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform;
using Platform;

namespace SampleApp
{
	public class NativeControl : NativeWindowHost
	{
		public NativeControl()
		{
            this.Built += (sender, e) => 
            {
//				// Do init stuff.
//				_window.TabOut += (innerSender, innerArgs) => 
//				{
//					if(innerArgs)
//						TabOutCore(FocusNavigationDirection.Previous);
//					else
//						TabOutCore(FocusNavigationDirection.Next);
//				};				
            };
		}
		        
		private static INativeWindow _window;
		
		/// <summary>
		/// 	Creates the window to be hosted. Derived classes override this
		/// 	method to build the window being hosted.
		/// </summary>
		/// <param name="parent">
		/// 	The platform handle of the parent window.
		/// </param>
		/// <returns>
		/// 	The platform handle of the created window.
		/// </returns>
		protected override IPlatformHandle Build(IPlatformHandle parent)
		{
			_window = WindowFactory.CreatePlatformWindow(parent.Handle);
			
			return new PlatformHandle(_window.Handle, "ChildWindow");			
		}
		
		/// <summary>
		/// 	Destroys the hosted window. Derived classes should override
		/// 	this method to destroy the hosted window.
		/// </summary>
		/// <param name="child">
		/// 	The platform handle of the child.
		/// </param>
		protected override void Destroy(IPlatformHandle child)
		{
			_window.Dispose();
		}
		
		/// <summary>
		/// 	Returns the rectangle of the native window. Derived classes override
		/// 	this to set the window rectangle relative to the control.
		/// </summary>
		/// <returns>
		/// 	The rectangle of the native window.
		/// </returns>
		/// <seealso cref="UpdateWindowPosition"/>
		protected override Rect GetWindowStartRectangle()
		{
			// Typically windows has x, y, ... properties
			int x, y, width, height;			
			_window.GetWindowRectangle(out x, out y, out width, out height);
						
			return new Rect(x, y, width, height);
		}
				
		/// <summary>
		/// 	Updates the natives window position. Derived classes override
		/// 	this method to set the position of the native window.
		/// </summary>
		/// <param name="rectangle">
		/// 	The rectangle the new window has to be positioned.
		/// </param>
		/// <seealso cref="GetWindowStartRectangle"/>
        protected override void UpdateWindowPosition(Rect rectangle)
        {
        	_window.Resize((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height);
        }
        
        /// <summary>
        /// 	Updates the transformation of the window.
        /// </summary>
        /// <param name="transformationMatrix">
        /// 	The transformation matrix to apply.
        /// </param>
		protected override void UpdateWindowTrans(Matrix transformationMatrix)
		{
			_window.Transform(
				transformationMatrix.M11, 
				transformationMatrix.M12, 
				transformationMatrix.M21, 
				transformationMatrix.M22, 
				transformationMatrix.M31, 
				transformationMatrix.M32
			);			
		}
		
//		protected override bool TabIntoCore(FocusNavigationDirection direction)
//		{			
//			if(direction == FocusNavigationDirection.Next)
//			{
//				// SetFocus for first control in native window
//				_window.HasKeyboardFocus = true;
//			}
//			else
//			{
//				// SetFocus for last control in native window
//				_window.HasKeyboardFocus = true;
//			}
//			return true;
//		}
//		
//		protected override void TabOutCore(FocusNavigationDirection direction)
//		{
//			var next = KeyboardNavigationHandler.GetNext(this, direction);
//			KeyboardDevice.Instance.SetFocusedElement(this, NavigationMethod.Tab, InputModifiers.None);
//			
//			this.OnLostFocus(new Avalonia.Interactivity.RoutedEventArgs());
//		}
    }
}