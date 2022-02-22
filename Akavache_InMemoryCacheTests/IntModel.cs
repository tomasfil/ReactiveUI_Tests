using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akavache_InMemoryCacheTests
{
    internal class IntModel : ReactiveObject
    {
        [Reactive]
        public string IntString { get; set; }

        public IntModel(string intString)
        {
            IntString = intString;
        }

        public static string GetCacheIdentifier(object parameter)
        {
            return $"{nameof(IntModel)}_{parameter}";
        }
    }
}
