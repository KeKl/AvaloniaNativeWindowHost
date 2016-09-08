using System;

namespace Platform
{
	/// <summary>
	/// 	Defines the interface for a native window.
	/// </summary>
	public interface INativeWindow : IDisposable
	{
		event Action<object, bool> TabOut;		
		
		/// <summary>
		/// 	The native handle to the window.
		/// </summary>
		IntPtr Handle { get; }
		
		/// <summary>
		/// 	Gets or sets the position of the window's left edge.	
		/// </summary>
		int X
		{
			get;
		}

		/// <summary>
		/// 	Gets or sets the position of the window's top edge.
		/// </summary>
		int Y
		{
			get;
		}

		/// <summary>
		/// 	Gets or sets the width of the window.
		/// </summary>
		int Width
		{
			get;
		}

		/// <summary>
		/// 	Gets or sets the height of the window.
		/// </summary>
		int Height
		{
			get;
		}

		/// <summary>
		/// 	Gets a value that indicates whether the window has focus.
		/// </summary>
		bool HasKeyboardFocus
		{
			get;
			set;
		}
		
		/// <summary>
		/// 	Return the current rectangle of the window.
		/// </summary>
		/// <param name="x">
		/// 	The position of the window's left edge.	
		/// </param>
		/// <param name="y">
		/// 	The position of the window's top edge.
		/// </param>
		/// <param name="width">
		/// 	The width of the window.
		/// </param>
		/// <param name="height">
		/// 	The height of the window.
		/// </param>
		void GetWindowRectangle(out int x, out int y, out int width, out int height);
		
		/// <summary>
		/// 	Resizes the window.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		void Resize(int x, int y, int width, int height);

		/// <summary>
		/// 	Transforms the window.
		/// </summary>
		/// <param name="m11">
		/// 	The element m11 of the transformation matrix.
		/// </param>
		/// <param name="m12">
		/// 	The element m11 of the transformation matrix.
		/// </param>
		/// <param name="m21">
		/// 	The element m11 of the transformation matrix.
		/// </param>
		/// <param name="m22">
		/// 	The element m11 of the transformation matrix.
		/// </param>
		/// <param name="m31">
		/// 	The element m11 of the transformation matrix.
		/// </param>
		/// <param name="m32">
		/// 	The element m11 of the transformation matrix.
		/// </param>
		void Transform(double m11, double m12, double m21, double m22, double m31, double m32);
	}
}
