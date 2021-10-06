using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Timers;
using System.Collections.ObjectModel;

namespace test
{
    public partial class MainPage : ContentPage
    {
        private string stockName = "";
        private string url = "";
        public List<Result> listItems = new List<Result>();
        
        public MainPage()
        {
            
            InitializeComponent();
            OnAppearing();
            Timer t = new Timer(20000);
            t.AutoReset = true;
            t.Elapsed += new ElapsedEventHandler(Update_Tickers);
            t.Start();
        }
           async void Populate_List()
        {
            
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            listItems = await App.DataBase.GetStocksAsync();
            App.listItemsDisplay = new ObservableCollection<Result>(listItems);
            itemListView.ItemsSource = App.listItemsDisplay;
            /*
            itemListView.GroupDisplayBinding = new Binding("sector");
            itemListView.IsGroupingEnabled = true;
            */
        }

        async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
             * Function which calls the Info Page to edit a Stock
             */
            if (e.CurrentSelection != null)
            {
                Result stock = (Result)e.CurrentSelection;
                await Shell.Current.GoToAsync($"{nameof(AddStock)}?{nameof(AddStock.inputId)}={stock.ID}={stock.ID.ToString()}");
            }
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

        async void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)

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

        async void Update_Tickers(Object source, ElapsedEventArgs e)
        {
            listItems = await App.DataBase.GetStocksAsync();
            int maxid = listItems.Max(t => t.ID);

            //Console.WriteLine(listItems.ToString());
            var DatabaseLength = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "stocks.db3")).Length;
            for (int i = 0; i < maxid;  i++)
            {

                string symbolname = listItems[i].symbol;
                int IDNumber = listItems[i].ID;
                Console.WriteLine($"Updating symbol: {symbolname}");
                url = $"https://query1.finance.yahoo.com/v7/finance/quote?lang=en-US&region=US&corsDomain=finance.yahoo.com&symbols={symbolname}";
                string jsonData = new WebClient().DownloadString(url);
                Root newData = await Populate_Item(jsonData);
                newData.quoteResponse.result[0].ID = IDNumber;
                if (newData.quoteResponse.result[0].change > 0)
                {
                    newData.quoteResponse.result[0].color = "Green";
                }
                else
                {
                    newData.quoteResponse.result[0].color = "Red";
                }

                await App.DataBase.SaveStockAsync(newData.quoteResponse.result[0]);
                App.listItemsDisplay[i] = newData.quoteResponse.result[0];
            }
        }

        async Task<Root> Populate_Item(string jsonData)
        {
            Root inStock = JsonConvert.DeserializeObject<Root>(jsonData);
            inStock.quoteResponse.result[0].change = ((inStock.quoteResponse.result[0].ask - inStock.quoteResponse.result[0].regularMarketPreviousClose) / inStock.quoteResponse.result[0].ask) * 100;
            //Fix the pct change to 0 if the market is not open yet
            if (inStock.quoteResponse.result[0].change <= -50)
            {
                inStock.quoteResponse.result[0].change = 0;
                inStock.quoteResponse.result[0].ask = inStock.quoteResponse.result[0].regularMarketPreviousClose;
            }
            return inStock;
        }
        
        async void example_ticker(string ticker)
        {

        }
    }
}
