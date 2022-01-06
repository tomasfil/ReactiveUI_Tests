using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI_MemoryLeakTest_Wpf.Models
{
    public class TestModel : ReactiveObject
    {
        public TestModel(int id, string name, TimeSpan duration)
        {
            Id = id;
            Name = name;
            Duration = duration;
        }

        [Reactive]
        public int Id { get; set; }
        [Reactive]
        public string Name { get; set; }
        [Reactive]
        public TimeSpan Duration { get; set; }
    }
}
