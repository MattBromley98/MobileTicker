using Microcharts;
using System.Collections.Generic;
using Xamarin.Forms.Xaml;
using Microcharts.Forms;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;

namespace test
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SectorInfo
    {
        public double value = 0;
        public List<Result> tempStockData = new List<Result>();
        public List<ChartEntry> entries = new List<ChartEntry>();
        private ObservableCollection<String> sectorNames = new ObservableCollection<String>();
        public SectorInfo()
        {
            InitializeComponent();
            OnAppearing();
            Chart1.Chart = new DonutChart() { Entries = entries };



        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //Reset portfolio value
            value = 0;
            entries.Clear();
            
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
                string Symbol = App.listItemsDisplay[i].shortName;
                string SectorName = App.listItemsDisplay[i].sector;
                int IndexinSectors = sectorNames.IndexOf(SectorName);
                App.SectorData.sectordata[IndexinSectors].ValueLabel += 1;
            }
            foreach (Sectors i in App.SectorData.sectordata)
            {
                //Ensure the Sector dosent have a value of 0
                if (i.ValueLabel != 0)
                {
                    entries.Add(new ChartEntry(i.ValueLabel) { Label = i.Label, ValueLabel = i.ValueLabel.ToString(), Color = SKColor.Parse(i.Color), ValueLabelColor = SKColor.Parse(i.Color) });
                }


            }
            
            
            PortfolioValue2.Text = Convert.ToString(value);

        }

        public async void Get_Data()
        {
            tempStockData = await App.DataBase.GetStocksAsync();
        }

        
    }
}