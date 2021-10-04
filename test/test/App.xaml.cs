using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace test
{
    
    public partial class App : Application
    {
        public static List<Result> StockList = new List<Result>();
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
