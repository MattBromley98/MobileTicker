using Microcharts;
using System.Collections.Generic;
using Xamarin.Forms.Xaml;
using Microcharts.Forms;
using SkiaSharp;

namespace test
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SectorInfo
    {
        public List<Result> tempStockData = new List<Result>();
        public SectorInfo()
        {
            List<ChartEntry> entries = new List<ChartEntry>();
            foreach (Sectors i in App.SectorData.sectordata) {
                entries.Add(new ChartEntry(i.ValueLabel) { Label = i.Label, ValueLabel=i.ValueLabel.ToString(), Color=SKColor.Parse("#FF1943") });

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