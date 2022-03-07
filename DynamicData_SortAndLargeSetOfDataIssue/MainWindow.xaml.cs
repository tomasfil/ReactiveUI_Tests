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

namespace DynamicData_SortAndLargeSetOfDataIssue
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ReactiveWindow<MainViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();
            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.SelectableInventoriesViewModel, v => v.SelectableInventories_ViewModelViewHost.ViewModel)
                    .DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.SelectableItemsViewModel, v => v.SelectableItems_ViewModelViewHost.ViewModel)
                    .DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.StockDataRows, v => v.Stocks_DataGrid.ItemsSource)
                    .DisposeWith(d);
            });
        }
    }
}
