using System;
using Avalonia.Input;

namespace Avalonia.Controls
{
	public interface IKeyboardInputSink
	{
		/// <summary>
        ///     Sets focus on either the first tab stop or the last tab stop of the sink.
        /// </summary>
		/// <param name="navigationMethod">
		/// 	Specifies whether focus should be set to the first or the last tab stop.
		/// </param>
		/// <returns>
		/// 	<b>True</b> if the focus has been set as requested. 
		/// 	<b>False</b>, if there are no tab stops.
		/// </returns>
		bool TabInto(NavigationMethod navigationMethod);
		
		/// <summary>
		/// 	Sets focus on either the previous tab stop or the next tab stop of the sink.
		/// </summary>
		/// <param name="navigationMethod">
		/// 	Specifies whether the focus should be set no the previous or the next
		/// 	tab stop relative to this one.
		/// </param>
		void TabOut(NavigationMethod navigationMethod);
	}
}
