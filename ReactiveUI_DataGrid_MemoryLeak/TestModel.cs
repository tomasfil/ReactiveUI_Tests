using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI_DataGrid_MemoryLeak
{
    public class TestModel : ReactiveObject
    {
        public TestModel()
        {
            Id=Guid.NewGuid();
            TestProp1 = "TestProp1";
            TestProp2 = "TestProp2";
            TestProp3 = "TestProp3";
            TestProp4 = "TestProp4";
            TestProp5 = "TestProp5";
            TestProp6 = "TestProp6";
        }
        [Reactive]
        public Guid Id { get; set; }
        [Reactive]
        public string TestProp1 { get; set; }
        [Reactive]
        public string TestProp2 { get; set; }
        [Reactive]
        public string TestProp3 { get; set; }
        [Reactive]
        public string TestProp4 { get; set; }
        [Reactive]
        public string TestProp5 { get; set; }
        [Reactive]
        public string TestProp6 { get; set; }
    }
}
