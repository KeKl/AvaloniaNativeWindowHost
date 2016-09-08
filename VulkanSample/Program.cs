using System;
using Avalonia.Controls;
using System.Windows.Threading;

namespace VulkanSample
{
	class Program
	{
		static void Main(string[] args)
        {
        	Dispatcher foo = Dispatcher.CurrentDispatcher;
                        
            AppBuilder.Configure<App>()
            	.UsePlatformDetect()
            	.Start<MainWindow>();
        }
	}
}