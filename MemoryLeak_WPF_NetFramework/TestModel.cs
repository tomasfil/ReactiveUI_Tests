using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryLeak_WPF_NetFramework
{
    public class TestModel : INotifyPropertyChanged
    {
        public TestModel()
        {
            Id = Guid.NewGuid();
            TestProp1 = "TestProp1";
            TestProp2 = "TestProp2";
            TestProp3 = "TestProp3";
            TestProp4 = "TestProp4";
            TestProp5 = "TestProp5";
            TestProp6 = "TestProp6";
        }

        public Guid Id { get; set; }

        public string TestProp1 { get; set; }

        public string TestProp2 { get; set; }

        public string TestProp3 { get; set; }

        public string TestProp4 { get; set; }

        public string TestProp5 { get; set; }

        public string TestProp6 { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
