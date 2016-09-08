using System;

namespace Win32
{
	internal enum ShowWindowCommand
	{
		/// <summary>
		/// hides the window and activates another window.
		/// </summary>
		Hide = 0,
		
		/// <summary>
		/// activates and displays a window. if the window is minimized or maximized, the system restores it to its original size and position. an application should specify this flag when displaying the window for the first time.
		/// </summary>
		ShowNormal = 1,
		
		/// <summary>
		/// activates the window and displays it as a minimized window.
		/// </summary>
		ShowMinimized = 2,
		
		/// <summary>
		/// activates the window and displays it as a maximized window.
		/// </summary>
		ShowMaximized = 3,
				
		/// <summary>
		/// displays the window as a minimized window. this value is similar to sw_showminimized, except the window is not activated.
		/// </summary>
		ShownoActivate = 4,
		
		/// <summary>
		/// activates the window and displays it in its current size and position.
		/// </summary>
		Show = 5,
		
		/// <summary>
		/// minimizes the specified window and activates the next top-level window in the z order.
		/// </summary>
		Minimize = 6,
		
		/// <summary>
		/// displays the window as a minimized window. this value is similar to sw_showminimized, except the window is not activated.
		/// </summary>
		ShowMinimizedNotActive = 7,
		
		/// <summary>
		/// displays the window in its current size and position. this value is similar to sw_show, except the window is not activated.
		/// </summary>
		ShowNotActivated = 8,
		
		/// <summary>
		/// activates and displays the window. if the window is minimized or maximized, the system restores it to its original size and position. an application should specify this flag when restoring a minimized window.
		/// </summary>
		Restore = 9,
		
		/// <summary>
		/// sets the show state based on the sw_ value specified in the startupinfo structure passed to the createprocess function by the program that started the application.
		/// </summary>
		ShowDefault = 10,
		
		/// <summary>
		/// windows 2000/xp: minimizes a window, even if the thread that owns the window is not responding. this flag should only be used when minimizing windows from a different thread.
		/// </summary>
		ForceMinimize = 11,
	}
}