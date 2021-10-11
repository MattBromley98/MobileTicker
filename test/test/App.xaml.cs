using System;
using System.IO;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using SkiaSharp;

namespace test
{
    
    public partial class App : Application
    {
        public static List<Result> StockList = new List<Result>();
        //List of the Sector Names
        public static ObservableCollection<Result> listItemsDisplay = new ObservableCollection<Result>();
        static stockdatabase Database;
        public static ListSectors SectorData = new ListSectors();
        //Create the database if not already constructed
        public static stockdatabase DataBase
        {
            get
            {
                if (Database == null)
                {
                    Database = new stockdatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "stocks.db3"));
                }
                return Database;
            }
        }        
        public App()
        {
            //Populate the List of Sectors
            SectorData.sectordata.Add(new Sectors { Label = "Air Travel", ValueLabel = 0, Color= "#2c3e50" });
            SectorData.sectordata.Add(new Sectors { Label = "Basic Materials", ValueLabel = 0, Color = "#7831d0" });
            SectorData.sectordata.Add(new Sectors { Label = "Communication Services", ValueLabel = 0, Color = "#3f3718" });
            SectorData.sectordata.Add(new Sectors { Label = "Conglomerates", ValueLabel = 0, Color = "#ac5a3a" });
            SectorData.sectordata.Add(new Sectors { Label = "Consumer Cyclical", ValueLabel = 0, Color = "#1973f0" });
            SectorData.sectordata.Add(new Sectors { Label = "Consumer Defensive", ValueLabel = 0, Color = "#12e687" });
            SectorData.sectordata.Add(new Sectors { Label = "Energy", ValueLabel = 0, Color = "#f63d87" });
            SectorData.sectordata.Add(new Sectors { Label = "Financial", ValueLabel = 0, Color = "#be2c25" });
            SectorData.sectordata.Add(new Sectors { Label = "Financial Services", ValueLabel = 0, Color = "#54715a" });
            SectorData.sectordata.Add(new Sectors { Label = "Healthcare", ValueLabel = 0, Color = "#2b0e5d" });
            SectorData.sectordata.Add(new Sectors { Label = "Industrial Goods", ValueLabel = 0, Color = "#d5d47d" });
            SectorData.sectordata.Add(new Sectors { Label = "Industrials", ValueLabel = 0, Color = "#44298e" });
            SectorData.sectordata.Add(new Sectors { Label = "Real Estate", ValueLabel = 0, Color = "#56c664" });
            SectorData.sectordata.Add(new Sectors { Label = "Services", ValueLabel = 0, Color = "#200e99" });
            SectorData.sectordata.Add(new Sectors { Label = "Technology", ValueLabel = 0, Color = "#31946e" });
            SectorData.sectordata.Add(new Sectors { Label = "Utilities", ValueLabel = 0, Color = "#dc7314" });

            InitializeComponent();
            XF.Material.Forms.Material.Init(this);
            MainPage = new AppShell();
            // MainPage = new NavigationPage(new test.MainPage());
        }


        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
