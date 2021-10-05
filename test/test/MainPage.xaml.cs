using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.Net;


namespace test
{
    public partial class MainPage : ContentPage
    {
        private string stockName = "";
        private string url = "";
        public MainPage()
        {
            /*
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
            App.StockList.Add(newStock.quoteResponse.result[0]);
            //MainLayout.Children.Add(listView);

            */
            InitializeComponent();
            OnAppearing();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            itemListView.ItemsSource = await App.DataBase.GetStocksAsync();
        }

        public void Add_Clicked(object sender, EventArgs e)
        {
             /*
            stockname = stock.text;
            url = $"https://query1.finance.yahoo.com/v7/finance/quote?lang=en-us&region=us&corsdomain=finance.yahoo.com&symbols={stockname}";
            string jsondata = new webclient().downloadstring(url);
            root newstock = jsonconvert.deserializeobject<root>(jsondata);
            newstock.quoteresponse.result[0].change = ((newstock.quoteresponse.result[0].ask - newstock.quoteresponse.result[0].regularmarketpreviousclose)/ newstock.quoteresponse.result[0].ask) * 100;
            if (newstock.quoteresponse.result[0].change > 0)
            {
                newstock.quoteresponse.result[0].color = "green";
            }
            else
            {
                newstock.quoteresponse.result[0].color = "red";
            }
            stockfinal.add(newstock.quoteresponse.result[0]);
            itemlistview.beginrefresh();
            itemlistview.itemssource = null;
            itemlistview.itemssource = stockfinal;
            itemlistview.endrefresh();
             */
        }

        void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)

        {

            //Survey selected

        }

        void Goto_Add(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new AddStock());
        }


        void Handle_Delete_Clicked(object sender, SelectedItemChangedEventArgs e)

        {

            //Survey selected

        }
    }
}
