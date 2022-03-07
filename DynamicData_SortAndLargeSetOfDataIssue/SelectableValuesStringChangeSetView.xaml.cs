using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
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

namespace DynamicData_SortAndLargeSetOfDataIssue;
/// <summary>
/// Interaction logic for SelectableValueStringChangeSetView.xaml
/// </summary>
public partial class SelectableValuesStringChangeSetView : ReactiveUserControl<SelectableValuesStringChangeSetViewModel>
{
    public SelectableValuesStringChangeSetView()
    {
        InitializeComponent();
        this.WhenActivated(d =>
        {
            this.OneWayBind(ViewModel, vm => vm.SelectableStrings, v => v.Values_DataGrid.ItemsSource)
                .DisposeWith(d);
            this.Bind(ViewModel, vm=>vm.SearchText, v=>v.Search_TextBox.Text)
                .DisposeWith(d);

            this.BindCommand(ViewModel, vm=>vm.SelectAllCommand, v=>v.SelectAll_Button)
                .DisposeWith(d);
            this.BindCommand(ViewModel, vm=>vm.DeselectAllCommand, v=>v.DeselectAll_Button)
                .DisposeWith(d);

            Values_DataGrid.Events()
                .SelectionChanged
                .Select(_ => Values_DataGrid.SelectedItems.Cast<SelectableValue<string>>().ToList())
                .BindTo(ViewModel, vm => vm.SelectedItems)
                .DisposeWith(d);

            Values_DataGrid.Events()
                .PreviewKeyDown
                .Where(x=>x.Key==Key.Space)
                .Select(_=>Unit.Default)
                .InvokeCommand(ViewModel,vm=>vm.InvertSelectedItemsCommand)
                .DisposeWith(d);
        });
    }
}
