using Microcharts;
using System.Collections.Generic;
using Xamarin.Forms.Xaml;
using Microcharts.Forms;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;

namespace test
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SectorInfo
    {
        public double value = 0;
        public List<Result> tempStockData = new List<Result>();
        public List<ChartEntry> entries = new List<ChartEntry>();
        public List<ChartEntry> entrieshistory = new List<ChartEntry>();
        private ObservableCollection<String> sectorNames = new ObservableCollection<String>();
        public List<double> TotalValue = new List<double>();
        public List<Double> AmountAllocated = new List<Double>();
        public SectorInfo()
        {
            InitializeComponent();
            OnAppearing();
            Chart1.Chart = new DonutChart() { Entries = entries, LabelTextSize=23 };
            Chart2.Chart = new LineChart() { Entries = entrieshistory, LineMode=LineMode.Spline, PointSize=0};



        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            //Reset portfolio value
            value = 0;
            entries.Clear();
            entrieshistory.Clear();
            AmountAllocated.Clear();
            App.history.Clear();
            sectorNames = App.SectorData.Retrieve_Items();
            //Try and populate the SectorData with Relevant Sectors
            //Iterate through the Items of listItemsDisplay
            int maxID = App.listItemsDisplay.Count;
            //Ensure all the Sector Index's are 0 before the calculation
            foreach (Sectors i in App.SectorData.sectordata)
            {
                i.ValueLabel = 0;
            }
            //Iterate across the Item List
            for (int i = 0; i < maxID; i++)
            {
                value += App.listItemsDisplay[i].allocated;
                string Symbol = App.listItemsDisplay[i].symbol;
                try
                {
                    App.Get_History(Symbol);
                    string SectorName = App.listItemsDisplay[i].sector;
                    int IndexinSectors = sectorNames.IndexOf(SectorName);
                    AmountAllocated.Add(App.listItemsDisplay[i].amount);
                    App.SectorData.sectordata[IndexinSectors].ValueLabel += (int)Math.Ceiling(App.listItemsDisplay[i].allocated);
                }
                catch
                {
                    DisplayAlert("Error", $"A stock in the potfolio named {Symbol} is returning null values and needs to be removed from the portfolio", "OK").ContinueWith(t =>
                    {
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
            foreach (Sectors i in App.SectorData.sectordata)
            {
                //Ensure the Sector dosent have a value of 0
                if (i.ValueLabel != 0)
                {
                    entries.Add(new ChartEntry(i.ValueLabel) { Label = i.Label, ValueLabel = i.ValueLabel.ToString(), Color = SKColor.Parse(i.Color), ValueLabelColor = SKColor.Parse(i.Color) });
                }


            }
            int iterator = 0;
            foreach(List<Double> i in App.history)
            {
                double allocate = AmountAllocated[iterator];
                if (iterator == 0)
                {//First stock add all values, second stock we add the values together and the allocations

                    var newValue = i.Select(r => r * allocate).ToList();
                    TotalValue = newValue;
                    iterator += 1;
                }
                else
                {
                    var newValue = i.Select(r => r * allocate).ToList();
                    TotalValue = TotalValue.Zip(newValue, (x, y) => x + y).ToList();
                    iterator += 1;
                }
            }
            iterator = 0;
                foreach (float close in TotalValue)
                {
                    entrieshistory.Add(new ChartEntry(close) { Label = Convert.ToString(iterator), Color= SKColor.Parse("000FFF") });
                    iterator = iterator + 1;

                }
            value = (int)Math.Ceiling(value);
            PortfolioValue2.Text = Convert.ToString(value) + " " + App.Currency;


        }
            
            


        }


        
    }
