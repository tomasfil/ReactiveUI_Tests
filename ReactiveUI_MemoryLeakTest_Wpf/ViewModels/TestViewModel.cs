using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI_MemoryLeakTest_Wpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI_MemoryLeakTest_Wpf.ViewModels
{
    public class TestViewModel : ReactiveObject
    {
        [Reactive]
        public TestModel TestModel { get; set; }

        public TestViewModel(TestModel testModel)
        {
            TestModel = testModel;
        }
    }
}
