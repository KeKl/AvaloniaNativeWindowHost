using System;
using Avalonia.Controls;
using Platform;

namespace VulkanSample
{
	public class VulkanControl : NativeWindowHost
	{		
		private static INativeWindow _window;
		
		/// <summary>
		/// 	Updates the natives window position. Derived classes override
		/// 	this method to set the position of the native window.
		/// </summary>
		/// <param name="rectangle">
		/// 	The rectangle the new window has to be positioned.
		/// </param>
		/// <seealso cref="GetWindowStartRectangle"/>
        protected override void UpdateWindowPosition(Avalonia.Rect rectangle)
        {
        	_window.Resize((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height);
        }
		
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
		protected override Avalonia.Platform.IPlatformHandle Build(Avalonia.Platform.IPlatformHandle parent)
		{
			_window = new VulkanSurface(parent.Handle);			
						
			return new Avalonia.Platform.PlatformHandle(_window.Handle, "ChildWindow");
		}
		
		/// <summary>
		/// 	Destroys the hosted window. Derived classes should override
		/// 	this method to destroy the hosted window.
		/// </summary>
		/// <param name="child">
		/// 	The platform handle of the child.
		/// </param>
		protected override void Destroy(Avalonia.Platform.IPlatformHandle child)
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
		protected override Avalonia.Rect GetWindowStartRectangle()
		{
			// Typically windows has x, y, ... properties
			int x, y, width, height;			
			_window.GetWindowRectangle(out x, out y, out width, out height);
						
			return new Avalonia.Rect(x, y, width, height);
		}
	}
}