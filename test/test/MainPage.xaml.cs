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
using System.Collections;

namespace test
{
    public partial class MainPage : ContentPage
    {
        private string stockName = "";
        private string url = "";
        private Result selectedStock = new Result();
        public List<Result> listItems = new List<Result>();
        private int appearingListItemIndex = 0;
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
            //Set up the floating Action Button
            //FloatingActionButtonAdd
            AbsoluteLayout.SetLayoutFlags(MyAbsolute, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(MyAbsolute, new Rectangle(0f, 0f, 1f, 1f));
            MyAbsolute.Children.Add(itemListView);
            FloatingActionButtonAdd.Clicked += (sender, e) =>
            {
                Goto_Add();
            };
            //Overlay the Fab Button
            AbsoluteLayout.SetLayoutFlags(FloatingActionButtonAdd, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(FloatingActionButtonAdd, new Rectangle(1f, 1f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            //Check if the List is Empty if so Always Show the Floating Action Button
            itemListView.Scrolled += OnCollectionViewScrolled;
            this.ToolbarItems.Add(item);
            item.IsEnabled = false;
            Timer t = new Timer(20000);
            t.AutoReset = true;
            t.Elapsed += new ElapsedEventHandler(Update_Tickers);
            t.Start();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            //Set the options to ensure Our Floating Action Button can disappear when not at the end of the list

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
                //await Shell.Current.GoToAsync($"{nameof(AddStock)}?{nameof()}");


            }
        }

        async void Delete_Stock(object sender, EventArgs e)
        {
            App.listItemsDisplay.Remove(selectedStock);
            await App.DataBase.DeleteStockAsync(selectedStock);
            item.IsEnabled = false;

        }

        public void Goto_Add()
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
                    double AmountStock = listItems[i].amount;
                    string sectorName = listItems[i].sector;
                    string CurrencyName = listItems[i].currency;
                    url = $"https://query1.finance.yahoo.com/v7/finance/quote?lang=en-US&region=US&corsDomain=finance.yahoo.com&symbols={symbolname}";
                    string jsonData = new WebClient().DownloadString(url);
                    Root newData = await Populate_Item(jsonData);
                    newData.quoteResponse.result[0].ID = IDNumber;
                    newData.quoteResponse.result[0].sector = sectorName;
                    newData.quoteResponse.result[0].amount = AmountStock;
                    //Calculate the new allocated price
                    newData.quoteResponse.result[0].allocated = await App.CurrencyConvertAsync(CurrencyName, App.Currency, newData.quoteResponse.result[0].ask) * AmountStock;
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

        async void OnCollectionViewScrolled(object sender, ScrolledEventArgs e)
        {

            List_ItemAppearing();
            Timer t2 = new Timer(4000);
            t2.AutoReset = false;
            t2.Elapsed += new ElapsedEventHandler(List_ItemDisappearing);
            t2.Start();

            // Custom logic
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

        //Function to Hide the Action Button when the List Item is disappearing
        void List_ItemDisappearing(Object source, ElapsedEventArgs e)
        {
            //Check the List has a minimum of 4 items and therefore hide the FAB
            if(listItems.Count >= 4)
            {
                FloatingActionButtonAdd.Hide();
            }
            
        }
        //Function to Show the Action Button when the List Item is Appearing
        void List_ItemAppearing()
        {
            FloatingActionButtonAdd.Show();

        }
    }
}
