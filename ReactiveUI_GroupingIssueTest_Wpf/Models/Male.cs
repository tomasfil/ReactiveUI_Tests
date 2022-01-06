using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI_GroupingIssueTest_Wpf.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI_GroupingIssueTest_Wpf.Models
{
    public class Male : ReactiveObject
    {
        public Male(int id, Greek favouriteGreekLetter)
        {
            Id = id;
            FavouriteGreekLetter = favouriteGreekLetter;
        }

        [Reactive]
        public int Id { get; set; }
        [Reactive]
        public Greek FavouriteGreekLetter { get; set; }
    }
}
