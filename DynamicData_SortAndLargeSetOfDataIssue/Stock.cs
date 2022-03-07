using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace DynamicData_SortAndLargeSetOfDataIssue
{
    public class Stock : ReactiveObject
    {
        [Reactive]
        public string CaseNo { get; set; }
        [Reactive]
        public string Inventory { get; set; }
        [Reactive]
        public DateTime? ExpirationDate { get; set; }
        [Reactive]
        public string Item { get; set; }
    }
}