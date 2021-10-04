using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.Net;

public class Result
{
    public string language { get; set; }
    public string region { get; set; }
    public string quoteType { get; set; }
    public string quoteSourceName { get; set; }
    public bool triggerable { get; set; }
    public string currency { get; set; }
    public long firstTradeDateMilliseconds { get; set; }
    public double regularMarketChange { get; set; }
    public double regularMarketChangePercent { get; set; }
    public int regularMarketTime { get; set; }
    public double regularMarketPrice { get; set; }
    public double regularMarketDayHigh { get; set; }
    public string regularMarketDayRange { get; set; }
    public double regularMarketDayLow { get; set; }
    public int regularMarketVolume { get; set; }
    public double regularMarketPreviousClose { get; set; }
    public double bid { get; set; }
    public double ask { get; set; }
    public int bidSize { get; set; }
    public int askSize { get; set; }
    public string fullExchangeName { get; set; }
    public string financialCurrency { get; set; }
    public double regularMarketOpen { get; set; }
    public int averageDailyVolume3Month { get; set; }
    public int averageDailyVolume10Day { get; set; }
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


namespace test
{
    public partial class MainPage : ContentPage
    {
        private string stockName = "";
        private string url = "";
        public List<Result> stockfinal = new List<Result>();
        public MainPage()
        {
            url = $"https://query1.finance.yahoo.com/v7/finance/quote?lang=en-US&region=US&corsDomain=finance.yahoo.com&symbols=MSFT";
            string jsonData = new WebClient().DownloadString(url);
            Root newStock = JsonConvert.DeserializeObject<Root>(jsonData);
            newStock.quoteResponse.result[0].change = ((newStock.quoteResponse.result[0].ask - newStock.quoteResponse.result[0].regularMarketPreviousClose)/ newStock.quoteResponse.result[0].ask) * 100;
            //Calculate the colour for the Pct Change (Will Change this to map to relative pct change soon)
            if (newStock.quoteResponse.result[0].change > 0)
            {
                newStock.quoteResponse.result[0].color = "Green";
            }
            else
            {
                newStock.quoteResponse.result[0].color = "Red";
            }
            stockfinal.Add(newStock.quoteResponse.result[0]);
            //MainLayout.Children.Add(listView);
            InitializeComponent();
            itemListView.ItemsSource = stockfinal;
            

        }

        private void Add_Clicked(object sender, EventArgs e)
        {
            stockName = Stock.Text;
            url = $"https://query1.finance.yahoo.com/v7/finance/quote?lang=en-US&region=US&corsDomain=finance.yahoo.com&symbols={stockName}";
            string jsonData = new WebClient().DownloadString(url);
            Root newStock = JsonConvert.DeserializeObject<Root>(jsonData);
            newStock.quoteResponse.result[0].change = ((newStock.quoteResponse.result[0].ask - newStock.quoteResponse.result[0].regularMarketPreviousClose)/ newStock.quoteResponse.result[0].ask) * 100;
            if (newStock.quoteResponse.result[0].change > 0)
            {
                newStock.quoteResponse.result[0].color = "Green";
            }
            else
            {
                newStock.quoteResponse.result[0].color = "Red";
            }
            stockfinal.Add(newStock.quoteResponse.result[0]);
            itemListView.BeginRefresh();
            itemListView.ItemsSource = null;
            itemListView.ItemsSource = stockfinal;
            itemListView.EndRefresh();

        }

        void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)

        {

            //Survey selected

        }


        void Handle_Delete_Clicked(object sender, SelectedItemChangedEventArgs e)

        {

            //Survey selected

        }
    }
}
