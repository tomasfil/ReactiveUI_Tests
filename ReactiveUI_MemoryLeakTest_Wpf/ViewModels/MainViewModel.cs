using DynamicData;
using ReactiveUI;
using ReactiveUI_MemoryLeakTest_Wpf.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI_MemoryLeakTest_Wpf.ViewModels
{
    public class MainViewModel : ReactiveObject
    {

        private SourceCache<TestModel, int> sourceCache;
        public ReadOnlyObservableCollection<TestViewModel> Tests => tests;
        private ReadOnlyObservableCollection<TestViewModel> tests;
        private Random random;
        public MainViewModel()
        {
            random = new Random();
            sourceCache = new SourceCache<TestModel, int>(m => m.Id);

            sourceCache.Connect()
                .Transform(model => new TestViewModel(model))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out tests)
                .Subscribe();

            Observable.Interval(TimeSpan.FromMilliseconds(100))
                .Select(_ => new TestModel(random.Next(99999), random.Next(99999).ToString(), TimeSpan.FromMilliseconds(random.Next(99999))))
                .Do(sourceCache.AddOrUpdate)
                .Delay(TimeSpan.FromMilliseconds(90))
                .Do(sourceCache.Remove)
                .Subscribe();

        }
    }
}
