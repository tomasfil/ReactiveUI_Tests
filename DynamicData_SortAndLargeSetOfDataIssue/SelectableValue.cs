using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicData_SortAndLargeSetOfDataIssue
{
    public class SelectableValue<TValue> : ReactiveObject, IDisposable
    {
        private readonly CompositeDisposable disposables;
        public SelectableValue(TValue value, bool isSelected = false)
        {
            disposables = new CompositeDisposable();
            _value = value;
            IsSelected = isSelected;

            this.WhenAnyValue(x => x._value)
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToPropertyEx(this, x => x.Value)
                .DisposeWith(disposables);
        }

        [ObservableAsProperty]
        public TValue Value { get; private set; }

        [Reactive]
        private TValue _value { get; set; }
        [Reactive]
        public bool IsSelected { get; set; }

        public void SetValue(TValue value)
        {
            _value = value;
        }

        public void Dispose()
        {
            disposables.Dispose();
        }
    }
}
