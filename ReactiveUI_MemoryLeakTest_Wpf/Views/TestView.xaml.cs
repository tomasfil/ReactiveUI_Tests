
using ReactiveUI;
using ReactiveUI_MemoryLeakTest_Wpf.ViewModels;
using System.Reactive.Disposables;
using System.Windows.Controls;

namespace ReactiveUI_MemoryLeakTest_Wpf.Views
{
    /// <summary>
    /// Interaction logic for TestView.xaml
    /// </summary>
    public partial class TestView : ReactiveUserControl<TestViewModel>
    {
        public TestView()
        {
            InitializeComponent();
            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.TestModel.Id, v => v.t.Text)
                    .DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.TestModel.Name, v => v.tt.Text)
                    .DisposeWith(d);
                this.OneWayBind(ViewModel, vm => vm.TestModel.Duration, v => v.ttt.Text)
                    .DisposeWith(d);
            });
        }
    }
}
