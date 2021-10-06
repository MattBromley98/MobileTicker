using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Net;
using Xamarin.Essentials;
using SQLite;

namespace test
{
    [QueryProperty(nameof(inputId), nameof(inputId))]
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class AddStock : ContentPage
    {
        public List<String> SectorList = new List<string>();
        public bool isBusy = false;
        public AddStock()
        {
            InitializeComponent();
            //Add a list of sectors for the user to choose
            SectorList.Add("Air Travel");
            SectorList.Add("Basic Materials");
            SectorList.Add("Communication Services");
            SectorList.Add("Conglomerates");
            SectorList.Add("Consumer Cyclical");
            SectorList.Add("Consumer Defensive");
            SectorList.Add("Energy");
            SectorList.Add("Financial");
            SectorList.Add("Financial Services");
            SectorList.Add("Healthcare");
            SectorList.Add("Industrial Goods");
            SectorList.Add("Industrials");
            SectorList.Add("Real Estate");
            SectorList.Add("Services");
            SectorList.Add("Technology");
            SectorList.Add("Utilities");
            Type.ItemsSource = SectorList;
            
            


            
            
        }
        public string inputId
        {
            set
            {
                Load_Stock(value);
            }
        }

        async void Add_Item(object sender, EventArgs e)
        {
            if (isBusy == false) {
            isBusy = true;
            string url = "";
            string StockName = "";
            //What to execute when the Add Button is pressed on the AddStock page
            StockName = Ticker.Text;
            url = $"https://query1.finance.yahoo.com/v7/finance/quote?lang=en-US&region=US&corsDomain=finance.yahoo.com&symbols={StockName}";

            string jsonData = new WebClient().DownloadString(url);

            Root newStock = await Populate_Item(jsonData);
            try
            {
                newStock.quoteResponse.result[0].sector = Type.Items[Type.SelectedIndex];
            }
            catch
            {
                newStock.quoteResponse.result[0].sector = "";
            }
            
            if (newStock.quoteResponse.result[0].change > 0)
            {
                newStock.quoteResponse.result[0].color = "Green";
            }
            else
            {
                newStock.quoteResponse.result[0].color = "Red";
            }
            if (!string.IsNullOrWhiteSpace(newStock.quoteResponse.result[0].shortName))
            {
                await App.DataBase.SaveStockAsync(newStock.quoteResponse.result[0]);
                App.listItemsDisplay.Add(newStock.quoteResponse.result[0]);
            }
            await Navigation.PopAsync();
            isBusy = false;
            await Navigation.PushAsync(new MainPage());
            }
        }
        async Task<Root> Populate_Item(string jsonData)
        {
            Root inStock = JsonConvert.DeserializeObject<Root>(jsonData);
            inStock.quoteResponse.result[0].change = ((inStock.quoteResponse.result[0].ask - inStock.quoteResponse.result[0].regularMarketPreviousClose) / inStock.quoteResponse.result[0].ask) * 100;
            if (inStock.quoteResponse.result[0].change <= -50)
            {
                inStock.quoteResponse.result[0].change = 0;
                inStock.quoteResponse.result[0].ask = inStock.quoteResponse.result[0].regularMarketPreviousClose;
            }
            return inStock;
        }

        async void Load_Stock(string itemID)
        {
            try
            {
                int id = Convert.ToInt32(itemID);
                Result stock = await App.DataBase.GetStockAsync(id);
                BindingContext = stock;
            }
            catch
            {
                Console.WriteLine($"Failed to load Stock ID {itemID}");
            }
        }
    }


}