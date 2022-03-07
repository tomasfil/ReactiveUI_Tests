using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskPoolCanExecute
{
    public class MainViewModel : ReactiveObject
    {
        [Reactive]
        private bool canExecuteField { get; set; }
        public ReactiveCommand<Unit, Unit> DebugCommand { get; }

        public MainViewModel()
        {
            Observable.Interval(TimeSpan.FromSeconds(5))
                .ObserveOn(RxApp.TaskpoolScheduler)
                .Select(_ => !canExecuteField)
                .BindTo(this, x => x.canExecuteField);

            var canExecuteChanged = this.WhenAnyValue(x => x.canExecuteField)
                .ObserveOn(RxApp.TaskpoolScheduler);
            DebugCommand = ReactiveCommand.Create(() => Debug.WriteLine("Command fire"), canExecuteChanged);
        }
    }
}
