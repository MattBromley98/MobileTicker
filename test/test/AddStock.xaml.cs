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

namespace test
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddStock : ContentPage
    {
        public AddStock()
        {
            InitializeComponent();
        }

        public void Add_Item(object sender, EventArgs e)
        {
            string url = "";
            string StockName = "";
            //What to execute when the Add Button is pressed on the AddStock page
            StockName = Ticker.Text;
            url = $"https://query1.finance.yahoo.com/v7/finance/quote?lang=en-US&region=US&corsDomain=finance.yahoo.com&symbols={StockName}";

            string jsonData = new WebClient().DownloadString(url);
            Root newStock = JsonConvert.DeserializeObject<Root>(jsonData);
            newStock.quoteResponse.result[0].change = ((newStock.quoteResponse.result[0].ask - newStock.quoteResponse.result[0].regularMarketPreviousClose) / newStock.quoteResponse.result[0].ask) * 100;
            if (newStock.quoteResponse.result[0].change > 0)
            {
                newStock.quoteResponse.result[0].color = "Green";
            }
            else
            {
                newStock.quoteResponse.result[0].color = "Red";
            }

            App.StockList.Add(newStock.quoteResponse.result[0]);
            App.Current.MainPage.Navigation.PushAsync(new MainPage());
        }

    }


}