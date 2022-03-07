using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicData_SortAndLargeSetOfDataIssue
{
    public class StockDataRow
    {
        public StockDataRow(Stock stock)
        {
            Guid = Guid.NewGuid();
            Stock = stock;
        }

        public Guid Guid { get; }
        public Stock Stock { get; }
    }
}
