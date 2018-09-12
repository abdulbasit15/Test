/* Copyright (C) 2017 Interactive Brokers LLC. All rights reserved.  This code is subject to the terms
 * and conditions of the IB API Non-Commercial License or the IB API Commercial License, as applicable. */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBApi;
using System.IO;
using System.Globalization;

namespace Samples
{
    //! [ewrapperimpl]
    public class EWrapperImpl : EWrapper
    {
        //! [ewrapperimpl]
        private int nextOrderId;
        //! [socket_declare]
        EClientSocket clientSocket;
        public readonly EReaderSignal Signal;
        //! [socket_declare]

        //! [socket_init]
        public EWrapperImpl()
        {
            Signal = new EReaderMonitorSignal();
            clientSocket = new EClientSocket(this, Signal);
        }
        //! [socket_init]

        public EClientSocket ClientSocket
        {
            get { return clientSocket; }
            set { clientSocket = value; }
        }

        public int NextOrderId
        {
            get { return nextOrderId; }
            set { nextOrderId = value; }
        }

        public string BboExchange { get; private set; }

        public virtual void error(Exception e)
        {
            Console.WriteLine("Exception thrown: " + e);
            throw e;
        }

        public virtual void error(string str)
        {
            Console.WriteLine("Error: " + str + "\n");
        }

        //! [error]
        public virtual void error(int id, int errorCode, string errorMsg)
        {
            Console.WriteLine("Error. Id: " + id + ", Code: " + errorCode + ", Msg: " + errorMsg + "\n");
        }
        //! [error]

        public virtual void connectionClosed()
        {
            Console.WriteLine("Connection closed.\n");
        }

        public virtual void currentTime(long time)
        {
            Console.WriteLine("Current Time: " + time + "\n");
        }

        //! [tickprice]
        public virtual void tickPrice(int tickerId, int field, double price, TickAttrib attribs)
        {
            Console.WriteLine("Tick Price. Ticker Id:" + tickerId + ", Field: " + field + ", Price: " + price + ", CanAutoExecute: " + attribs.CanAutoExecute +
                ", PastLimit: " + attribs.PastLimit + ", PreOpen: " + attribs.PreOpen);
        }
        //! [tickprice]

        //! [ticksize]
        public virtual void tickSize(int tickerId, int field, int size)
        {
            Console.WriteLine("Tick Size. Ticker Id:" + tickerId + ", Field: " + field + ", Size: " + size);
        }
        //! [ticksize]

        //! [tickstring]
        public virtual void tickString(int tickerId, int tickType, string value)
        {
            Console.WriteLine("Tick string. Ticker Id:" + tickerId + ", Type: " + tickType + ", Value: " + value);
        }
        //! [tickstring]

        //! [tickgeneric]
        public virtual void tickGeneric(int tickerId, int field, double value)
        {
            Console.WriteLine("Tick Generic. Ticker Id:" + tickerId + ", Field: " + field + ", Value: " + value);
        }
        //! [tickgeneric]

        public virtual void tickEFP(int tickerId, int tickType, double basisPoints, string formattedBasisPoints, double impliedFuture, int holdDays, string futureLastTradeDate, double dividendImpact, double dividendsToLastTradeDate)
        {
            Console.WriteLine("TickEFP. " + tickerId + ", Type: " + tickType + ", BasisPoints: " + basisPoints + ", FormattedBasisPoints: " + formattedBasisPoints + ", ImpliedFuture: " + impliedFuture + ", HoldDays: " + holdDays + ", FutureLastTradeDate: " + futureLastTradeDate + ", DividendImpact: " + dividendImpact + ", DividendsToLastTradeDate: " + dividendsToLastTradeDate);
        }

        //! [ticksnapshotend]
        public virtual void tickSnapshotEnd(int tickerId)
        {
            Console.WriteLine("TickSnapshotEnd: " + tickerId);
        }
        //! [ticksnapshotend]

        //! [nextvalidid]
        public virtual void nextValidId(int orderId)
        {
            Console.WriteLine("Next Valid Id: " + orderId);
            NextOrderId = orderId;
        }
        //! [nextvalidid]

        public virtual void deltaNeutralValidation(int reqId, UnderComp underComp)
        {
            Console.WriteLine("DeltaNeutralValidation. " + reqId + ", ConId: " + underComp.ConId + ", Delta: " + underComp.Delta + ", Price: " + underComp.Price);
        }

        //! [managedaccounts]
        public virtual void managedAccounts(string accountsList)
        {
            Console.WriteLine("Account list: " + accountsList);
        }
        //! [managedaccounts]

        //! [tickoptioncomputation]
        public virtual void tickOptionComputation(int tickerId, int field, double impliedVolatility, double delta, double optPrice, double pvDividend, double gamma, double vega, double theta, double undPrice)
        {
            Console.WriteLine("TickOptionComputation. TickerId: " + tickerId + ", field: " + field + ", ImpliedVolatility: " + impliedVolatility + ", Delta: " + delta
                + ", OptionPrice: " + optPrice + ", pvDividend: " + pvDividend + ", Gamma: " + gamma + ", Vega: " + vega + ", Theta: " + theta + ", UnderlyingPrice: " + undPrice);
        }
        //! [tickoptioncomputation]

        //! [accountsummary]
        public virtual void accountSummary(int reqId, string account, string tag, string value, string currency)
        {
            Console.WriteLine("Acct Summary. ReqId: " + reqId + ", Acct: " + account + ", Tag: " + tag + ", Value: " + value + ", Currency: " + currency);
        }
        //! [accountsummary]

        //! [accountsummaryend]
        public virtual void accountSummaryEnd(int reqId)
        {
            Console.WriteLine("AccountSummaryEnd. Req Id: " + reqId + "\n");
        }
        //! [accountsummaryend]

        //! [updateaccountvalue]
        public virtual void updateAccountValue(string key, string value, string currency, string accountName)
        {
            Console.WriteLine("UpdateAccountValue. Key: " + key + ", Value: " + value + ", Currency: " + currency + ", AccountName: " + accountName);
        }
        //! [updateaccountvalue]

        //! [updateportfolio]
        public virtual void updatePortfolio(Contract contract, double position, double marketPrice, double marketValue, double averageCost, double unrealizedPNL, double realizedPNL, string accountName)
        {
            Console.WriteLine("UpdatePortfolio. " + contract.Symbol + ", " + contract.SecType + " @ " + contract.Exchange
                + ": Position: " + position + ", MarketPrice: " + marketPrice + ", MarketValue: " + marketValue + ", AverageCost: " + averageCost
                + ", UnrealizedPNL: " + unrealizedPNL + ", RealizedPNL: " + realizedPNL + ", AccountName: " + accountName);
        }
        //! [updateportfolio]

        //! [updateaccounttime]
        public virtual void updateAccountTime(string timestamp)
        {
            Console.WriteLine("UpdateAccountTime. Time: " + timestamp + "\n");
        }
        //! [updateaccounttime]

        //! [accountdownloadend]
        public virtual void accountDownloadEnd(string account)
        {
            Console.WriteLine("Account download finished: " + account + "\n");
        }
        //! [accountdownloadend]

        //! [orderstatus]
        public virtual void orderStatus(int orderId, string status, double filled, double remaining, double avgFillPrice, int permId, int parentId, double lastFillPrice, int clientId, string whyHeld, double mktCapPrice)
        {
            Console.WriteLine("OrderStatus. Id: " + orderId + ", Status: " + status + ", Filled" + filled + ", Remaining: " + remaining
                + ", AvgFillPrice: " + avgFillPrice + ", PermId: " + permId + ", ParentId: " + parentId + ", LastFillPrice: " + lastFillPrice + ", ClientId: " + clientId + ", WhyHeld: " + whyHeld + ", MktCapPrice: " + mktCapPrice);
        }
        //! [orderstatus]

        //! [openorder]
        public virtual void openOrder(int orderId, Contract contract, Order order, OrderState orderState)
        {
            Console.WriteLine("OpenOrder. ID: " + orderId + ", " + contract.Symbol + ", " + contract.SecType + " @ " + contract.Exchange + ": " + order.Action + ", " + order.OrderType + " " + order.TotalQuantity + ", " + orderState.Status);
        }
        //! [openorder]

        //! [openorderend]
        public virtual void openOrderEnd()
        {
            Console.WriteLine("OpenOrderEnd");
        }
        //! [openorderend]

        //! [contractdetails]
        public virtual void contractDetails(int reqId, ContractDetails contractDetails)
        {
            Console.WriteLine("ContractDetails begin. ReqId: " + reqId);
            printContractMsg(contractDetails.Summary);
            printContractDetailsMsg(contractDetails);
            Console.WriteLine("ContractDetails end. ReqId: " + reqId);
        }
        //! [contractdetails]

        public void printContractMsg(Contract contract)
        {
            Console.WriteLine("\tConId: " + contract.ConId);
            Console.WriteLine("\tSymbol: " + contract.Symbol);
            Console.WriteLine("\tSecType: " + contract.SecType);
            Console.WriteLine("\tLastTradeDateOrContractMonth: " + contract.LastTradeDateOrContractMonth);
            Console.WriteLine("\tStrike: " + contract.Strike);
            Console.WriteLine("\tRight: " + contract.Right);
            Console.WriteLine("\tMultiplier: " + contract.Multiplier);
            Console.WriteLine("\tExchange: " + contract.Exchange);
            Console.WriteLine("\tPrimaryExchange: " + contract.PrimaryExch);
            Console.WriteLine("\tCurrency: " + contract.Currency);
            Console.WriteLine("\tLocalSymbol: " + contract.LocalSymbol);
            Console.WriteLine("\tTradingClass: " + contract.TradingClass);
        }

        public void printContractDetailsMsg(ContractDetails contractDetails)
        {
            Console.WriteLine("\tMarketName: " + contractDetails.MarketName);
            Console.WriteLine("\tMinTick: " + contractDetails.MinTick);
            Console.WriteLine("\tPriceMagnifier: " + contractDetails.PriceMagnifier);
            Console.WriteLine("\tOrderTypes: " + contractDetails.OrderTypes);
            Console.WriteLine("\tValidExchanges: " + contractDetails.ValidExchanges);
            Console.WriteLine("\tUnderConId: " + contractDetails.UnderConId);
            Console.WriteLine("\tLongName: " + contractDetails.LongName);
            Console.WriteLine("\tContractMonth: " + contractDetails.ContractMonth);
            Console.WriteLine("\tIndystry: " + contractDetails.Industry);
            Console.WriteLine("\tCategory: " + contractDetails.Category);
            Console.WriteLine("\tSubCategory: " + contractDetails.Subcategory);
            Console.WriteLine("\tTimeZoneId: " + contractDetails.TimeZoneId);
            Console.WriteLine("\tTradingHours: " + contractDetails.TradingHours);
            Console.WriteLine("\tLiquidHours: " + contractDetails.LiquidHours);
            Console.WriteLine("\tEvRule: " + contractDetails.EvRule);
            Console.WriteLine("\tEvMultiplier: " + contractDetails.EvMultiplier);
            Console.WriteLine("\tMdSizeMultiplier: " + contractDetails.MdSizeMultiplier);
            Console.WriteLine("\tAggGroup: " + contractDetails.AggGroup);
            Console.WriteLine("\tUnderSymbol: " + contractDetails.UnderSymbol);
            Console.WriteLine("\tUnderSecType: " + contractDetails.UnderSecType);
            Console.WriteLine("\tMarketRuleIds: " + contractDetails.MarketRuleIds);
            Console.WriteLine("\tRealExpirationDate: " + contractDetails.RealExpirationDate);
            printContractDetailsSecIdList(contractDetails.SecIdList);
        }

        public void printContractDetailsSecIdList(List<TagValue> secIdList)
        {
            if (secIdList != null && secIdList.Count > 0)
            {
                Console.Write("\tSecIdList: {");
                foreach (TagValue tagValue in secIdList)
                {
                    Console.Write(tagValue.Tag + "=" + tagValue.Value + ";");
                }
                Console.WriteLine("}");
            }
        }

        public void printBondContractDetailsMsg(ContractDetails contractDetails)
        {
            Console.WriteLine("\tSymbol: " + contractDetails.Summary.Symbol);
            Console.WriteLine("\tSecType: " + contractDetails.Summary.SecType);
            Console.WriteLine("\tCusip: " + contractDetails.Cusip);
            Console.WriteLine("\tCoupon: " + contractDetails.Coupon);
            Console.WriteLine("\tMaturity: " + contractDetails.Maturity);
            Console.WriteLine("\tIssueDate: " + contractDetails.IssueDate);
            Console.WriteLine("\tRatings: " + contractDetails.Ratings);
            Console.WriteLine("\tBondType: " + contractDetails.BondType);
            Console.WriteLine("\tCouponType: " + contractDetails.CouponType);
            Console.WriteLine("\tConvertible: " + contractDetails.Convertible);
            Console.WriteLine("\tCallable: " + contractDetails.Callable);
            Console.WriteLine("\tPutable: " + contractDetails.Putable);
            Console.WriteLine("\tDescAppend: " + contractDetails.DescAppend);
            Console.WriteLine("\tExchange: " + contractDetails.Summary.Exchange);
            Console.WriteLine("\tCurrency: " + contractDetails.Summary.Currency);
            Console.WriteLine("\tMarketName: " + contractDetails.MarketName);
            Console.WriteLine("\tTradingClass: " + contractDetails.Summary.TradingClass);
            Console.WriteLine("\tConId: " + contractDetails.Summary.ConId);
            Console.WriteLine("\tMinTick: " + contractDetails.MinTick);
            Console.WriteLine("\tMdSizeMultiplier: " + contractDetails.MdSizeMultiplier);
            Console.WriteLine("\tOrderTypes: " + contractDetails.OrderTypes);
            Console.WriteLine("\tValidExchanges: " + contractDetails.ValidExchanges);
            Console.WriteLine("\tNextOptionDate: " + contractDetails.NextOptionDate);
            Console.WriteLine("\tNextOptionType: " + contractDetails.NextOptionType);
            Console.WriteLine("\tNextOptionPartial: " + contractDetails.NextOptionPartial);
            Console.WriteLine("\tNotes: " + contractDetails.Notes);
            Console.WriteLine("\tLong Name: " + contractDetails.LongName);
            Console.WriteLine("\tEvRule: " + contractDetails.EvRule);
            Console.WriteLine("\tEvMultiplier: " + contractDetails.EvMultiplier);
            Console.WriteLine("\tAggGroup: " + contractDetails.AggGroup);
            Console.WriteLine("\tMarketRuleIds: " + contractDetails.MarketRuleIds);
            printContractDetailsSecIdList(contractDetails.SecIdList);
        }


        //! [contractdetailsend]
        public virtual void contractDetailsEnd(int reqId)
        {
            Console.WriteLine("ContractDetailsEnd. " + reqId + "\n");
        }
        //! [contractdetailsend]

        //! [execdetails]
        public virtual void execDetails(int reqId, Contract contract, Execution execution)
        {
            Console.WriteLine("ExecDetails. " + reqId + " - " + contract.Symbol + ", " + contract.SecType + ", " + contract.Currency + " - " + execution.ExecId + ", " + execution.OrderId + ", " + execution.Shares + ", " + execution.LastLiquidity);
        }
        //! [execdetails]

        //! [execdetailsend]
        public virtual void execDetailsEnd(int reqId)
        {
            Console.WriteLine("ExecDetailsEnd. " + reqId + "\n");
        }
        //! [execdetailsend]

        //! [commissionreport]
        public virtual void commissionReport(CommissionReport commissionReport)
        {
            Console.WriteLine("CommissionReport. " + commissionReport.ExecId + " - " + commissionReport.Commission + " " + commissionReport.Currency + " RPNL " + commissionReport.RealizedPNL);
        }
        //! [commissionreport]

        //! [fundamentaldata]
        public virtual void fundamentalData(int reqId, string data)
        {
            Console.WriteLine("FundamentalData. " + reqId + "" + data + "\n");
        }
        //! [fundamentaldata]

        //! [marketdatatype]
        public virtual void marketDataType(int reqId, int marketDataType)
        {
            Console.WriteLine("MarketDataType. " + reqId + ", Type: " + marketDataType + "\n");
        }
        //! [marketdatatype]

        //! [updatemktdepth]
        public virtual void updateMktDepth(int tickerId, int position, int operation, int side, double price, int size)
        {
            Console.WriteLine("UpdateMarketDepth. " + tickerId + " - Position: " + position + ", Operation: " + operation + ", Side: " + side + ", Price: " + price + ", Size" + size);
        }
        //! [updatemktdepth]

        //! [updatemktdepthl2]
        public virtual void updateMktDepthL2(int tickerId, int position, string marketMaker, int operation, int side, double price, int size)
        {
            Console.WriteLine("UpdateMarketDepthL2. " + tickerId + " - Position: " + position + ", Operation: " + operation + ", Side: " + side + ", Price: " + price + ", Size" + size);
        }
        //! [updatemktdepthl2]

        //! [updatenewsbulletin]
        public virtual void updateNewsBulletin(int msgId, int msgType, String message, String origExchange)
        {
            Console.WriteLine("News Bulletins. " + msgId + " - Type: " + msgType + ", Message: " + message + ", Exchange of Origin: " + origExchange + "\n");
        }
        //! [updatenewsbulletin]

        //! [positionend]
        public virtual void positionEnd()
        {
            Console.WriteLine("PositionEnd \n");
        }
        //! [positionend]

        //! [realtimebar]

        //! [scannerparameters]
        public virtual void scannerParameters(string xml)
        {
            Console.WriteLine("ScannerParameters. " + xml + "\n");
        }
        //! [scannerparameters]

        //! [scannerdata]
        public virtual void scannerData(int reqId, int rank, ContractDetails contractDetails, string distance, string benchmark, string projection, string legsStr)
        {
            Console.WriteLine("ScannerData. " + reqId + " - Rank: " + rank + ", Symbol: " + contractDetails.Summary.Symbol + ", SecType: " + contractDetails.Summary.SecType + ", Currency: " + contractDetails.Summary.Currency
                + ", Distance: " + distance + ", Benchmark: " + benchmark + ", Projection: " + projection + ", Legs String: " + legsStr);
        }
        //! [scannerdata]

        //! [scannerdataend]
        public virtual void scannerDataEnd(int reqId)
        {
            Console.WriteLine("ScannerDataEnd. " + reqId);
        }
        //! [scannerdataend]

        //! [receivefa]
        public virtual void receiveFA(int faDataType, string faXmlData)
        {
            Console.WriteLine("Receing FA: " + faDataType + " - " + faXmlData);
        }
        //! [receivefa]

        public virtual void bondContractDetails(int requestId, ContractDetails contractDetails)
        {
            Console.WriteLine("BondContractDetails begin. ReqId: " + requestId);
            printBondContractDetailsMsg(contractDetails);
            Console.WriteLine("BondContractDetails end. ReqId: " + requestId);
        }

        //! [historicaldataend]
        public virtual void historicalDataEnd(int reqId, string startDate, string endDate)
        {
            Console.WriteLine("HistoricalDataEnd - " + reqId + " from " + startDate + " to " + endDate);
        }
        //! [historicaldataend]

        public virtual void verifyMessageAPI(string apiData)
        {
            Console.WriteLine("verifyMessageAPI: " + apiData);
        }
        public virtual void verifyCompleted(bool isSuccessful, string errorText)
        {
            Console.WriteLine("verifyCompleted. IsSuccessfule: " + isSuccessful + " - Error: " + errorText);
        }
        public virtual void verifyAndAuthMessageAPI(string apiData, string xyzChallenge)
        {
            Console.WriteLine("verifyAndAuthMessageAPI: " + apiData + " " + xyzChallenge);
        }
        public virtual void verifyAndAuthCompleted(bool isSuccessful, string errorText)
        {
            Console.WriteLine("verifyAndAuthCompleted. IsSuccessful: " + isSuccessful + " - Error: " + errorText);
        }
        //! [displaygrouplist]
        public virtual void displayGroupList(int reqId, string groups)
        {
            Console.WriteLine("DisplayGroupList. Request: " + reqId + ", Groups" + groups);
        }
        //! [displaygrouplist]

        //! [displaygroupupdated]
        public virtual void displayGroupUpdated(int reqId, string contractInfo)
        {
            Console.WriteLine("displayGroupUpdated. Request: " + reqId + ", ContractInfo: " + contractInfo);
        }
        //! [displaygroupupdated]

        //! [positionmulti]
        public virtual void positionMulti(int reqId, string account, string modelCode, Contract contract, double pos, double avgCost)
        {
            Console.WriteLine("Position Multi. Request: " + reqId + ", Account: " + account + ", ModelCode: " + modelCode + ", Symbol: " + contract.Symbol + ", SecType: " + contract.SecType + ", Currency: " + contract.Currency + ", Position: " + pos + ", Avg cost: " + avgCost + "\n");
        }
        //! [positionmulti]

        //! [positionmultiend]
        public virtual void positionMultiEnd(int reqId)
        {
            Console.WriteLine("Position Multi End. Request: " + reqId + "\n");
        }
        //! [positionmultiend]

        //! [accountupdatemulti]
        public virtual void accountUpdateMulti(int reqId, string account, string modelCode, string key, string value, string currency)
        {
            Console.WriteLine("Account Update Multi. Request: " + reqId + ", Account: " + account + ", ModelCode: " + modelCode + ", Key: " + key + ", Value: " + value + ", Currency: " + currency + "\n");
        }
        //! [accountupdatemulti]

        //! [accountupdatemultiend]
        public virtual void accountUpdateMultiEnd(int reqId)
        {
            Console.WriteLine("Account Update Multi End. Request: " + reqId + "\n");
        }
        //! [accountupdatemultiend]

        //! [securityDefinitionOptionParameter]
        public void securityDefinitionOptionParameter(int reqId, string exchange, int underlyingConId, string tradingClass, string multiplier, HashSet<string> expirations, HashSet<double> strikes)
        {
            Console.WriteLine("Security Definition Option Parameter. Reqest: {0}, Exchange: {1}, Undrelying contract id: {2}, Trading class: {3}, Multiplier: {4}, Expirations: {5}, Strikes: {6}",
                              reqId, exchange, underlyingConId, tradingClass, multiplier, string.Join(", ", expirations), string.Join(", ", strikes));
        }
        //! [securityDefinitionOptionParameter]

        //! [securityDefinitionOptionParameterEnd]
        public void securityDefinitionOptionParameterEnd(int reqId)
        {
            Console.WriteLine("Security Definition Option Parameter End. Request: " + reqId + "\n");
        }
        //! [securityDefinitionOptionParameterEnd]

        //! [connectack]
        public void connectAck()
        {
            if (ClientSocket.AsyncEConnect)
                ClientSocket.startApi();
        }
        //! [connectack]

        //! [softDollarTiers]
        public void softDollarTiers(int reqId, SoftDollarTier[] tiers)
        {
            Console.WriteLine("Soft Dollar Tiers:");

            foreach (var tier in tiers)
            {
                Console.WriteLine(tier.DisplayName);
            }
        }
        //! [softDollarTiers]

        //! [familyCodes]
        public void familyCodes(FamilyCode[] familyCodes)
        {
            Console.WriteLine("Family Codes:");

            foreach (var familyCode in familyCodes)
            {
                Console.WriteLine("Account ID: {0}, Family Code Str: {1}", familyCode.AccountID, familyCode.FamilyCodeStr);
            }
        }
        //! [familyCodes]

        //! [symbolSamples]
        public void symbolSamples(int reqId, ContractDescription[] contractDescriptions)
        {
            string derivSecTypes;
            Console.WriteLine("Symbol Samples. Request Id: {0}", reqId);

            foreach (var contractDescription in contractDescriptions)
            {
                derivSecTypes = "";
                foreach (var derivSecType in contractDescription.DerivativeSecTypes)
                {
                    derivSecTypes += derivSecType;
                    derivSecTypes += " ";
                }
                Console.WriteLine("Contract: conId - {0}, symbol - {1}, secType - {2}, primExchange - {3}, currency - {4}, derivativeSecTypes - {5}",
                    contractDescription.Contract.ConId, contractDescription.Contract.Symbol, contractDescription.Contract.SecType,
                    contractDescription.Contract.PrimaryExch, contractDescription.Contract.Currency, derivSecTypes);
            }
        }
        //! [symbolSamples]

        //! [mktDepthExchanges]
        public void mktDepthExchanges(DepthMktDataDescription[] depthMktDataDescriptions)
        {
            Console.WriteLine("Market Depth Exchanges:");

            foreach (var depthMktDataDescription in depthMktDataDescriptions)
            {
                Console.WriteLine("Depth Market Data Description: Exchange: {0}, Security Type: {1}, Listing Exch: {2}, Service Data Type: {3}, Agg Group: {4}",
                    depthMktDataDescription.Exchange, depthMktDataDescription.SecType,
                    depthMktDataDescription.ListingExch, depthMktDataDescription.ServiceDataType,
                    depthMktDataDescription.AggGroup != Int32.MaxValue ? depthMktDataDescription.AggGroup.ToString() : ""
                    );
            }
        }
        //! [mktDepthExchanges]

        //! [tickNews]
        public void tickNews(int tickerId, long timeStamp, string providerCode, string articleId, string headline, string extraData)
        {
            Console.WriteLine("Tick News. Ticker Id: {0}, Time Stamp: {1}, Provider Code: {2}, Article Id: {3}, headline: {4}, extraData: {5}", tickerId, timeStamp, providerCode, articleId, headline, extraData);
        }
        //! [tickNews]

        //! [smartcomponents]
        public void smartComponents(int reqId, Dictionary<int, KeyValuePair<string, char>> theMap)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("==== Smart Components Begin (total={0}) reqId = {1} ====\n", theMap.Count, reqId);

            foreach (var item in theMap)
            {
                sb.AppendFormat("bit number: {0}, exchange: {1}, exchange letter: {2}\n", item.Key, item.Value.Key, item.Value.Value);
            }

            sb.AppendFormat("==== Smart Components Begin (total={0}) reqId = {1} ====\n", theMap.Count, reqId);

            Console.WriteLine(sb);
        }
        //! [smartcomponents]

        //! [tickReqParams]
        public void tickReqParams(int tickerId, double minTick, string bboExchange, int snapshotPermissions)
        {
            Console.WriteLine("id={0} minTick = {1} bboExchange = {2} snapshotPermissions = {3}", tickerId, minTick, bboExchange, snapshotPermissions);

            BboExchange = bboExchange;
        }
        //! [tickReqParams]

        //! [newsProviders]
        public void newsProviders(NewsProvider[] newsProviders)
        {
            Console.WriteLine("News Providers:");

            foreach (var newsProvider in newsProviders)
            {
                Console.WriteLine("News provider: providerCode - {0}, providerName - {1}",
                    newsProvider.ProviderCode, newsProvider.ProviderName);
            }
        }
        //! [newsProviders]

        //! [newsArticle]
        public void newsArticle(int requestId, int articleType, string articleText)
        {
            Console.WriteLine("News Article. Request Id: {0}, ArticleType: {1}", requestId, articleType);
            if (articleType == 0)
            {
                Console.WriteLine("News Article Text: {0}", articleText);
            }
            else if (articleType == 1)
            {
                Console.WriteLine("News Article Text: article text is binary/pdf and cannot be displayed");
            }
        }
        //! [newsArticle]

        //! [historicalNews]
        public void historicalNews(int requestId, string time, string providerCode, string articleId, string headline)
        {
            Console.WriteLine("Historical News. Request Id: {0}, Time: {1}, Provider Code: {2}, Article Id: {3}, headline: {4}", requestId, time, providerCode, articleId, headline);
        }
        //! [historicalNews]

        //! [historicalNewsEnd]
        public void historicalNewsEnd(int requestId, bool hasMore)
        {
            Console.WriteLine("Historical News End. Request Id: {0}, Has More: {1}", requestId, hasMore);
        }
        //! [historicalNewsEnd]

        //! [headTimestamp]
        public void headTimestamp(int reqId, string headTimestamp)
        {
            Console.WriteLine("Head time stamp. Request Id: {0}, Head time stamp: {1}", reqId, headTimestamp);
        }
        //! [headTimestamp]

        //! [histogramData]
        public void histogramData(int reqId, HistogramEntry[] data)
        {
            Console.WriteLine("Histogram data. Request Id: {0}, data size: {1}", reqId, data.Length);
            data.ToList().ForEach(i => Console.WriteLine("\tPrice: {0}, Size: {1}", i.Price, i.Size));
        }
        //! [histogramData]

        //! [historicalDataUpdate]
        public void historicalDataUpdate(int reqId, Bar bar)
        {
            Console.WriteLine("HistoricalDataUpdate. " + reqId + " - Time: " + bar.Time + ", Open: " + bar.Open + ", High: " + bar.High + ", Low: " + bar.Low + ", Close: " + bar.Close + ", Volume: " + bar.Volume + ", Count: " + bar.Count + ", WAP: " + bar.WAP);
        }
        //! [historicalDataUpdate]

        //! [rerouteMktDataReq]
        public void rerouteMktDataReq(int reqId, int conId, string exchange)
        {
            Console.WriteLine("Re-route market data request. Req Id: {0}, ConId: {1}, Exchange: {2}", reqId, conId, exchange);
        }
        //! [rerouteMktDataReq]

        //! [rerouteMktDepthReq]
        public void rerouteMktDepthReq(int reqId, int conId, string exchange)
        {
            Console.WriteLine("Re-route market depth request. Req Id: {0}, ConId: {1}, Exchange: {2}", reqId, conId, exchange);
        }
        //! [rerouteMktDepthReq]

        //! [marketRule]
        public void marketRule(int marketRuleId, PriceIncrement[] priceIncrements)
        {
            Console.WriteLine("Market Rule Id: " + marketRuleId);
            foreach (var priceIncrement in priceIncrements)
            {
                Console.WriteLine("Low Edge: {0}, Increment: {1}", ((decimal)priceIncrement.LowEdge).ToString(), ((decimal)priceIncrement.Increment).ToString());
            }
        }
        //! [marketRule]

        //! [pnl]
        public void pnl(int reqId, double dailyPnL, double unrealizedPnL, double realizedPnL)
        {
            Console.WriteLine("PnL. Request Id: {0}, Daily PnL: {1}, Unrealized PnL: {2}, Realized PnL: {3}", reqId, dailyPnL, unrealizedPnL, realizedPnL);
        }
        //! [pnl]

        //! [pnlsingle]
        public void pnlSingle(int reqId, int pos, double dailyPnL, double unrealizedPnL, double realizedPnL, double value)
        {
            Console.WriteLine("PnL Single. Request Id: {0}, Pos {1}, Daily PnL {2}, Unrealized PnL {3}, Realized PnL: {4}, Value: {5}", reqId, pos, dailyPnL, unrealizedPnL, realizedPnL, value);
        }
        //! [pnlsingle]

        //! [historicalticks]
        public void historicalTicks(int reqId, HistoricalTick[] ticks, bool done)
        {
            foreach (var tick in ticks)
            {
                Console.WriteLine("Historical Tick. Request Id: {0}, Time: {1}, Price: {2}, Size: {3}", reqId, tick.Time, tick.Price, tick.Size);
            }
        }
        //! [historicalticks]

        //! [historicalticksbidask]
        public void historicalTicksBidAsk(int reqId, HistoricalTickBidAsk[] ticks, bool done)
        {
            foreach (var tick in ticks)
            {
                Console.WriteLine("Historical Tick Bid/Ask. Request Id: {0}, Time: {1}, Mask: {2} Price Bid: {3}, Price Ask {4}, Size Bid: {5}, Size Ask {6}",
                    reqId, tick.Time, tick.Mask, tick.PriceBid, tick.PriceAsk, tick.SizeBid, tick.SizeAsk);
            }
        }
        //! [historicalticksbidask]

        //! [historicaltickslast]
        public void historicalTicksLast(int reqId, HistoricalTickLast[] ticks, bool done)
        {
            foreach (var tick in ticks)
            {
                Console.WriteLine("Historical Tick Last. Request Id: {0}, Time: {1}, Mask: {2}, Price: {3}, Size: {4}, Exchange: {5}, Special Conditions: {6}",
                    reqId, tick.Time, tick.Mask, tick.Price, tick.Size, tick.Exchange, tick.SpecialConditions);
            }
        }
        //! [historicaltickslast]

        //! [tickbytickalllast]
        public void tickByTickAllLast(int reqId, int tickType, long time, double price, int size, TickAttrib attribs, string exchange, string specialConditions)
        {
            string tickTypeStr = tickType == 1 ? "Last" : "AllLast";
            string timeStr = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(Convert.ToDouble(time)).ToString("yyyyMMdd-HH:mm:ss zzz");

            Console.WriteLine("Tick-By-Tick. Request Id: {0}, TickType: {1}, Time: {2}, Price: {3}, Size: {4}, Exchange: {5}, Special Conditions: {6}, PastLimit: {7}, Unreported: {8}",
                reqId, tickTypeStr, timeStr, price, size, exchange, specialConditions, attribs.PastLimit, attribs.Unreported);
        }
        //! [tickbytickalllast]

        //! [tickbytickbidask]
        public void tickByTickBidAsk(int reqId, long time, double bidPrice, double askPrice, int bidSize, int askSize, TickAttrib attribs)
        {
            string timeStr = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(Convert.ToDouble(time)).ToString("yyyyMMdd-HH:mm:ss zzz");
            Console.WriteLine("Tick-By-Tick. Request Id: {0}, TickType: BidAsk, Time: {1}, BidPrice: {2}, AskPrice: {3}, BidSize: {4}, AskSize: {5}, BidPastLow: {6}, AskPastHigh: {7}",
                reqId, timeStr, bidPrice, askPrice, bidSize, askSize, attribs.BidPastLow, attribs.AskPastHigh);
        }
        //! [tickbytickbidask]

        //! [tickbytickmidpoint]
        public void tickByTickMidPoint(int reqId, long time, double midPoint)
        {
            string timeStr = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(Convert.ToDouble(time)).ToString("yyyyMMdd-HH:mm:ss zzz");
            Console.WriteLine("Tick-By-Tick. Request Id: {0}, TickType: MidPoint, Time: {1}, MidPoint: {2}",
                reqId, timeStr, midPoint);
        }
        //! [tickbytickmidpoint]

        //! [historicaldata]
        public virtual void historicalData(int reqId, Bar bar)
        {
            //SingleBuy(reqId, bar);
            if (reqId == 1000)
            {
                HourlyData(reqId, bar);
                var text = File.ReadAllText("hourly-data.csv");
                string[] splits = text.Split(',');
                var date = Convert.ToDateTime(splits[0]);
                var stockValue = splits[1];
                var value930 = splits[2];
                var value10 = splits[3];
                var value11 = splits[4];
                var value12 = splits[5];
                var value13 = splits[6];
                var value14 = splits[7];
                var value15 = splits[8];
                var dataReadComplete = splits[9];
                if (dataReadComplete == "true")
                {
                    SetUpContract(reqId, bar, date, stockValue, value930, value10, value11, value12, value13, value14, value15);
                }
            }
            else
            {
                SingleBuyOptionBackTest(reqId, bar);
            }
        }
        private void SingleBuyOptionBackTest(int reqId, Bar bar)
        {
            int numberOfTrades = 0;
            bool buyTrigger = false;
            bool dayTrade = false;
            double soldStockTotal = 0, boughtStockTotal = 0;
            double boughtStock = 0;
            double soldStock = 0;

            double open = 0, close = 0, profit = 0, avgPrice = 0;

            DateTime startDate = new DateTime();
            DateTime currentDate = DateTime.ParseExact(bar.Time, "yyyyMMdd  HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            var startTime = currentDate.Date.AddHours(9).AddMinutes(30);
            var tradeStartTime = currentDate.Date.AddHours(10).AddMinutes(00);
            bool buyTimeFrame = true;
            var dontBuyTime = currentDate.Date.AddHours(15).AddMinutes(30);
            var sellTime = currentDate.Date.AddHours(15).AddMinutes(58);
            var percentage = (bar.Close - bar.Open) * 100 / bar.Open;

            if (currentDate == startTime && File.Exists("init.csv"))
            {
                File.Delete("init.csv");
            }

            using (StreamWriter w = File.AppendText("init.csv")) { }
            var text = File.ReadAllText("init.csv");
            if (string.IsNullOrEmpty(text))
            {
                File.WriteAllText("init.csv", bar.Time + ", " + bar.Open + ", " + bar.Close + ", " + dayTrade + ", " + buyTrigger + ", " + boughtStock + ", " + soldStock + ", " + profit + ", " + avgPrice + ", " + numberOfTrades);
                startDate = currentDate;
                open = bar.Open;
                close = bar.Close;
            }
            else
            {
                string[] splits = text.Split(',');
                startDate = DateTime.ParseExact(splits[0], "yyyyMMdd  HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                open = Convert.ToDouble(splits[1]);
                close = Convert.ToDouble(splits[2]);
                dayTrade = Convert.ToBoolean(splits[3]);
                buyTrigger = Convert.ToBoolean(splits[4]);
                boughtStockTotal = boughtStock = Convert.ToDouble(splits[5]);
                soldStock = Convert.ToDouble(splits[6]);
                //profit = Convert.ToDouble(splits[7]);
                avgPrice = Convert.ToDouble(splits[8]);
                numberOfTrades = Convert.ToInt32(splits[9]);
            }           

            if (currentDate < tradeStartTime || startDate > dontBuyTime)
            {
                buyTimeFrame = false;
            }
            else  if (percentage < -5)
            {
                buyTrigger = true;
            }

            if (buyTrigger)
            {
                File.WriteAllText("init.csv", bar.Time + ", " + bar.Open + ", " + bar.Close + ", " + dayTrade + ", " + buyTrigger + ", " + boughtStock + ", " + soldStock + ", " + profit + ", " + avgPrice + ", " + numberOfTrades);
                // dayTrade
                if (buyTimeFrame && boughtStock == 0 && percentage > 0)
                {
                    boughtStock = 100 * bar.Close;
                    numberOfTrades++;
                    avgPrice = avgPrice == 0 ? bar.Close : (avgPrice + bar.Close) / 2;
                    File.WriteAllText("init.csv", bar.Time + ", " + bar.Open + ", " + bar.Close + ", " + dayTrade + ", " + buyTrigger + ", " + boughtStock + ", " + soldStock + ", " + profit + ", " + avgPrice + ", " + numberOfTrades);
                }

                if (boughtStock > 0)
                {
                    if ((percentage < 0 && bar.Close > avgPrice * 1.05) || (bar.Close < avgPrice * .90) || (startDate > sellTime))
                    {
                        soldStock = 100 * bar.Close;                        
                        //numberOfTrades = 0;
                        dayTrade = true;
                        buyTrigger = false;
                        soldStockTotal = soldStock;
                        profit = soldStock - boughtStock;
                        boughtStockTotal = boughtStock;
                        boughtStock = 0;
                        soldStock = 0;
                        avgPrice = 0;
                        File.WriteAllText("init.csv", bar.Time + ", " + bar.Open + ", " + bar.Close + ", " + dayTrade + ", " + buyTrigger + ", " + boughtStock + ", " + soldStock + ", " + profit + ", " + avgPrice + ", " + numberOfTrades);
                    }
                }
            }

            if (!File.Exists("mydata.csv"))
            {
                using (StreamWriter sw = File.AppendText("mydata.csv"))
                {
                    sw.WriteLine("CurrentDateTime" + "," + "Expiration" + "," + "days" + "," + "stockValue" + "," + "strike" + "," + "callPut" + "," + "Open" + "," + "High" + "," + "Low" + "," + "Close" + "," + "Percentage" + ", " + "dayTrade" + ", " + "buyTrigger" + ", " + "boughtStock" + ", " + "soldStock" + ", " + "profit" + ", " + "avgPrice" + ", " + "numberOfTrades");
                }
            }
            using (StreamWriter sw = File.AppendText("mydata.csv"))
            {
                var textContract = File.ReadAllText(reqId + "-setupcontract.csv");
                string[] splits = textContract.Split(',');
                var date = Convert.ToDateTime(splits[0]);
                var stockValue = splits[2];
                var strike = splits[3];
                var callPut = splits[4];

                int days;
                // next friday
                days = GetDays(startDate);
                
                //var value12 = splits[5];
                //var value13 = splits[6];
                //var value14 = splits[7];
                //var value15 = splits[8];
                //var dataReadComplete = splits[9];
                //File.WriteAllText("backtest-setupcontract.csv", date.ToShortDateString() + "," + contractExpiration + "," + stockValue + "," + strike + "," + callPut + "," + value930 + "," + value10 + "," + value11 + "," + value12 + "," + value13 + "," + value14 + "," + value15);
                var contractExpiration = startDate.AddDays(days);
                days = GetBusinessDays(startDate, startDate.AddDays(days));
                sw.WriteLine(bar.Time + "," + contractExpiration + "," + days + "," + stockValue + "," + strike + "," + callPut + "," + bar.Open + "," + bar.High + "," + bar.Low + "," + bar.Close + "," + (bar.Close - bar.Open) * 100 / bar.Open + ", " + dayTrade + ", " + buyTrigger + ", " + boughtStockTotal + ", " + soldStockTotal + ", " + profit + ", " + avgPrice + ", " + numberOfTrades);
                // Flush the output to disk
                sw.Flush();
            }
        }

        private static int GetDays(DateTime startDate)
        {
            int days;
            if ((int)(startDate.DayOfWeek) > 1)
            {
                days = (int)DayOfWeek.Friday - (int)startDate.DayOfWeek + 7;
            }
            else
            {
                days = (int)DayOfWeek.Friday - (int)startDate.DayOfWeek;
            }

            return days;
        }

        private void SetUpContract(int reqId, Bar bar, DateTime date, string stockValue = null, string value930 = null, string value10 = null, string value11 = null, string value12 = null, string value13 = null, string value14 = null, string value15 = null)
        {
            //using (StreamWriter w = File.AppendText("setupcontract.csv")) { }

            var contract = ContractSamples.USOptionContract();
            var callPut = Convert.ToDouble(value10) > 0 ? "C" : "P";
            var strike = Math.Round(Convert.ToDouble(stockValue));
            contract.Right = callPut;
            contract.Strike = strike;
            int days;
            // next friday
            days = GetDays(date);

            String endTime = date.AddDays(1).ToString("yyyyMMdd HH:mm:ss");
            var contractExpiration = date.AddDays(days).ToString("yyyyMMdd");
            var dt = date.AddDays(days);
            contract.LastTradeDateOrContractMonth = contractExpiration;

            clientSocket.reqHistoricalData(Convert.ToInt32(date.ToString("yyyyMMdd")), contract, endTime, "1 D", "1 min", "MIDPOINT", 1, 1, false, null);
            using (StreamWriter w = File.AppendText(date.ToString("yyyyMMdd") + "-setupcontract.csv")) { }
            File.WriteAllText(date.ToString("yyyyMMdd") + "-setupcontract.csv", date.ToShortDateString() + "," + contractExpiration + "," + stockValue + "," + strike + "," + callPut + "," + value930 + "," + value10 + "," + value11 + "," + value12 + "," + value13 + "," + value14 + "," + value15);

        }

        private void HourlyData(int reqId, Bar bar)
        {
            Console.WriteLine("HistoricalData. " + reqId + " - Time: " + bar.Time + ", Open: " + bar.Open + ", High: " + bar.High + ", Low: " + bar.Low + ", Close: " + bar.Close + ", Volume: " + bar.Volume + ", Count: " + bar.Count + ", WAP: " + bar.WAP);

            using (StreamWriter w = File.AppendText("hourly-data.csv")) { }
            var text = File.ReadAllText("hourly-data.csv");
            var time930 = "0";
            var time10 = "0";
            var time11 = "0";
            var time12 = "0";
            var time13 = "0";
            var time14 = "0";
            var time15 = "0";
            DateTime currentDate = DateTime.ParseExact(bar.Time, "yyyyMMdd  HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            if (string.IsNullOrEmpty(text))
            {
                File.WriteAllText("hourly-data.csv", currentDate.Date + "," + bar.Close + "," + time930 + "," + time10 + "," + time11 + "," + time12 + "," + time13 + "," + time14 + "," + time15 + ",0");
            }
            else
            {
                string[] splits = text.Split(',');
                time930 = splits[2];
                time10 = splits[3];
                time11 = splits[4];
                time12 = splits[5];
                time13 = splits[6];
                time14 = splits[7];
                time15 = splits[8];
            }

            if (currentDate.TimeOfDay.Hours == 9)
            {
                time930 = ((bar.Close - bar.Open) * 100 / bar.Open).ToString();
                File.WriteAllText("hourly-data.csv", currentDate.ToShortDateString() + "," + bar.Close + "," + time930 + "," + time10 + "," + time11 + "," + time12 + "," + time13 + "," + time14 + "," + time15 + ",0");
            }
            if (currentDate.TimeOfDay.Hours == 10)
            {
                time10 = ((bar.Close - bar.Open) * 100 / bar.Open).ToString();
                File.WriteAllText("hourly-data.csv", currentDate.ToShortDateString() + "," + bar.Close + "," + time930 + "," + time10 + "," + time11 + "," + time12 + "," + time13 + "," + time14 + "," + time15 + ",0");
            }
            if (currentDate.TimeOfDay.Hours == 11)
            {
                time11 = ((bar.Close - bar.Open) * 100 / bar.Open).ToString();
                File.WriteAllText("hourly-data.csv", currentDate.ToShortDateString() + "," + bar.Close + "," + time930 + "," + time10 + "," + time11 + "," + time12 + "," + time13 + "," + time14 + "," + time15 + ",0");
            }
            if (currentDate.TimeOfDay.Hours == 12)
            {
                time12 = ((bar.Close - bar.Open) * 100 / bar.Open).ToString();
                File.WriteAllText("hourly-data.csv", currentDate.ToShortDateString() + "," + bar.Close + "," + time930 + "," + time10 + "," + time11 + "," + time12 + "," + time13 + "," + time14 + "," + time15 + ",0");
            }
            if (currentDate.TimeOfDay.Hours == 13)
            {
                time13 = ((bar.Close - bar.Open) * 100 / bar.Open).ToString();
                File.WriteAllText("hourly-data.csv", currentDate.ToShortDateString() + "," + bar.Close + "," + time930 + "," + time10 + "," + time11 + "," + time12 + "," + time13 + "," + time14 + "," + time15 + ",0");
            }
            if (currentDate.TimeOfDay.Hours == 14)
            {
                time14 = ((bar.Close - bar.Open) * 100 / bar.Open).ToString();
                File.WriteAllText("hourly-data.csv", currentDate.ToShortDateString() + "," + bar.Close + "," + time930 + "," + time10 + "," + time11 + "," + time12 + "," + time13 + "," + time14 + "," + time15 + ",0");
            }
            if (currentDate.TimeOfDay.Hours == 15)
            {
                time15 = ((bar.Close - bar.Open) * 100 / bar.Open).ToString();
                File.WriteAllText("hourly-data.csv", currentDate.ToShortDateString() + "," + bar.Close + "," + time930 + "," + time10 + "," + time11 + "," + time12 + "," + time13 + "," + time14 + "," + time15 + ",true");
                using (StreamWriter sw = File.AppendText("hourly-data-final.csv"))
                {
                    sw.WriteLine(currentDate.ToShortDateString() + "," + bar.Close + "," + time930 + "," + time10 + "," + time11 + "," + time12 + "," + time13 + "," + time14 + "," + time15 + ",0");
                    // Flush the output to disk
                    sw.Flush();
                }
            }
        }

        private void SingleBuy(int reqId, Bar bar)
        {
            int numberOfTrades = 0;
            bool buyTrigger = false;
            bool dayTrade = false;
            double boughtStock = 0;
            double soldStock = 0;
            using (StreamWriter w = File.AppendText("init.csv")) { }
            var text = File.ReadAllText("init.csv");
            double open = 0, close = 0, profit = 0, avgPrice = 0;
            DateTime startDate = new DateTime();
            DateTime currentDate = DateTime.ParseExact(bar.Time, "yyyyMMdd  HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            var percentage = (bar.Close - bar.Open) * 100 / bar.Open;

            if (string.IsNullOrEmpty(text))
            {
                if (percentage < -0.1)
                {
                    buyTrigger = true;
                }
                File.WriteAllText("init.csv", bar.Time + ", " + bar.Open + ", " + bar.Close + ", " + dayTrade + ", " + buyTrigger + ", " + boughtStock + ", " + soldStock + ", " + profit + ", " + bar.Close + ", " + avgPrice + ", " + numberOfTrades);
                startDate = currentDate;
                open = bar.Open;
                close = bar.Close;
            }
            else
            {
                string[] splits = text.Split(',');
                startDate = DateTime.ParseExact(splits[0], "yyyyMMdd  HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                open = Convert.ToDouble(splits[1]);
                close = Convert.ToDouble(splits[2]);
                dayTrade = Convert.ToBoolean(splits[3]);
                buyTrigger = Convert.ToBoolean(splits[4]);
                boughtStock = Convert.ToDouble(splits[5]);
                soldStock = Convert.ToDouble(splits[6]);
                //profit = Convert.ToDouble(splits[7]);
                avgPrice = Convert.ToDouble(splits[9]);
                numberOfTrades = Convert.ToInt32(splits[10]);
            }

            if (percentage < -5)
            {
                buyTrigger = true;
            }

            if (buyTrigger)
            {
                File.WriteAllText("init.csv", bar.Time + ", " + bar.Open + ", " + bar.Close + ", " + dayTrade + ", " + buyTrigger + ", " + boughtStock + ", " + soldStock + ", " + profit + ", " + bar.Close + ", " + avgPrice + ", " + numberOfTrades);
                if (boughtStock == 0 && percentage > 0)
                {
                    boughtStock = 100 * bar.Close;
                    numberOfTrades++;
                    avgPrice = avgPrice == 0 ? bar.Close : (avgPrice + bar.Close) / 2;
                    File.WriteAllText("init.csv", bar.Time + ", " + bar.Open + ", " + bar.Close + ", " + dayTrade + ", " + buyTrigger + ", " + boughtStock + ", " + soldStock + ", " + profit + ", " + bar.Close + ", " + avgPrice + ", " + numberOfTrades);
                    //clientSocket.placeOrder(nextOrderId++, ContractSamples.USStock(), OrderSamples.MarketOrder("BUY", 1));
                }

                if (boughtStock > 0)
                {
                    if (percentage < 0 && bar.Close > avgPrice)
                    {
                        soldStock = numberOfTrades * 100 * bar.Close;
                        avgPrice = 0;
                        numberOfTrades = 0;
                        dayTrade = true;
                        buyTrigger = false;
                        profit = soldStock - boughtStock;
                        boughtStock = 0;
                        File.WriteAllText("init.csv", bar.Time + ", " + bar.Open + ", " + bar.Close + ", " + dayTrade + ", " + buyTrigger + ", " + boughtStock + ", " + soldStock + ", " + profit + ", " + bar.Close + ", " + avgPrice + ", " + numberOfTrades);
                    }
                }
            }
            //if (currentDate.Date > startDate.Date)
            //{
            //    File.WriteAllText("init.csv", bar.Time + ", " + bar.Open + ", " + bar.Close);
            //}                    

            //decimal open, close;
            using (StreamWriter sw = File.AppendText("mydata.csv"))
            {
                // write the data to the file
                //sw.WriteLine("HistoricalData. " + reqId + " - Time: " + bar.Time + ", Open: " + bar.Open + ", High: " + bar.High + ", Low: " + bar.Low + ", Close: " + bar.Close + ", Volume: " + bar.Volume + ", Count: " + bar.Count + ", WAP: " + bar.WAP);
                //Time	Open	High	Low	Close	Volume	Count	WAP	%	dayTrade	buyTrigger	boughtStock	soldStock	profit	avgPrice	numberOfTrades

                sw.WriteLine(bar.Time + "," + bar.Open + "," + bar.High + "," + bar.Low + "," + bar.Close + "," + bar.Volume + "," + bar.Count + "," + bar.WAP + "," + (bar.Close - bar.Open) * 100 / bar.Open + ", " + dayTrade + ", " + buyTrigger + ", " + boughtStock + ", " + soldStock + ", " + profit + ", " + avgPrice + ", " + numberOfTrades);
                // Flush the output to disk
                sw.Flush();
            }
        }
        //! [realtimebar]
        
        public int GetBusinessDays(DateTime start, DateTime end)
        {
            if (start.DayOfWeek == DayOfWeek.Saturday)
            {
                start = start.AddDays(2);
            }
            else if (start.DayOfWeek == DayOfWeek.Sunday)
            {
                start = start.AddDays(1);
            }

            if (end.DayOfWeek == DayOfWeek.Saturday)
            {
                end = end.AddDays(-1);
            }
            else if (end.DayOfWeek == DayOfWeek.Sunday)
            {
                end = end.AddDays(-2);
            }

            int diff = (int)end.Subtract(start).TotalDays;

            int result = diff / 7 * 5 + diff % 7;

            if (end.DayOfWeek < start.DayOfWeek)
            {
                return result - 2;
            }
            else
            {
                return result;
            }
        }

        private void Buy(int reqId)
        {
            if (reqId == 1001)
            {
                clientSocket.placeOrder(nextOrderId++, ContractSamples.USOptionContract(), OrderSamples.MarketOrder("BUY", 100));
            }
            if (reqId == 1002)
            {
                clientSocket.placeOrder(nextOrderId++, ContractSamples.USOptionContract2(), OrderSamples.MarketOrder("BUY", 100));
            }
            if (reqId == 1003)
            {
                clientSocket.placeOrder(nextOrderId++, ContractSamples.USOptionContract3(), OrderSamples.MarketOrder("BUY", 100));
            }
            if (reqId == 1004)
            {
                clientSocket.placeOrder(nextOrderId++, ContractSamples.USOptionContract4(), OrderSamples.MarketOrder("BUY", 100));
            }
        }

        private void Sell(int reqId)
        {
            if (reqId == 1001)
            {
                clientSocket.placeOrder(nextOrderId++, ContractSamples.USOptionContract(), OrderSamples.MarketOrder("SELL", 100));
            }
            if (reqId == 1002)
            {
                clientSocket.placeOrder(nextOrderId++, ContractSamples.USOptionContract2(), OrderSamples.MarketOrder("SELL", 100));
            }
            if (reqId == 1003)
            {
                clientSocket.placeOrder(nextOrderId++, ContractSamples.USOptionContract3(), OrderSamples.MarketOrder("SELL", 100));
            }
            if (reqId == 1004)
            {
                clientSocket.placeOrder(nextOrderId++, ContractSamples.USOptionContract4(), OrderSamples.MarketOrder("SELL", 100));
            }
        }       

        private void RecursiveBuy(int reqId, Bar bar)
        {
            int numberOfTrades = 0;
            bool buyTrigger = false;
            bool dayTrade = false;
            double boughtStock = 0;
            double soldStock = 0;
            using (StreamWriter w = File.AppendText("init.csv")) { }
            var text = File.ReadAllText("init.csv");
            double open = 0, close = 0, profit = 0, avgPrice = 0, totalBought = 0;
            DateTime startDate = new DateTime();
            DateTime currentDate = DateTime.ParseExact(bar.Time, "yyyyMMdd  HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            var percentage = (bar.Close - bar.Open) * 100 / bar.Open;

            if (string.IsNullOrEmpty(text))
            {
                if (percentage < -0.1)
                {
                    buyTrigger = true;
                }
                File.WriteAllText("init.csv", bar.Time + ", " + bar.Open + ", " + bar.Close + ", " + dayTrade + ", " + buyTrigger + ", " + totalBought + ", " + boughtStock + ", " + soldStock + ", " + profit + ", " + avgPrice + ", " + numberOfTrades);
                startDate = currentDate;
                open = bar.Open;
                close = bar.Close;
            }
            else
            {
                string[] splits = text.Split(',');
                startDate = DateTime.ParseExact(splits[0], "yyyyMMdd  HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                open = Convert.ToDouble(splits[1]);
                close = Convert.ToDouble(splits[2]);
                dayTrade = Convert.ToBoolean(splits[3]);
                buyTrigger = Convert.ToBoolean(splits[4]);
                totalBought = Convert.ToDouble(splits[5]);
                boughtStock = Convert.ToDouble(splits[6]);
                //soldStock = Convert.ToDouble(splits[7]);
                //profit = Convert.ToDouble(splits[7]);
                avgPrice = Convert.ToDouble(splits[9]);
                numberOfTrades = Convert.ToInt32(splits[10]);
            }

            if (percentage < -0.1)
            {
                buyTrigger = true;
            }

            if (buyTrigger)
            {
                File.WriteAllText("init.csv", bar.Time + ", " + bar.Open + ", " + bar.Close + ", " + dayTrade + ", " + buyTrigger + ", " + totalBought + ", " + boughtStock + ", " + soldStock + ", " + profit + ", " + avgPrice + ", " + numberOfTrades);
                if (percentage > 0)
                {
                    boughtStock = 100 * bar.Close;
                    totalBought += boughtStock;
                    avgPrice = avgPrice == 0 ? bar.Close : (avgPrice * numberOfTrades + bar.Close) / (numberOfTrades + 1);
                    numberOfTrades++;
                    buyTrigger = false;
                    File.WriteAllText("init.csv", bar.Time + ", " + bar.Open + ", " + bar.Close + ", " + dayTrade + ", " + buyTrigger + ", " + totalBought + ", " + boughtStock + ", " + soldStock + ", " + profit + ", " + avgPrice + ", " + numberOfTrades);
                }

                if (boughtStock > 0)
                {
                    if (percentage < 0 && bar.Close > avgPrice)
                    {
                        soldStock = numberOfTrades * 100 * bar.Close;
                        avgPrice = 0;
                        numberOfTrades = 0;
                        dayTrade = true;
                        buyTrigger = false;
                        profit = soldStock - totalBought;
                        boughtStock = 0;
                        totalBought = 0;
                        File.WriteAllText("init.csv", bar.Time + ", " + bar.Open + ", " + bar.Close + ", " + dayTrade + ", " + buyTrigger + ", " + totalBought + ", " + boughtStock + ", " + soldStock + ", " + profit + ", " + avgPrice + ", " + numberOfTrades);
                    }
                }
            }

            //if (currentDate.Date > startDate.Date)
            //{
            //    File.WriteAllText("init.csv", bar.Time + ", " + bar.Open + ", " + bar.Close);
            //}                    

            //decimal open, close;
            using (StreamWriter sw = File.AppendText("mydata.csv"))
            {
                // write the data to the file
                //sw.WriteLine("HistoricalData. " + reqId + " - Time: " + bar.Time + ", Open: " + bar.Open + ", High: " + bar.High + ", Low: " + bar.Low + ", Close: " + bar.Close + ", Volume: " + bar.Volume + ", Count: " + bar.Count + ", WAP: " + bar.WAP);

                sw.WriteLine(bar.Time + "," + bar.Open + "," + bar.High + "," + bar.Low + "," + bar.Close + "," + bar.Volume + "," + bar.Count + "," + bar.WAP + "," + (bar.Close - bar.Open) * 100 / bar.Open + ", " + dayTrade + ", " + buyTrigger + ", " + totalBought + ", " + boughtStock + ", " + soldStock + ", " + profit + ", " + avgPrice + ", " + numberOfTrades);
                // Flush the output to disk
                sw.Flush();
            }
        }

        private string GetOptionBarSize(int reqId, long time, double open, double close, int barSize)
        {
            double lastOpen = 0, lastClose = 0;
            string data = "0,0,0";

            using (StreamWriter w = File.AppendText(DateTime.Today.ToString("yyyyMMdd") + "-" + reqId + "-obarCalculation.csv")) { }
            var text = File.ReadAllText(DateTime.Today.ToString("yyyyMMdd") + "-" + reqId + "-obarCalculation.csv");

            if (string.IsNullOrEmpty(text))
            {
                File.WriteAllText(DateTime.Today.ToString("yyyyMMdd") + "-" + reqId + "-obarCalculation.csv", time + ", " + open + ", " + close + ", " + barSize);
            }
            else
            {
                string[] splits = text.Split(',');
                lastOpen = Convert.ToDouble(splits[1]);
                lastClose = Convert.ToDouble(splits[2]);
                if (time % (5 * barSize) == 0)
                {
                    data = lastOpen.ToString() + "," + close.ToString() + "," + time.ToString();
                    File.WriteAllText(DateTime.Today.ToString("yyyyMMdd") + "-" + reqId + "-obarCalculation.csv", time + ", " + open + ", " + 0 + ", " + barSize);
                }
                else
                {
                    data = "0,0,0";
                    File.WriteAllText(DateTime.Today.ToString("yyyyMMdd") + "-" + reqId + "-obarCalculation.csv", time + ", " + lastOpen + ", " + close + ", " + barSize);
                }
            }

            using (StreamWriter sw = File.AppendText(DateTime.Today.ToString("yyyyMMdd") + "-" + reqId + "-obarSize.csv"))
            {
                sw.WriteLine(time + "," + open + "," + close + "," + barSize + "," + data.Split(',')[0] + "," + data.Split(',')[1] + "," + data.Split(',')[2]);
                // Flush the output to disk
                sw.Flush();
            }
            return data;
        }

        private string GetBarSize(long time, double open, double close, int barSize)
        {

            double lastOpen = 0, lastClose = 0;
            string data = "0,0,0";

            using (StreamWriter w = File.AppendText("barCalculation.csv")) { }
            var text = File.ReadAllText("barCalculation.csv");

            if (string.IsNullOrEmpty(text))
            {
                File.WriteAllText("barCalculation.csv", time + ", " + open + ", " + close + ", " + barSize);
            }
            else
            {
                string[] splits = text.Split(',');
                lastOpen = Convert.ToDouble(splits[1]);
                lastClose = Convert.ToDouble(splits[2]);
                if (time % (5 * barSize) == 0)
                {
                    data = lastOpen.ToString() + "," + close.ToString() + "," + time.ToString();
                    File.WriteAllText("barCalculation.csv", time + ", " + open + ", " + 0 + ", " + barSize);
                }
                else
                {
                    data = "0,0,0";
                    File.WriteAllText("barCalculation.csv", time + ", " + lastOpen + ", " + close + ", " + barSize);
                }
            }

            using (StreamWriter sw = File.AppendText("barSize.csv"))
            {
                sw.WriteLine(time + "," + open + "," + close + "," + barSize + "," + data.Split(',')[0] + "," + data.Split(',')[1] + "," + data.Split(',')[2]);
                // Flush the output to disk
                sw.Flush();
            }
            return data;
        }

        public virtual void position(string account, Contract contract, double pos, double avgCost)
        {
            Console.WriteLine("Position. " + account + " - Symbol: " + contract.Symbol + ", SecType: " + contract.SecType + ", Currency: " + contract.Currency + ", Position: " + pos + ", Avg cost: " + avgCost);

            if (pos == 0)
            {
                Contract contract4 = ContractSamples.USOptionContract4();
                if (contract.Symbol == contract4.Symbol
                    && contract.LastTradeDateOrContractMonth == contract4.LastTradeDateOrContractMonth
                    && contract.Strike == contract4.Strike)
                {
                    using (StreamWriter sw = File.AppendText(DateTime.Today.ToString("yyyyMMdd") + "-" + contract.Symbol + "-" + contract.LastTradeDateOrContractMonth + "-" + contract.Strike + "-buyTrigger.csv"))
                    {
                        sw.WriteLine("0");
                    }
                    clientSocket.cancelPositions();
                }
                //File.WriteAllText(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-oinit.csv", startDate + ", " + open + ", " + close + ", " + dayTrade + ", " + buyTrigger + ", " + boughtStock + ", " + soldStock + ", " + profit + ", " + close + ", " + avgPrice + ", " + numberOfTrades);
                //File.WriteAllText(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-oinit.csv", currentDate + ", " + open + ", " + close + ", " + dayTrade + ", " + buyTrigger + ", " + boughtStock + ", " + soldStock + ", " + profit + ", " + close + ", " + avgPrice + ", " + numberOfTrades);
                //File.WriteAllText("avgCost.csv", avgCost + "," + pos + "," + contract.Symbol + "," + contract.SecType);
                //clientSocket.placeOrder(nextOrderId++, ContractSamples.USOptionContract4(), OrderSamples.Stop("SELL", pos, Math.Round((avgCost / 100) * .90, 2)));

                //clientSocket.placeOrder(nextOrderId++, ContractSamples.USOptionContract(), OrderSamples.LimitOrder("SELL", pos, Math.Round(avgCost / 100 * 1.05, 2)));
            }
            if(pos == 100)
            {
                //clientSocket.cancelPositions();
                clientSocket.placeOrder(nextOrderId++, ContractSamples.USOptionContract(), OrderSamples.LimitOrder("SELL", 100, Math.Round(avgCost/100 + .05, 2)));
            }
        }

        public virtual void realtimeBar(int reqId, long time, double open, double high, double low, double close, long volume, double WAP, int count)
        {
            List<double> l = new List<double>();
            l.Add(open);
            l.Add(close);
            
            //if (reqId == 1003)
            //{
            //    RealOptionMinTrailingSell(reqId, time, open, high, low, close, volume, WAP, count);
            //}
            //else if (reqId == 1004)
            //{
            //    RealOptionMinLimitSell(reqId, time, open, high, low, close, volume, WAP, count);
            //}
            //else
            //{
            //    RealOptionSingleBuy(reqId, time, open, high, low, close, volume, WAP, count);
            //}
            RealOptionMinTrailingSell(reqId, time, open, high, low, close, volume, WAP, count);
            Console.WriteLine("Option RealTimeBars. " + reqId + " - Time: " + time + ", Open: " + open + ", High: " + high + ", Low: " + low + ", Close: " + close + ", Volume: " + volume + ", Count: " + count + ", WAP: " + WAP);
        }

        private void RealOptionSingleBuy(int reqId, long time, double open, double high, double low, double close, long volume, double WAP, int count)
        {
            int numberOfTrades = 0;
            bool buyTrigger = false;
            bool dayTrade = false;
            double soldStockTotal = 0, boughtStockTotal = 0;
            double boughtStock = 0;
            double soldStock = 0;
            double lastOpen = 0, lastclose = 0, profit = 0, avgPrice = 0;
            System.DateTime startDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            startDate = startDate.AddSeconds(time).ToLocalTime();
            DateTime currentDate = startDate;
            bool buyTimeFrame = true;
            var dontBuyTime = currentDate.Date.AddHours(15).AddMinutes(00);
            var sellTime = currentDate.Date.AddHours(15).AddMinutes(58);

            //if (!File.Exists(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-oinit.csv"))
            //{
            //    using (StreamWriter sw = File.AppendText(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-oinit.csv"))
            //    {
            //        //File.WriteAllText(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-oinit.csv", currentDate + ", " + open + ", " + close + ", " + dayTrade + ", " + buyTrigger + ", " + boughtStock + ", " + soldStock + ", " + profit + ", " + close + ", " + avgPrice + ", " + numberOfTrades);
            //        sw.WriteLine("currentDate" + "," + "open" + "," + "close" + "," + "dayTrade" + "," + "buyTrigger" + "," + "boughtStock" + "," + "soldStock" + "," + "profit" + "," + "close" + "," + "avgPrice" + ", " + "numberOfTrades");
            //    }
            //}

            //if (!File.Exists(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-obarCalculation.csv"))
            //{
            //    using (StreamWriter sw = File.AppendText(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-obarCalculation.csv"))
            //    {
            //        //File.WriteAllText(DateTime.Today.ToString("yyyyMMdd") + "-" + reqId + "-obarCalculation.csv", time + ", " + open + ", " + close + ", " + barSize);
            //        sw.WriteLine("time" + "," + "open" + "," + "close" + "," + "barSize");
            //    }
            //}

            using (StreamWriter w = File.AppendText(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-oinit.csv")) { }
            var text = File.ReadAllText(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-oinit.csv");

            // barsize = multiple of 5 secs. eg 1min = 12
            var per = GetOptionBarSize(reqId, time, open, close, 12);
            if (per != "0,0,0")
            {
                var barSplits = per.Split(',');
                double o = Convert.ToDouble(barSplits[0]);
                double c = Convert.ToDouble(barSplits[1]);
                open = o;
                close = c;
                var percentage = (close - open) * 100 / open;
                if (string.IsNullOrEmpty(text))
                {
                    File.WriteAllText(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-oinit.csv", startDate + ", " + open + ", " + close + ", " + dayTrade + ", " + buyTrigger + ", " + boughtStock + ", " + soldStock + ", " + profit + ", " + close + ", " + avgPrice + ", " + numberOfTrades);
                    startDate = currentDate;
                    lastOpen = open;
                    lastclose = close;
                }
                else
                {
                    string[] splits = text.Split(',');
                    //startDate = Convert.ToDateTime(splits[0]);
                    lastOpen = Convert.ToDouble(splits[1]);
                    lastclose = Convert.ToDouble(splits[2]);
                    dayTrade = Convert.ToBoolean(splits[3]);
                    buyTrigger = Convert.ToBoolean(splits[4]);
                    boughtStock = Convert.ToDouble(splits[5]);
                    soldStock = Convert.ToDouble(splits[6]);
                    //profit = Convert.ToDouble(splits[7]);
                    avgPrice = Convert.ToDouble(splits[9]);
                    numberOfTrades = Convert.ToInt32(splits[10]);
                }

                if (percentage < -5)
                {
                    buyTrigger = true;
                }

                if (startDate > dontBuyTime)
                {
                    buyTimeFrame = false;
                }

                if (buyTrigger)
                {
                    File.WriteAllText(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-oinit.csv", currentDate + ", " + open + ", " + close + ", " + dayTrade + ", " + buyTrigger + ", " + boughtStock + ", " + soldStock + ", " + profit + ", " + close + ", " + avgPrice + ", " + numberOfTrades);
                    if (buyTimeFrame && boughtStock == 0 && percentage > 0)
                    {
                        boughtStock = 100 * close;
                        numberOfTrades++;
                        avgPrice = avgPrice == 0 ? close : (avgPrice + close) / 2;
                        File.WriteAllText(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-oinit.csv", currentDate + ", " + open + ", " + close + ", " + dayTrade + ", " + buyTrigger + ", " + boughtStock + ", " + soldStock + ", " + profit + ", " + close + ", " + avgPrice + ", " + numberOfTrades);
                        Buy(reqId);

                        //System.Threading.Thread.Sleep(1000);
                        //clientSocket.placeOrder(nextOrderId++, ContractSamples.USOptionContract(), OrderSamples.Stop("SELL", 100, Math.Round(close * .85, 2)));
                        //clientSocket.reqPositions();
                        //clientSocket.cancelPositions();
                    }

                    if (boughtStock > 0)
                    {
                        if ((percentage < 0 && close > 1.05 * avgPrice) || (close < avgPrice * .90) || ((startDate > sellTime)))
                        {
                            soldStock = 100 * close;
                            avgPrice = 0;
                            //numberOfTrades = 0;
                            dayTrade = true;
                            buyTrigger = false;
                            soldStockTotal = soldStock;
                            profit = soldStock - boughtStock;
                            boughtStockTotal = boughtStock;
                            boughtStock = 0;
                            soldStock = 0;
                            File.WriteAllText(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-oinit.csv", currentDate + ", " + open + ", " + close + ", " + dayTrade + ", " + buyTrigger + ", " + boughtStock + ", " + soldStock + ", " + profit + ", " + close + ", " + avgPrice + ", " + numberOfTrades);
                            Sell(reqId);
                        }
                    }
                }

                if (!File.Exists(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-omydata.csv"))
                {
                    using (StreamWriter sw = File.AppendText(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-omydata.csv"))
                    {
                        sw.WriteLine("startDate" + "," + "Expiration" + "," + "days" + "," + "strike" + "," + "callPut" + "," + "open" + "," + "high" + "," + "low" + "," + "close" + "," + "percentage" + ", " + "dayTrade" + ", " + "buyTrigger" + ", " + "boughtStock" + ", " + "soldStock" + ", " + "profit" + ", " + "avgPrice" + ", " + "numberOfTrades");
                    }
                }

                using (StreamWriter sw = File.AppendText(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-omydata.csv"))
                {
                    string expiration = "", days = "", strike = "", callPut = "";
                    if (reqId == 1001)
                    {
                        expiration = ContractSamples.USOptionContract().LastTradeDateOrContractMonth;
                        days = GetBusinessDays(DateTime.Today, Convert.ToDateTime(DateTime.ParseExact(expiration, "yyyyMMdd", CultureInfo.InvariantCulture))).ToString();
                        strike = ContractSamples.USOptionContract().Strike.ToString();
                        callPut = ContractSamples.USOptionContract().Right;
                    }
                    if (reqId == 1002)
                    {
                        expiration = ContractSamples.USOptionContract2().LastTradeDateOrContractMonth;
                        days = GetBusinessDays(DateTime.Today, Convert.ToDateTime(DateTime.ParseExact(expiration, "yyyyMMdd", CultureInfo.InvariantCulture))).ToString();
                        strike = ContractSamples.USOptionContract2().Strike.ToString();
                        callPut = ContractSamples.USOptionContract2().Right;
                    }
                    if (reqId == 1003)
                    {
                        expiration = ContractSamples.USOptionContract3().LastTradeDateOrContractMonth;
                        days = GetBusinessDays(DateTime.Today, Convert.ToDateTime(DateTime.ParseExact(expiration, "yyyyMMdd", CultureInfo.InvariantCulture))).ToString();
                        strike = ContractSamples.USOptionContract3().Strike.ToString();
                        callPut = ContractSamples.USOptionContract3().Right;
                    }
                    //sw.WriteLine("CurrentDateTime" + "," + "Expiration" + "," + "days" + "," + "stockValue" + "," + "strike" + "," + "callPut" + "," + "Open" + "," + "High" + "," + "Low" + "," + "Close" + "," + "Percentage" + ", " + "dayTrade" + ", " + "buyTrigger" + ", " + "boughtStock" + ", " + "soldStock" + ", " + "profit" + ", " + "avgPrice" + ", " + "numberOfTrades");
                    sw.WriteLine(startDate + "," + expiration + "," + days + "," + strike + "," + callPut + "," + open + "," + high + "," + low + "," + close + "," + (close - open) * 100 / open + ", " + dayTrade + ", " + buyTrigger + ", " + boughtStockTotal + ", " + soldStockTotal + ", " + profit + ", " + avgPrice + ", " + numberOfTrades);
                    sw.Flush();
                }
            }
        }

        private void RealOptionMinTrailingSell(int reqId, long time, double open, double high, double low, double close, long volume, double WAP, int count)
        {
            int numberOfTrades = 0;
            bool buyTrigger = false;
            bool dayTrade = false;
            double soldStockTotal = 0, boughtStockTotal = 0;
            double boughtStock = 0;
            double soldStock = 0;
            double lastOpen = 0, lastclose = 0, profit = 0, avgPrice = 0;
            System.DateTime startDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            startDate = startDate.AddSeconds(time).ToLocalTime();
            DateTime currentDate = startDate;
            bool buyTimeFrame = true;
            var dontBuyTime = currentDate.Date.AddHours(15).AddMinutes(00);
            var sellTime = currentDate.Date.AddHours(15).AddMinutes(58);           

            using (StreamWriter w = File.AppendText(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-oinit.csv")) { }
            var text = File.ReadAllText(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-oinit.csv");
            var sp = text.Split(',');
            boughtStock = Convert.ToDouble(sp[5]);
            // barsize = multiple of 5 secs. eg 1min = 12
            var per = GetOptionBarSize(reqId, time, open, close, boughtStock > 0 ? 1 : 12);
            if (per != "0,0,0")
            {
                var barSplits = per.Split(',');
                double o = Convert.ToDouble(barSplits[0]);
                double c = Convert.ToDouble(barSplits[1]);
                open = o;
                close = c;
                var percentage = (close - open) * 100 / open;
                if (string.IsNullOrEmpty(text))
                {
                    File.WriteAllText(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-oinit.csv", startDate + ", " + open + ", " + close + ", " + dayTrade + ", " + buyTrigger + ", " + boughtStock + ", " + soldStock + ", " + profit + ", " + close + ", " + avgPrice + ", " + numberOfTrades);
                    startDate = currentDate;
                    lastOpen = open;
                    lastclose = close;
                }
                else
                {
                    string[] splits = text.Split(',');
                    //startDate = Convert.ToDateTime(splits[0]);
                    lastOpen = Convert.ToDouble(splits[1]);
                    lastclose = Convert.ToDouble(splits[2]);
                    dayTrade = Convert.ToBoolean(splits[3]);
                    buyTrigger = Convert.ToBoolean(splits[4]);
                    boughtStock = Convert.ToDouble(splits[5]);
                    soldStock = Convert.ToDouble(splits[6]);
                    //profit = Convert.ToDouble(splits[7]);
                    avgPrice = Convert.ToDouble(splits[9]);
                    numberOfTrades = Convert.ToInt32(splits[10]);
                }

                if (percentage < -5)
                {
                    buyTrigger = true;
                }

                if (startDate > dontBuyTime)
                {
                    buyTimeFrame = false;
                }

                if (buyTrigger)
                {
                    File.WriteAllText(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-oinit.csv", currentDate + ", " + open + ", " + close + ", " + dayTrade + ", " + buyTrigger + ", " + boughtStock + ", " + soldStock + ", " + profit + ", " + close + ", " + avgPrice + ", " + numberOfTrades);
                    if (buyTimeFrame && boughtStock == 0 && percentage > 0)
                    {
                        boughtStock = 100 * close;
                        numberOfTrades++;
                        avgPrice = avgPrice == 0 ? close : (avgPrice + close) / 2;
                        File.WriteAllText(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-oinit.csv", currentDate + ", " + open + ", " + close + ", " + dayTrade + ", " + buyTrigger + ", " + boughtStock + ", " + soldStock + ", " + profit + ", " + close + ", " + avgPrice + ", " + numberOfTrades);
                        Buy(reqId);

                        //System.Threading.Thread.Sleep(1000);
                        //clientSocket.placeOrder(nextOrderId++, ContractSamples.USOptionContract(), OrderSamples.LimitOrder("SELL", 100, Math.Round(close + .07, 2)));
                        //clientSocket.reqPositions();
                        //clientSocket.cancelPositions();
                    }

                    if (boughtStock > 0)
                    {
                        if ((percentage < 0 && close > avgPrice + 0.03) || (close < avgPrice * .90) || ((startDate > sellTime)))
                        {
                            soldStock = 100 * close;
                            avgPrice = 0;
                            //numberOfTrades = 0;
                            dayTrade = true;
                            buyTrigger = false;
                            soldStockTotal = soldStock;
                            profit = soldStock - boughtStock;
                            boughtStockTotal = boughtStock;
                            boughtStock = 0;
                            soldStock = 0;
                            File.WriteAllText(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-oinit.csv", currentDate + ", " + open + ", " + close + ", " + dayTrade + ", " + buyTrigger + ", " + boughtStock + ", " + soldStock + ", " + profit + ", " + close + ", " + avgPrice + ", " + numberOfTrades);
                            Sell(reqId);
                        }
                    }
                }

                if (!File.Exists(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-omydata.csv"))
                {
                    using (StreamWriter sw = File.AppendText(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-omydata.csv"))
                    {
                        sw.WriteLine("startDate" + "," + "Expiration" + "," + "days" + "," + "strike" + "," + "callPut" + "," + "open" + "," + "high" + "," + "low" + "," + "close" + "," + "percentage" + ", " + "dayTrade" + ", " + "buyTrigger" + ", " + "boughtStock" + ", " + "soldStock" + ", " + "profit" + ", " + "avgPrice" + ", " + "numberOfTrades");
                    }
                }

                using (StreamWriter sw = File.AppendText(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-omydata.csv"))
                {
                    string expiration = "", days = "", strike = "", callPut = "";
                    if (reqId == 1001)
                    {
                        expiration = ContractSamples.USOptionContract().LastTradeDateOrContractMonth;
                        days = GetBusinessDays(DateTime.Today, Convert.ToDateTime(DateTime.ParseExact(expiration, "yyyyMMdd", CultureInfo.InvariantCulture))).ToString();
                        strike = ContractSamples.USOptionContract().Strike.ToString();
                        callPut = ContractSamples.USOptionContract().Right;
                    }
                    if (reqId == 1002)
                    {
                        expiration = ContractSamples.USOptionContract2().LastTradeDateOrContractMonth;
                        days = GetBusinessDays(DateTime.Today, Convert.ToDateTime(DateTime.ParseExact(expiration, "yyyyMMdd", CultureInfo.InvariantCulture))).ToString();
                        strike = ContractSamples.USOptionContract2().Strike.ToString();
                        callPut = ContractSamples.USOptionContract2().Right;
                    }
                    if (reqId == 1003)
                    {
                        expiration = ContractSamples.USOptionContract3().LastTradeDateOrContractMonth;
                        days = GetBusinessDays(DateTime.Today, Convert.ToDateTime(DateTime.ParseExact(expiration, "yyyyMMdd", CultureInfo.InvariantCulture))).ToString();
                        strike = ContractSamples.USOptionContract3().Strike.ToString();
                        callPut = ContractSamples.USOptionContract3().Right;
                    }
                    //sw.WriteLine("CurrentDateTime" + "," + "Expiration" + "," + "days" + "," + "stockValue" + "," + "strike" + "," + "callPut" + "," + "Open" + "," + "High" + "," + "Low" + "," + "Close" + "," + "Percentage" + ", " + "dayTrade" + ", " + "buyTrigger" + ", " + "boughtStock" + ", " + "soldStock" + ", " + "profit" + ", " + "avgPrice" + ", " + "numberOfTrades");
                    sw.WriteLine(startDate + "," + expiration + "," + days + "," + strike + "," + callPut + "," + open + "," + high + "," + low + "," + close + "," + (close - open) * 100 / open + ", " + dayTrade + ", " + buyTrigger + ", " + boughtStockTotal + ", " + soldStockTotal + ", " + profit + ", " + avgPrice + ", " + numberOfTrades);
                    sw.Flush();
                }
            }
        }
    
        private void RealOptionMinLimitSell(int reqId, long time, double open, double high, double low, double close, long volume, double WAP, int count)
        {
            int numberOfTrades = 0;
            bool buyTrigger = false;
            bool dayTrade = false;
            double soldStockTotal = 0, boughtStockTotal = 0;
            double boughtStock = 0;
            double soldStock = 0;
            double lastOpen = 0, lastclose = 0, profit = 0, avgPrice = 0;
            System.DateTime startDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            startDate = startDate.AddSeconds(time).ToLocalTime();
            DateTime currentDate = startDate;
            bool buyTimeFrame = true;
            var dontBuyTime = currentDate.Date.AddHours(15).AddMinutes(00);
            var sellTime = currentDate.Date.AddHours(15).AddMinutes(58);

            using (StreamWriter w = File.AppendText(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-oinit.csv")) { }
            var text = File.ReadAllText(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-oinit.csv");

            var rsi = CalculateRsi(reqId, startDate, open, close, 14);
            // barsize = multiple of 5 secs. eg 1min = 12
            var per = GetOptionBarSize(reqId, time, open, close, boughtStock > 0 ? 1: 12);
            if (per != "0,0,0")
            {
                var barSplits = per.Split(',');
                double o = Convert.ToDouble(barSplits[0]);
                double c = Convert.ToDouble(barSplits[1]);
                open = o;
                close = c;
                var percentage = (close - open) * 100 / open;
                if (string.IsNullOrEmpty(text))
                {
                    File.WriteAllText(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-oinit.csv", startDate + ", " + open + ", " + close + ", " + dayTrade + ", " + buyTrigger + ", " + boughtStock + ", " + soldStock + ", " + profit + ", " + close + ", " + avgPrice + ", " + numberOfTrades);
                    startDate = currentDate;
                    lastOpen = open;
                    lastclose = close;
                }
                else
                {
                    string[] splits = text.Split(',');
                    //startDate = Convert.ToDateTime(splits[0]);
                    lastOpen = Convert.ToDouble(splits[1]);
                    lastclose = Convert.ToDouble(splits[2]);
                    dayTrade = Convert.ToBoolean(splits[3]);
                    buyTrigger = Convert.ToBoolean(splits[4]);

                    //if (reqId == 1004)
                    //{
                    //    Contract contract = ContractSamples.USOptionContract4();
                    //    if (File.Exists(DateTime.Today.ToString("yyyyMMdd") + "-" + contract.Symbol + "-" + contract.LastTradeDateOrContractMonth + "-" + contract.Strike + "-buyTrigger.csv"))
                    //    {
                    //        buyTrigger = (File.ReadAllText(DateTime.Today.ToString("yyyyMMdd") + "-" + contract.Symbol + "-" + contract.LastTradeDateOrContractMonth + "-" + contract.Strike + "-buyTrigger.csv")[0]) == '0' ? false: true;
                    //        File.Delete(DateTime.Today.ToString("yyyyMMdd") + "-" + contract.Symbol + "-" + contract.LastTradeDateOrContractMonth + "-" + contract.Strike + "-buyTrigger.csv");
                    //    }
                    //}
                    
                    boughtStock = Convert.ToDouble(splits[5]);
                    soldStock = Convert.ToDouble(splits[6]);
                    //profit = Convert.ToDouble(splits[7]);
                    avgPrice = Convert.ToDouble(splits[9]);
                    numberOfTrades = Convert.ToInt32(splits[10]);
                }

                if (percentage < -5)
                {
                    buyTrigger = true;
                }

                if (startDate > dontBuyTime)
                {
                    buyTimeFrame = false;
                }

                if (buyTrigger)
                {
                    File.WriteAllText(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-oinit.csv", currentDate + ", " + open + ", " + close + ", " + dayTrade + ", " + buyTrigger + ", " + boughtStock + ", " + soldStock + ", " + profit + ", " + close + ", " + avgPrice + ", " + numberOfTrades);
                    if (buyTimeFrame && boughtStock == 0 && percentage > 0)
                    {
                        boughtStock = 100 * close;
                        numberOfTrades++;
                        avgPrice = avgPrice == 0 ? close : (avgPrice + close) / 2;
                        File.WriteAllText(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-oinit.csv", currentDate + ", " + open + ", " + close + ", " + dayTrade + ", " + buyTrigger + ", " + boughtStock + ", " + soldStock + ", " + profit + ", " + close + ", " + avgPrice + ", " + numberOfTrades);
                        Buy(reqId);

                        //System.Threading.Thread.Sleep(3000);
                        //clientSocket.placeOrder(nextOrderId++, ContractSamples.USOptionContract(), OrderSamples.LimitOrder("SELL", 100, Math.Round(close + .05, 2)));
                        //clientSocket.reqPositions();
                        //clientSocket.cancelPositions();
                    }

                    if (boughtStock > 0)
                    {
                        if ((percentage < 0 && close > avgPrice + 0.05) || (close < avgPrice * .90) || ((startDate > sellTime)))
                        {
                            soldStock = 100 * close;
                            avgPrice = 0;
                            //numberOfTrades = 0;
                            dayTrade = true;
                            buyTrigger = false;
                            soldStockTotal = soldStock;
                            profit = soldStock - boughtStock;
                            boughtStockTotal = boughtStock;
                            boughtStock = 0;
                            soldStock = 0;
                            File.WriteAllText(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-oinit.csv", currentDate + ", " + open + ", " + close + ", " + dayTrade + ", " + buyTrigger + ", " + boughtStock + ", " + soldStock + ", " + profit + ", " + close + ", " + avgPrice + ", " + numberOfTrades);
                            Sell(reqId);
                        }
                    }
                }

                if (!File.Exists(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-omydata.csv"))
                {
                    using (StreamWriter sw = File.AppendText(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-omydata.csv"))
                    {
                        sw.WriteLine("startDate" + "," + "Expiration" + "," + "days" + "," + "strike" + "," + "callPut" + "," + "open" + "," + "high" + "," + "low" + "," + "close" + "," + "percentage" + ", " + "dayTrade" + ", " + "buyTrigger" + ", " + "boughtStock" + ", " + "soldStock" + ", " + "profit" + ", " + "avgPrice" + ", " + "numberOfTrades");
                    }
                }

                using (StreamWriter sw = File.AppendText(currentDate.ToString("yyyyMMdd") + "-" + reqId + "-omydata.csv"))
                {
                    string expiration = "", days = "", strike = "", callPut = "";
                    if (reqId == 1001)
                    {
                        expiration = ContractSamples.USOptionContract().LastTradeDateOrContractMonth;
                        days = GetBusinessDays(DateTime.Today, Convert.ToDateTime(DateTime.ParseExact(expiration, "yyyyMMdd", CultureInfo.InvariantCulture))).ToString();
                        strike = ContractSamples.USOptionContract().Strike.ToString();
                        callPut = ContractSamples.USOptionContract().Right;
                    }
                    if (reqId == 1002)
                    {
                        expiration = ContractSamples.USOptionContract2().LastTradeDateOrContractMonth;
                        days = GetBusinessDays(DateTime.Today, Convert.ToDateTime(DateTime.ParseExact(expiration, "yyyyMMdd", CultureInfo.InvariantCulture))).ToString();
                        strike = ContractSamples.USOptionContract2().Strike.ToString();
                        callPut = ContractSamples.USOptionContract2().Right;
                    }
                    if (reqId == 1003)
                    {
                        expiration = ContractSamples.USOptionContract3().LastTradeDateOrContractMonth;
                        days = GetBusinessDays(DateTime.Today, Convert.ToDateTime(DateTime.ParseExact(expiration, "yyyyMMdd", CultureInfo.InvariantCulture))).ToString();
                        strike = ContractSamples.USOptionContract3().Strike.ToString();
                        callPut = ContractSamples.USOptionContract3().Right;
                    }
                    //sw.WriteLine("CurrentDateTime" + "," + "Expiration" + "," + "days" + "," + "stockValue" + "," + "strike" + "," + "callPut" + "," + "Open" + "," + "High" + "," + "Low" + "," + "Close" + "," + "Percentage" + ", " + "dayTrade" + ", " + "buyTrigger" + ", " + "boughtStock" + ", " + "soldStock" + ", " + "profit" + ", " + "avgPrice" + ", " + "numberOfTrades");
                    sw.WriteLine(startDate + "," + expiration + "," + days + "," + strike + "," + callPut + "," + open + "," + high + "," + low + "," + close + "," + (close - open) * 100 / open + ", " + dayTrade + ", " + buyTrigger + ", " + boughtStockTotal + ", " + soldStockTotal + ", " + profit + ", " + avgPrice + ", " + numberOfTrades);
                    sw.Flush();
                }
            }
        }

        public static double? CalculateRsi(int reqId, DateTime date, double open, double close, int period)
        {
            var difference = close - open;            
            using (StreamWriter w = File.AppendText(date.ToString("yyyyMMdd") + "-" + reqId + "-rsiCal.csv")) { }
            var text = File.ReadAllText(date.ToString("yyyyMMdd") + "-" + reqId + "-rsiCal.csv");
            if (text != null)
            {
                text = difference.ToString();
            }
            else
            {
                text = difference.ToString() + "," + text;
            }
            File.WriteAllText(date.ToString("yyyyMMdd") + "-" + reqId + "-rsiCal.csv", text);


            var previousGainLoss = text.Split(',');
            double? rsi;
            double sumGain = 0;
            int gainCounter = 0;
            double sumLoss = 0;
            int lossCounter = 0;
            for (int i = 0; i < previousGainLoss.Length; i++)
            {
                double val = Convert.ToDouble(previousGainLoss[i]);
                if (val >= 0)
                {
                    if(gainCounter != period)
                    {
                        sumGain += val;
                        gainCounter++;
                    }                    
                }
                else
                {
                    if (lossCounter != period)
                    {
                        sumLoss += val;
                        lossCounter++;
                    }
                }
                if (gainCounter == period && lossCounter == period)
                {
                    break;
                }
            }

            if (gainCounter == period && lossCounter == period)
            {
                sumLoss = sumLoss * -1;
                var relativeStrength = sumGain / sumLoss;
                if (sumGain == 0) return 0;
                if (sumLoss == 0) return 100;
                rsi = 100.0 - (100.0 / (1 + relativeStrength));
                using (StreamWriter sw = File.AppendText(date.ToString("yyyyMMdd") + "-" + reqId + "-rsi.csv"))
                {
                    sw.WriteLine(date + "," + rsi);
                    sw.Flush();
                }
                return rsi;
            }            
            return null;
        }
    }
}