using ReactiveUI;
using ReactiveUI_Akavache_Suspension.ViewModels;
using System.Windows;

namespace ReactiveUI_Akavache_Suspension
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = RxApp.SuspensionHost.GetAppState<MainViewModel>();
        }
    }
}