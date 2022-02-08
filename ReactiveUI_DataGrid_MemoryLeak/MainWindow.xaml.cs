using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
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

namespace ReactiveUI_DataGrid_MemoryLeak
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ReactiveWindow<ViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel= new ViewModel();
            this.WhenActivated(d => 
            {
                this.OneWayBind(ViewModel, vm => vm.Models, v => v.Main_DataGrid.ItemsSource)
                .DisposeWith(d);
            });
        }
    }
}
