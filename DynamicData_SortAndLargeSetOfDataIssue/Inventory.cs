using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace DynamicData_SortAndLargeSetOfDataIssue
{
    internal class Inventory : ReactiveObject
    {
        [Reactive]
        public string Name { get; set; }
        [Reactive]
        public string SomeProp { get; set; }
        public Guid RandomSortHelper =Guid.NewGuid();
    }
}