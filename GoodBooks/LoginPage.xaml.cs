using static Android.Icu.Text.CaseMap;
using System.Security.Cryptography;

namespace GoodBooks;

public partial class LoginPage : ContentPage
{
    App globalref = (App)Application.Current;

    public LoginPage()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, true);
    }

    async void LoginButton_Clicked(System.Object sender, System.EventArgs e)
    {
        if (email.Text != "" && password.Text != "")
        {
            var result =
                from User user in globalref.allUsers
                where user.email.ToLower() == email.Text.ToLower()
                && user.password == password.Text
                select user;

            if (result.Any())
            {
                var matchedUser = result.FirstOrDefault();
                globalref.SignedIn = true;
                globalref.activeUser = matchedUser;

                var actBooks =
                    from Book book in globalref.bks
                    where book.seller == globalref.activeUser.userName
                    select book;
                List<Book> aBks = actBooks.ToList();
                foreach (var item in aBks)
                {
                    globalref.activeUserBooks.Add(item);
                }
                await DisplayAlert("Login successful", "", "Continue");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Input invalid", "Please enter a valid email address or password", "OK");
            }
        }
        else
        {
            await DisplayAlert("Input invalid", "Please enter a valid email address or password", "OK");
        }

    }

    private async void RegisterButton_Clicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new CreateAccountPage());
    }
}
