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
        public List<Result> tempStockData = new List<Result>();
        private ObservableCollection<String> sectorNames = new ObservableCollection<String>();
        public SectorInfo()
        {
            sectorNames = App.SectorData.Retrieve_Items();
            List<ChartEntry> entries = new List<ChartEntry>();
            //Try and populate the SectorData with Relevant Sectors
            //Iterate through the Items of listItemsDisplay
            int maxID = App.listItemsDisplay.Count;

            for (int i =0; i<maxID; i++)
            {
                string Symbol = App.listItemsDisplay[i].shortName;
                string sectorName = App.listItemsDisplay[i].sector;
                int IndexinSectors = sectorNames.IndexOf(sectorName);
                App.SectorData.sectordata[IndexinSectors].ValueLabel += 1;
            }
            foreach (Sectors i in App.SectorData.sectordata) {
                //Ensure the Sector dosent have a value of 0
                if (i.ValueLabel != 0) {
                    entries.Add(new ChartEntry(i.ValueLabel) { Label = i.Label, ValueLabel = i.ValueLabel.ToString(), Color = SKColor.Parse(i.Color) });
                }
                

            }
            InitializeComponent();
            Chart1.Chart = new DonutChart() { Entries = entries };
            
        }

        public async void Get_Data()
        {
            tempStockData = await App.DataBase.GetStocksAsync();
        }

        
    }
}