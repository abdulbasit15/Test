using System;
using System.Collections.Generic;
using System.Text;

namespace BacktestCsv
{
    public class StockPrice
    {
        public string DateTime { get; set; }
        public double VIX { get; set; }
        public double SPY { get; set; }
        public double QQQ { get; set; }
        public double TQQQ { get; set; }
        public double AAPL { get; set; }
    }
}
