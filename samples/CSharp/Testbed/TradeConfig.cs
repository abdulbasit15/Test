using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testbed
{
    public class TradeConfig
    {
        public bool Historical { get; set; }
        public bool Realtime { get; set; }
        public bool AfterRTH { get; set; }
        public double CallBuyRsi { get; set; }
        public double CallSellRsi { get; set; }
        public double PutBuyRsi { get; set; }
        public double PutSellRsi { get; set; }
        public List<StockConfig> Stocks { get; set; }
    }

    public class StockConfig
    {
        //	{ "id":1001, "stock": "AAPL", "price": 61, "callStrike": 288, "putStrike":286, "expirationDate":20190510 },
        public int Id { get; set; }
        public string Symbol { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }
        public double Capital { get; set; }
        public double CallStrike { get; set; }
        public double PutStrike { get; set; }
        public string Expiration { get; set; }
    }
}
