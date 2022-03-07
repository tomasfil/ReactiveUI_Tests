using DynamicData.Binding;
using DynamicData;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace DynamicData_SortAndLargeSetOfDataIssue
{
    public class SelectableValuesStringChangeSetViewModel : ReactiveObject
    {
        [Reactive]
        public string SearchText { get; set; }
        public SelectableValuesStringChangeSetViewModel(IObservable<IChangeSet<SelectableValue<string>, string>> changeSet) : this()
        {
            var searchFilter = this.WhenAnyValue(x => x.SearchText)
                .ObserveOn(RxApp.TaskpoolScheduler)
                .Throttle(TimeSpan.FromMilliseconds(300))
                .DistinctUntilChanged()
                .Select(s => s ?? string.Empty)
                .Select(searchText => new Func<SelectableValue<string>, bool>(value => string.IsNullOrEmpty(searchText) || value.IsSelected || value.Value?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true));


            changeSet
                .Filter(searchFilter)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Sort(SortExpressionComparer<SelectableValue<string>>.Ascending(a => a.Value))
                .Bind(out selectableStrings)
                .Subscribe();
        }
        public SelectableValuesStringChangeSetViewModel()
        {

            var canInvert = this.WhenAnyValue(x => x.SelectedItems)
                                .Select(s => s is not null && s.Count > 0)
                                .ObserveOn(RxApp.MainThreadScheduler);

            InvertSelectedItemsCommand = ReactiveCommand.CreateFromObservable(InvertSelectedItemsImpl, canInvert);

            SelectAllCommand = ReactiveCommand.CreateFromObservable(SelectAllImpl);

            DeselectAllCommand = ReactiveCommand.CreateFromObservable(DeselectAllImpl);
        }

        public SelectableValuesStringChangeSetViewModel(IObservable<IChangeSet<SelectableValue<string>, int>> changeSet)
        {
            var searchFilter = this.WhenAnyValue(x => x.SearchText)
              .Select(s => s ?? string.Empty)
              .Select(searchText => new Func<SelectableValue<string>, bool>(value => string.IsNullOrEmpty(searchText) || value.IsSelected || value.Value?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true));


            changeSet
                .ObserveOn(RxApp.TaskpoolScheduler)
                .Filter(searchFilter)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Sort(SortExpressionComparer<SelectableValue<string>>.Ascending(a => a.Value))
                .Bind(out selectableStrings)
                .Subscribe();
        }

        private IObservable<Unit> InvertSelectedItemsImpl()
        {
            return Observable.Start(() =>
            {
                foreach (var item in SelectedItems)
                {
                    item.IsSelected = !item.IsSelected;
                }
            }, RxApp.MainThreadScheduler);
        }
        private IObservable<Unit> DeselectAllImpl()
        {
            return Observable.Start(() =>
            {
                foreach (var item in SelectableStrings.Where(x => x.IsSelected).ToList())
                {
                    item.IsSelected = false;
                }
            }, RxApp.MainThreadScheduler);
        }
        private IObservable<Unit> SelectAllImpl()
        {
            return Observable.Start(() =>
            {
                foreach (var item in SelectableStrings.Where(x => !x.IsSelected).ToList())
                {
                    item.IsSelected = true;
                }
            }, RxApp.MainThreadScheduler);
        }

        [Reactive]
        public List<SelectableValue<string>> SelectedItems { get; set; }

        private readonly ReadOnlyObservableCollection<SelectableValue<string>> selectableStrings;
        public ReadOnlyObservableCollection<SelectableValue<string>> SelectableStrings => selectableStrings;


        public ReactiveCommand<Unit, Unit> InvertSelectedItemsCommand { get; }
        public ReactiveCommand<Unit, Unit> SelectAllCommand { get; }
        public ReactiveCommand<Unit, Unit> DeselectAllCommand { get; }
    }
}
