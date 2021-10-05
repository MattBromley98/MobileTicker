using System;
using System.IO;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace test
{
    
    public partial class App : Application
    {
        public static List<Result> StockList = new List<Result>();
        static stockdatabase Database;
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
            
            InitializeComponent();

            MainPage = new NavigationPage(new test.MainPage());
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
