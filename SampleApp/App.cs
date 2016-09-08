using System;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Avalonia.Themes.Default;

namespace SampleApp
{
	public class App : Application
	{
		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}