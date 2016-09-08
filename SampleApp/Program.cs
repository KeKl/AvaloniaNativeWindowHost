using System;
using System.Windows.Threading;
using System.Linq;
using Avalonia.Controls;

namespace SampleApp
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
