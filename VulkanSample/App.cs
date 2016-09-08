using System;
using Avalonia;
using Avalonia.Markup.Xaml;

namespace VulkanSample
{
	public class App : Application
	{
		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}