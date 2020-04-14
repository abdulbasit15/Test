using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BacktestCsv
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var reader = new StreamReader(@"StockPrices.csv"))
            {
                bool buyTrigger = false;
                bool sellTrigger = false;
                bool stocksBought = false;
                double buyVixPrice = 20;
                double sellVixPrice = 15;
                int records = -1;
                StockPrice sp = null;
                List<StockPrice> stockPrices = new List<StockPrice>();
                List<StockPrice> stocksTraded = new List<StockPrice>();
                while (!reader.EndOfStream)
                {
                    sp = new StockPrice();
                    records++;
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    sp.DateTime = values[0];
                    sp.VIX = values[1] == null ? 0.0 : Convert.ToDouble(values[1]);
                    sp.SPY = values[2] == null ? 0.0 : Convert.ToDouble(values[2]);
                    //sp.QQQ = values[2] == null ? 0.0 : Convert.ToDouble(values[2]);
                    //sp.TQQQ = values[3] == null ? 0.0 : Convert.ToDouble(values[3]);
                    //sp.AAPL = values[4] == null ? 0.0 : Convert.ToDouble(values[4]);
                    stockPrices.Add(sp);

                    if (buyTrigger)
                    {
                        if (sp.VIX > stockPrices[records - 1].VIX)
                            continue;
                        else
                        {
                            stocksTraded.Add(sp);
                            buyTrigger = false;
                            stocksBought = true;
                            using (StreamWriter sw = File.AppendText("StocksTraded.csv"))
                            {
                                sw.WriteLine($"{sp.DateTime}, {sp.VIX}, {sp.SPY}, {sp.QQQ}, {sp.TQQQ}, {sp.AAPL}");
                                sw.Flush();
                            }
                        }
                    }

                    if (sp.VIX > buyVixPrice && !stocksBought)
                    {
                        buyTrigger = true;
                    }

                    if (sellTrigger)
                    {
                        if(sp.VIX < stockPrices[records - 1].VIX)
                        continue;
                        else
                        {
                            stocksTraded.Add(sp);
                            sellTrigger = false;
                            stocksBought = false;
                            using (StreamWriter sw = File.AppendText("StocksTraded.csv"))
                            {
                                sw.WriteLine($"{sp.DateTime}, {sp.VIX},,,,,, {sp.SPY}, {sp.QQQ}, {sp.TQQQ}, {sp.AAPL}");
                                sw.Flush();
                            }
                        }
                    }
                    
                    if (stocksBought && sp.VIX < sellVixPrice)
                    {
                        sellTrigger = true;
                    }                    
                }
            }
        }
    }
}
