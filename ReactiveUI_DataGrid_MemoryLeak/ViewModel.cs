using DynamicData;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI_DataGrid_MemoryLeak
{
    public class ViewModel
    {
        private readonly SourceCache<TestModel,Guid> _modelsCache;
        private readonly ReadOnlyObservableCollection<TestModel> _models;
        public ReadOnlyObservableCollection<TestModel> Models=>_models;
        public ViewModel()
        {
            _modelsCache = new(model => model.Id);

            _modelsCache.Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _models)
                .Subscribe();

            var cycle=Observable.Interval(TimeSpan.FromSeconds(1))
                .Select(_ => Enumerable.Range(0, 30).Select(_ => new TestModel()).ToList())
                .Do(col => _modelsCache.AddOrUpdate(col))
                .Delay(TimeSpan.FromSeconds(0.5))
                .Do(col => _modelsCache.Remove(col))
                .Subscribe();

            Observable.Start(() => Unit.Default, RxApp.TaskpoolScheduler)
                .Delay(TimeSpan.FromSeconds(30))
                .Subscribe(_ => cycle.Dispose());
        }
    }
}
