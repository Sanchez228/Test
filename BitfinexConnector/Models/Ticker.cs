using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitfinexConnector.Models
{
    public class Ticker
    {
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public decimal LastPrice { get; set; }
    }
}
