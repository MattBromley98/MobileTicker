using System;
using System.IO;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using SkiaSharp;
using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Linq;



namespace test
{
    public partial class App : Application
    {
        //JSON Settings to ensure all stocks can be parsed
        static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Include,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        public static List<List<Double>> history = new List<List<Double>>();
        public static string Currency = "USD";
        public static List<Result> StockList = new List<Result>();
        //List of the Sector Names
        public static ObservableCollection<Result> listItemsDisplay = new ObservableCollection<Result>();
        static stockdatabase Database;
        public static ListSectors SectorData = new ListSectors();
        public static string TimeFrame = "1y";
        public static string Interval = "1wk";
        //Create the database if not already constructed
        public static stockdatabase DataBase
        {
            get
            {
                if (Database == null)
                {
                    Database = new stockdatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "stocks.db3"));
                }
                return Database;
            }
        }
        public App()
        {
            
            //Populate the List of Sectors
            SectorData.sectordata.Add(new Sectors { Label = "Air Travel", ValueLabel = 0, Color = "#2c3e50" });
            SectorData.sectordata.Add(new Sectors { Label = "Basic Materials", ValueLabel = 0, Color = "#7831d0" });
            SectorData.sectordata.Add(new Sectors { Label = "Communication Services", ValueLabel = 0, Color = "#3f3718" });
            SectorData.sectordata.Add(new Sectors { Label = "Conglomerates", ValueLabel = 0, Color = "#ac5a3a" });
            SectorData.sectordata.Add(new Sectors { Label = "Consumer Cyclical", ValueLabel = 0, Color = "#1973f0" });
            SectorData.sectordata.Add(new Sectors { Label = "Consumer Defensive", ValueLabel = 0, Color = "#12e687" });
            SectorData.sectordata.Add(new Sectors { Label = "Energy", ValueLabel = 0, Color = "#f63d87" });
            SectorData.sectordata.Add(new Sectors { Label = "Financial", ValueLabel = 0, Color = "#be2c25" });
            SectorData.sectordata.Add(new Sectors { Label = "Financial Services", ValueLabel = 0, Color = "#54715a" });
            SectorData.sectordata.Add(new Sectors { Label = "Healthcare", ValueLabel = 0, Color = "#2b0e5d" });
            SectorData.sectordata.Add(new Sectors { Label = "Industrial Goods", ValueLabel = 0, Color = "#d5d47d" });
            SectorData.sectordata.Add(new Sectors { Label = "Industrials", ValueLabel = 0, Color = "#44298e" });
            SectorData.sectordata.Add(new Sectors { Label = "Real Estate", ValueLabel = 0, Color = "#56c664" });
            SectorData.sectordata.Add(new Sectors { Label = "Services", ValueLabel = 0, Color = "#200e99" });
            SectorData.sectordata.Add(new Sectors { Label = "Technology", ValueLabel = 0, Color = "#31946e" });
            SectorData.sectordata.Add(new Sectors { Label = "Utilities", ValueLabel = 0, Color = "#dc7314" });

            InitializeComponent();
            XF.Material.Forms.Material.Init(this);
            MainPage = new AppShell();
            // MainPage = new NavigationPage(new test.MainPage());
        }

        public static async Task<double> CurrencyConvertAsync(string ConvertFrom, string ConvertTo, double Value){
            //Converts a Value from One Currency to another using latest Yahoo Finance Currency Values
            //ConvertFrom -> Currency to Convert From e.g (GBp)
            //ConvertTo - > Currency to Convert To e.g (USD)
            double newValue = 0;
            ConvertTo = App.Currency;
            //First check the stock is not listed in Pennies
            if (ConvertFrom == "GBp")
            {
                Value = Value / 100;
                ConvertFrom = "GBP";
            }
            //Define the ticker symbol of the Convert Rate
            string currencyTicker = ConvertFrom + ConvertTo + "=X";
            string url = $"https://query1.finance.yahoo.com/v7/finance/quote?lang=en-US&region=US&corsDomain=finance.yahoo.com&symbols={currencyTicker}";
            string jsonData = new WebClient().DownloadString(url);
            Root newData = JsonConvert.DeserializeObject<Root>(jsonData);
            double currentPrice = newData.quoteResponse.result[0].ask;
            newValue = Value * currentPrice; 
            return newValue;
            }

        public static List<double> CurrencyConvertList(string ConvertFrom, string ConvertTo, List<double> Value)
        {
            //Converts a Value from One Currency to another using latest Yahoo Finance Currency Values
            //ConvertFrom -> Currency to Convert From e.g (GBp)
            //ConvertTo - > Currency to Convert To e.g (USD)
            ConvertTo = App.Currency;
            //First check the stock is not listed in Pennies
            if (ConvertFrom == "GBp")
            {
                Value = Value.Select(r => r / 100).ToList();
                ConvertFrom = "GBP";
            }
            //Define the ticker symbol of the Convert Rate
            string currencyTicker = ConvertFrom + ConvertTo + "=X";
            string url = $"https://query1.finance.yahoo.com/v7/finance/quote?lang=en-US&region=US&corsDomain=finance.yahoo.com&symbols={currencyTicker}";
            string jsonData = new WebClient().DownloadString(url);
            Root newData = JsonConvert.DeserializeObject<Root>(jsonData);
            double currentPrice = newData.quoteResponse.result[0].ask;
            Value = Value.Select(r => r * currentPrice).ToList();
            return Value;
        }

        public static void Get_History(string symbol)
        {
            //Check if Symbol contains any dots if so replace them to ensure the URL can be parsed
            symbol = symbol.Replace(".", "%2E");
            Console.WriteLine($"NEW STRING {symbol}");
            string url = $"https://query1.finance.yahoo.com/v7/finance/chart/{symbol}?range={App.TimeFrame}&interval={App.Interval}&indicators=quote&includeTimestamps=true";
            string jsonData = new WebClient().DownloadString(url);

            Root2 newHistory = JsonConvert.DeserializeObject<Root2>(jsonData, settings);


            
            //Ensure the history is in the correct currency

            var ClosePrice = CurrencyConvertList(newHistory.chart.result[0].meta.currency, App.Currency, newHistory.chart.result[0].indicators.quote[0].close);
            App.history.Add(ClosePrice);

        }


        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
