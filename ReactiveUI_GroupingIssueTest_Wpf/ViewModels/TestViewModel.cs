using DynamicData;
using ReactiveUI;
using ReactiveUI_GroupingIssueTest_Wpf.Enums;
using ReactiveUI_GroupingIssueTest_Wpf.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI_GroupingIssueTest_Wpf.ViewModels
{
    public class TestViewModel : ReactiveObject
    {
        private readonly Random rnd;
        private readonly SourceCache<Male, int> cache;

        private readonly ReadOnlyObservableCollection<IGroup<Male, int, Greek>> maleGroups;
        public ReadOnlyObservableCollection<IGroup<Male, int, Greek>> MaleGroups => maleGroups;
        

        public TestViewModel()
        {
            rnd = new Random();
            cache = new SourceCache<Male, int>(male=> male.Id);
            cache.AddOrUpdate(Enumerable.Range(1, 11).Select(i => new Male(i, Greek.Alpha)));

            cache.Connect()
                .AutoRefresh(male => male.FavouriteGreekLetter)
                .Group(male => male.FavouriteGreekLetter)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out maleGroups)
                .Subscribe();

            Observable.Interval(TimeSpan.FromSeconds(1))
                .Select(s => cache.Items.Skip(rnd.Next(0, cache.Count)).FirstOrDefault())
                .WhereNotNull()
                .Do(male => male.FavouriteGreekLetter = Enum.GetValues(typeof(Greek)).OfType<Greek>().OrderBy(e => Guid.NewGuid()).FirstOrDefault())
                .Subscribe();
        }
    }
}
