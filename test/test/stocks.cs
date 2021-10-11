using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using SkiaSharp;
using SQLite;
using Xamarin.Forms;

namespace test
{
    public class Result
        {
        [PrimaryKey, AutoIncrement]
            public int ID { get; set; }
            public string language { get; set; }
            public string region { get; set; }
            public string quoteType { get; set; }

            public string sector { get; set; }
            public string quoteSourceName { get; set; }
            public bool triggerable { get; set; }
            public string currency { get; set; }
            public string outputPrice { get; set; }
            public long firstTradeDateMilliseconds { get; set; }
            public double regularMarketChange { get; set; }
            public double regularMarketChangePercent { get; set; }
            public int regularMarketTime { get; set; }
            public double regularMarketPrice { get; set; }
            public double regularMarketDayHigh { get; set; }
            public string regularMarketDayRange { get; set; }
            public double regularMarketDayLow { get; set; }
            public double regularMarketVolume { get; set; }
            public double regularMarketPreviousClose { get; set; }
            public double bid { get; set; }
            public double ask { get; set; }
            public int bidSize { get; set; }
            public int askSize { get; set; }
            public string fullExchangeName { get; set; }
            public string financialCurrency { get; set; }
            public double regularMarketOpen { get; set; }
            public double averageDailyVolume3Month { get; set; }
            public double averageDailyVolume10Day { get; set; }
            public double fiftyTwoWeekLowChange { get; set; }
            public double fiftyTwoWeekLowChangePercent { get; set; }
            public string fiftyTwoWeekRange { get; set; }
            public double fiftyTwoWeekHighChange { get; set; }
            public double fiftyTwoWeekHighChangePercent { get; set; }
            public double fiftyTwoWeekLow { get; set; }
            public double fiftyTwoWeekHigh { get; set; }
            public int earningsTimestamp { get; set; }
            public int earningsTimestampStart { get; set; }
            public int earningsTimestampEnd { get; set; }
            public double trailingPE { get; set; }
            public double epsTrailingTwelveMonths { get; set; }
            public double epsForward { get; set; }
            public double epsCurrentYear { get; set; }
            public double priceEpsCurrentYear { get; set; }
            public long sharesOutstanding { get; set; }
            public double bookValue { get; set; }
            public double fiftyDayAverage { get; set; }
            public double fiftyDayAverageChange { get; set; }
            public double fiftyDayAverageChangePercent { get; set; }
            public double twoHundredDayAverage { get; set; }
            public double twoHundredDayAverageChange { get; set; }
            public double twoHundredDayAverageChangePercent { get; set; }
            public long marketCap { get; set; }
            public double forwardPE { get; set; }
            public double priceToBook { get; set; }
            public int sourceInterval { get; set; }
            public int exchangeDataDelayedBy { get; set; }
            public string averageAnalystRating { get; set; }
            public bool tradeable { get; set; }
            public int priceHint { get; set; }
            public double preMarketChange { get; set; }
            public double preMarketChangePercent { get; set; }
            public int preMarketTime { get; set; }
            public double preMarketPrice { get; set; }
            public string exchange { get; set; }
            public string longName { get; set; }
            public string messageBoardId { get; set; }
            public string exchangeTimezoneName { get; set; }
            public string exchangeTimezoneShortName { get; set; }
            public int gmtOffSetMilliseconds { get; set; }
            public string market { get; set; }
            public bool esgPopulated { get; set; }
            public string shortName { get; set; }
            public string marketState { get; set; }
            public string displayName { get; set; }
            public string symbol { get; set; }
            public double change { get; set; }
            public string color { get; set; }



    }
    public class QuoteResponse
        {
            public List<Result> result { get; set; }
            public object error { get; set; }
        }

        public class Root
        {
            public QuoteResponse quoteResponse { get; set; }
        }

    public class Sectors
    {
        public string Label { get; set; }
        public int ValueLabel { get; set; }
        public string Color { get; set; }

    }

    public class ListSectors : List<Sectors>
    {
        public ListSectors()
        {
            sectordata = new List<Sectors>();
            SectorNames = new ObservableCollection<string>();
        }
        
        public List<Sectors> sectordata { get; set; }
        
        public ObservableCollection<String> SectorNames { get; set; }
        public ObservableCollection<String> Retrieve_Items()
        {
            SectorNames.Clear();
            foreach (Sectors i in sectordata)
            {
                SectorNames.Add(i.Label);
            };
            return SectorNames;
        }
    }
}

