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
using System.Collections.ObjectModel;

namespace test
{
    [QueryProperty(nameof(inputId), nameof(inputId))]
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class AddStock : ContentPage
    {
        public bool isBusy = false;
        private ObservableCollection<String> sectorNames = new ObservableCollection<String>();
        public AddStock()
        {

            InitializeComponent();
            sectorNames = App.SectorData.Retrieve_Items();
            Type.Choices = sectorNames;
            //Add a list of sectors for the user to choose

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
            try
            {
                if (isBusy == false)
                {
                    isBusy = true;
                    string url = "";
                    string StockName = "";
                    double amount = 0;
                    double bep = 0;
                    //What to execute when the Add Button is pressed on the AddStock page
                    StockName = Ticker.Text;
                    try {

                        amount = Convert.ToDouble(Amount.Text);
                        bep = Convert.ToDouble(Breakeven.Text);
                    }
                    catch
                    {
                        await DisplayAlert("Error", "Please ensure the Amount and Breakeven Price entered is numeric", "OK");
                        IsBusy = false;
                        return;
                    }
                    url = $"https://query1.finance.yahoo.com/v7/finance/quote?lang=en-US&region=US&corsDomain=finance.yahoo.com&symbols={StockName}";

                    string jsonData = new WebClient().DownloadString(url);
                    
                    Root newStock = await Populate_Item(jsonData);
                    newStock.quoteResponse.result[0].amount = amount;
                    //Define 3 Methods for Different Currencies
                    newStock.quoteResponse.result[0].allocated = await App.CurrencyConvertAsync(newStock.quoteResponse.result[0].currency, "USD", newStock.quoteResponse.result[0].ask) * amount;
                    newStock.quoteResponse.result[0].bep = bep;
                    if (newStock.quoteResponse.result[0].change > 0)
                    {
                        newStock.quoteResponse.result[0].color = "Green";
                    }
                    else
                    {
                        newStock.quoteResponse.result[0].color = "Red";
                    }
                    //Find the Chosen Sector
                    try
                    {
                        await Navigation.PopAsync();
                        await App.DataBase.SaveStockAsync(newStock.quoteResponse.result[0]);
                        App.listItemsDisplay.Add(newStock.quoteResponse.result[0]);
                        isBusy = false;
                        await Navigation.PushAsync(new MainPage());
                    }
                    catch
                    {
                        await DisplayAlert("Sector Name","Please ensure to select a sector name","OK");
                        await Navigation.PopAsync();
                        isBusy = false;
                        await Navigation.PushAsync(new MainPage());
                    }


                }
            }
            catch
            {
                //If stock cant be added show a Error Message
                await DisplayAlert("Error adding Symbol", "This ticker symbol can not be found from Yahoo Finance", "OK");
                isBusy = false;
            }
        }
        async Task<Root> Populate_Item(string jsonData)
        {
            Root inStock = JsonConvert.DeserializeObject<Root>(jsonData);
            inStock.quoteResponse.result[0].change = ((inStock.quoteResponse.result[0].ask - inStock.quoteResponse.result[0].regularMarketPreviousClose) / inStock.quoteResponse.result[0].ask) * 100;
            inStock.quoteResponse.result[0].sector = Type.Text;
            if (inStock.quoteResponse.result[0].change <= -50)
            {
                inStock.quoteResponse.result[0].change = 0;
                inStock.quoteResponse.result[0].ask = inStock.quoteResponse.result[0].regularMarketPreviousClose;
            }
            string priceinString = inStock.quoteResponse.result[0].ask.ToString();
            inStock.quoteResponse.result[0].outputPrice = priceinString + " " + inStock.quoteResponse.result[0].currency;
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