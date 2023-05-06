using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliOrdersExtract
{

    public class Order
    {
        public DateTime OrderDate { get; set; }
        public string OrderID { get; set; }
        public int Quantity { get; set; }
        public decimal TotalUSD { get; set; }
        public decimal TotalILS { get; set; }
    }

}
