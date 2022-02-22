using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI_Akavache_Suspension.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        [DataMember]
        public int Integer  {get; set;}
        public MainViewModel()
        {
            Integer = 69;
        }
    }
}
