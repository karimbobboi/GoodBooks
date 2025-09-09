using System.Globalization;
using Microsoft.Maui.Controls;
using static Android.Telephony.CarrierConfigManager;
using System.Collections.Specialized;

namespace GoodBooks;

public partial class AccountPage : ContentPage
{
    readonly App globalref = (App)Application.Current;

    public AccountPage()
	{
		InitializeComponent();
        mainAcctPage.BindingContext = globalref;
        InitialAcctPage.BindingContext = globalref;
        mainAcctPage.SetBinding(IsVisibleProperty, new Binding("SignedIn"));

        listings.ItemsSource = globalref.activeUserBooks;
        recents.ItemsSource = globalref.recentBks;
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, true);
    }

    private async void SignInBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }

    private void InBtn_Clicked(object sender, EventArgs e)
    {
        globalref.activeUser = globalref.allUsers[1];
        globalref.SignedIn = true;
        InBtn.IsVisible = false;
        System.Diagnostics.Debug.WriteLine(globalref.activeUser.userName);
    }

    async void SeeAllBtn_Clicked(System.Object sender, System.EventArgs e)
    {
        if (listings.ItemsSource != null)
        {
            await Navigation.PushAsync(new AllBooks());
        }
        
    }

    async void LogOutBtn_Clicked(System.Object sender, System.EventArgs e)
    {
        globalref.SignedIn = false;
        globalref.activeUser = new User();
        InBtn.IsVisible = true;
        await DisplayAlert("Logged out", "", "OK");
    }
}

public class AccountItems
{
    public string name { get; set; }
    public string icon { get; set; }

    public AccountItems(string n)
    {
        name = n;
    }
}
