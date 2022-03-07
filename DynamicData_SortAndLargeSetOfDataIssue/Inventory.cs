using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DynamicData_SortAndLargeSetOfDataIssue
{
    internal class Inventory : ReactiveObject
    {
        [Reactive]
        public string Name { get; set; }
        [Reactive]
        public string SomeProp { get; set; }
    }
}