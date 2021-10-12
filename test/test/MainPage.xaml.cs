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
using XF.Material.Forms.UI.Dialogs;

namespace test
{
    public partial class MainPage : ContentPage
    {
        private string stockName = "";
        private string url = "";
        private Result selectedStock = new Result();
        public List<Result> listItems = new List<Result>();
        ToolbarItem item = new ToolbarItem
        {
            Text = "Delete",
            Order = ToolbarItemOrder.Primary,
            Priority = 1,
        };
        public MainPage()
        {
            
            InitializeComponent();
            OnAppearing();
            this.ToolbarItems.Add(item);
            item.IsEnabled = false;
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

        void Handle_ItemSelected(object sender, ItemTappedEventArgs e)
        {
            /*
             * Function which calls the Info Page to edit a Stock
             */
            if (e.Item != null)
            {
                //Check if the toolbar is already displayed

                // "this" refers to a Page object
                    item.IsEnabled = true;
                    Result stock = (Result)e.Item;
                    selectedStock = stock;
                    item.Clicked += Delete_Stock;
                    

                
            }
        }

        async void Delete_Stock(object sender, EventArgs e)
        {
            App.listItemsDisplay.Remove(selectedStock);
            await App.DataBase.DeleteStockAsync(selectedStock);
            item.IsEnabled = false;
            
            await Navigation.PushAsync(new MainPage(),false);
            await Navigation.PopAsync();
        }

        void Goto_Add(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new AddStock());
        }



        async void Update_Tickers(Object source, ElapsedEventArgs e)
        {
            listItems.Clear();
            listItems = await App.DataBase.GetStocksAsync();
            try
            {
                int maxid = listItems.Max(t => t.ID);

                //Console.WriteLine(listItems.ToString());
                var DatabaseLength = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "stocks.db3")).Length;
                for (int i = 0; i < maxid; i++)
                {
                    string sectorname = listItems[i].sector;
                    string symbolname = listItems[i].symbol;
                    int IDNumber = listItems[i].ID;
                    string sectorName = listItems[i].sector;
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
            catch
            {
                Console.WriteLine("Error updating a symbol...\n");
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
            string priceinString = inStock.quoteResponse.result[0].ask.ToString();
            inStock.quoteResponse.result[0].outputPrice = priceinString + " " + inStock.quoteResponse.result[0].currency;
            return inStock;
        }
        
        async void example_ticker(string ticker)
        {

        }
    }
}
