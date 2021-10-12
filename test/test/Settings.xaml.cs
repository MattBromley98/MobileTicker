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
		public Settings ()
		{
			Currencies.Add("USD");
			Currencies.Add("GBP");
			Currencies.Add("EUR");
			Currencies.Add("JPY");
			InitializeComponent ();
			CurrencyChoice.Choices = Currencies;
			CurrencyChoice.SelectedIndex = 0;
		}
		private void CurrencyChoice_OnSelectedIndexChanged(object sender, SelectedIndexChangedEventArgs e)
		{
			App.Currency = Currencies[e.Index];
			// Perform required operation
		}

	}
}