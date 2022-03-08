using DynamicData;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DynamicData_SortAndLargeSetOfDataIssue;

public class MainViewModel : ReactiveObject
{
    private readonly SourceCache<Inventory, string> inventoriesCache;

    internal ReactiveCommand<Unit, List<Inventory>> InitialInventoriesLoadCommand { get; }
    public SelectableValuesStringChangeSetViewModel SelectableInventoriesViewModel { get; }
    public SelectableValuesStringChangeSetViewModel SelectableItemsViewModel { get; }

    public MainViewModel()
    {
        inventoriesCache = new SourceCache<Inventory, string>(inventory => inventory.Name);

        InitialInventoriesLoadCommand = ReactiveCommand.CreateFromObservable(InitialInventoriesLoadImpl);
        InitialInventoriesLoadCommand.Execute().Subscribe();

        LoadStocksCommand = ReactiveCommand.CreateFromObservable<SelectableValue<string>, IList<StockDataRow>>(LoadStocksImpl);

        var publishedSelectableInventories = inventoriesCache.Connect()
           .TransformWithInlineUpdate(inv => new SelectableValue<string>(inv.Name), (val, inv) => val.SetValue(inv.Name))
           .DisposeMany()
           .Publish();

        SelectableInventoriesViewModel = new SelectableValuesStringChangeSetViewModel(publishedSelectableInventories);

        var publishedStockChangeSet = publishedSelectableInventories
            .AutoRefresh(x => x.IsSelected, changeSetBuffer: TimeSpan.FromMilliseconds(50), scheduler: RxApp.TaskpoolScheduler)
             .Filter(f => f.IsSelected)
         .TransformAsync(async selectableValue => await LoadStocksCommand.Execute(selectableValue))
         .TransformMany(dataRows => dataRows, dataRow => dataRow.Guid)
         .Publish();

        var publishedItemsChangeSet = publishedStockChangeSet
          .DistinctValues(stockDataRow => stockDataRow.Stock.Item)
          .Transform(item => new SelectableValue<string>(item))
          .Publish();

        SelectableItemsViewModel = new SelectableValuesStringChangeSetViewModel(publishedItemsChangeSet);

        publishedStockChangeSet
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out stockDataRows)
            .Subscribe();

        publishedItemsChangeSet.Connect();
        publishedStockChangeSet.Connect();
        publishedSelectableInventories.Connect();
    }

    private readonly ReadOnlyObservableCollection<StockDataRow> stockDataRows;
    public ReadOnlyObservableCollection<StockDataRow> StockDataRows => stockDataRows;
    private IObservable<IList<StockDataRow>> LoadStocksImpl(SelectableValue<string> inventorySelectableValue)
    {
        return Observable.Start(() => inventorySelectableValue)
            .Select(s => s.Value)
            .SelectMany(LoadStockDataRowsAsync);
    }

    private Random getRandom=> new Random();

    public ReactiveCommand<SelectableValue<string>, IList<StockDataRow>> LoadStocksCommand { get; }

    private async Task<IList<StockDataRow>> LoadStockDataRowsAsync(string inventory, CancellationToken cancellationToken)
    {
        try
        {
            //IO Call
            await Task.Delay(getRandom.Next(1, 100));

            return Enumerable.Range(0, getRandom.Next(1, 1000))
                .Select(i => new Stock() {
                    CaseNo = $"CaseNo{i}", 
                    ExpirationDate = DateTime.Now.AddHours(i),
                    Item = $"ItemNumber-{getRandom.Next(1, 10000)}",
                    Inventory=inventory
                })
                .Select(stock => new StockDataRow(stock)).ToList();
        }
        catch (OperationCanceledException)
        {
            return null;
        }
    }

    private IObservable<List<Inventory>> InitialInventoriesLoadImpl()
    {
        return Observable.StartAsync(LoadInventories, RxApp.TaskpoolScheduler)
                .Do(inventoriesCache.AddOrUpdate);
    }

    private async Task<List<Inventory>> LoadInventories(CancellationToken cancellationToken)
    {
        try
        {
            var inventories = Enumerable.Range(1, 150)
                .Select(i=> 
                    new Inventory() 
                    { 
                        Name = i.ToString(), 
                        SomeProp = Math.Pow(i,i).ToString()
                    })
                .OrderBy(o=>o.RandomSortHelper)
                .ToList();
            return await Task.FromResult(inventories);
        }
        catch (OperationCanceledException)
        {
            return null;
        }
    }
}
