using Akavache;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Akavache_InMemoryCacheTests
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random rnd = new Random();
        public MainWindow()
        {
            InitializeComponent();
            Akavache.Registrations.Start("AkavaTest");
            RunCycle();
        }

        private void RunCycle()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    var parameter = rnd.Next(0, 100);
                    IntModel? intModel = await BlobCache.InMemory.GetOrFetchObject(IntModel.GetCacheIdentifier(parameter), () => GetIntAsync(parameter));
                    //IntModel? intModel = await BlobCache.LocalMachine.GetOrFetchObject(IntModel.GetCacheIdentifier(parameter), () => GetIntAsync(parameter));

                    if(intModel is null)
                    {
                        Debugger.Break();
                    }

                    Debug.WriteLine(intModel.IntString.Length);
                    Debug.WriteLine(intModel.IntString);
                    await Task.Delay(rnd.Next(1, 100));
                }
            });
        }

        private async Task<IntModel> GetIntAsync(int val)
        {
            // Database IO call
            await Task.Delay(15);
            return new IntModel(val.ToString());

            
        }
    }
}
