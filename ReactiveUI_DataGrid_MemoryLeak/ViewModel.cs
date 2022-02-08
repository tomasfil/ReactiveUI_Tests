using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ReactiveUI_DataGrid_MemoryLeak
{
    public class ViewModel
    {
        public ObservableCollection<TestModel> Models = new ObservableCollection<TestModel>();
        private bool doIt = true;

        public ViewModel()
        {
            Task.Run(StartCycle);
            Task.Run(BreakCycle);
        }

        private async Task BreakCycle()
        {
            await Task.Delay(15000);
            doIt = false;
        }

        private async Task StartCycle()
        {
            while (doIt)
            {
                await Dispatcher.CurrentDispatcher.BeginInvoke(() =>
                {
                    foreach (var model in Enumerable.Range(0, 100).Select(_ => new TestModel()).ToList())
                    {
                        Models.Add(model);
                    }
                });
                await Task.Delay(200);
            }
            Dispatcher.CurrentDispatcher.Invoke(() => Models.Clear());
        }
    }
}