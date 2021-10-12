using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI;

namespace test
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Settings : ContentPage
	{
		public ObservableCollection<String> Currencies = new ObservableCollection<String>();
		public ObservableCollection<String> Timeframes = new ObservableCollection<string>();
		public ObservableCollection<String> Intervals = new ObservableCollection<string>();
		public Settings ()
		{
			//Add Currency Selections
			Currencies.Add("USD");
			Currencies.Add("GBP");
			Currencies.Add("EUR");
			Currencies.Add("JPY");
			//Add Interval Selections
			Intervals.Add("1d");
			Intervals.Add("5d");
			Intervals.Add("1wk");
			Intervals.Add("1mo");
			//Add Timeframe Selections
			Timeframes.Add("5d");
			Timeframes.Add("1wk");
			Timeframes.Add("3wk");
			Timeframes.Add("1mo");
			Timeframes.Add("3mo");
			Timeframes.Add("1y");
			Timeframes.Add("2y");
			Timeframes.Add("3y");
			InitializeComponent ();
			CurrencyChoice.Choices = Currencies;
			Timeframe.Choices = Timeframes;
			Interval.Choices = Intervals;
			CurrencyChoice.SelectedIndex = 0;
		}
		private void CurrencyChoice_OnSelectedIndexChanged(object sender, SelectedIndexChangedEventArgs e)
		{
			App.Currency = Currencies[e.Index];
			// Perform required operation
		}

		private void Timeframe_OnSelectedIndexChanged(object sender, SelectedIndexChangedEventArgs e)
		{
			App.TimeFrame = Timeframes[e.Index];
			// Perform required operation
		}

		private void Interval_OnSelectedIndexChanged(object sender, SelectedIndexChangedEventArgs e)
		{
			App.Interval = Intervals[e.Index];
			// Perform required operation
		}

	}
}