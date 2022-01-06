using ReactiveUI;
using ReactiveUI_MemoryLeakTest_Wpf.ViewModels;
using System;
using Splat;
using System.Collections.Generic;
using System.Linq;
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
using System.Reactive.Disposables;

namespace ReactiveUI_MemoryLeakTest_Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ReactiveWindow<MainViewModel>
    {
        public MainWindow()
        {
            ViewModel = new MainViewModel();
            InitializeComponent();

            this.WhenActivated(d => {
                this.OneWayBind(ViewModel, vm => vm.Tests, v => v.TestIC.ItemsSource)
                    .DisposeWith(d);
            });
           
        }
    }
}
