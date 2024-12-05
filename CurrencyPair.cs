using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchangeSystem
{
    public class CurrencyPair
    {
        public string PairName { get; set; }
        public decimal ExchangeRate { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
