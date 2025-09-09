namespace GoodBooks;

public partial class MainPage : ContentPage
{
    App globalref = (App)Application.Current;

    public MainPage()
	{
		InitializeComponent();
        Shell.SetNavBarIsVisible(this, true);
        NavigationPage.SetHasNavigationBar(this, false);

        all.ItemsSource = globalref.bks;
        popular.ItemsSource = globalref.bks;
        signout.BindingContext = globalref;
        signout.SetBinding(IsVisibleProperty, new Binding("SignedIn"));

        var result =
                from Book book in globalref.bks
                where book.genre == "Comedy"
                select book;
        genre.ItemsSource = result;
    }

	async private void OnCounterClicked(object sender, EventArgs e)
	{
        await Navigation.PushAsync(new BookPage(globalref.bks[1]));
    }

    async void ImageButton_Clicked(System.Object sender, System.EventArgs e)
    {
        Book book = (Book)all.CurrentItem;
        if (book != null)
        {
           await Navigation.PushAsync(new BookPage(book));
        }
    }

    async void SignIn_Clicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }

    async void SignOut_Clicked(System.Object sender, System.EventArgs e)
    {
        globalref.SignedIn = false;
        globalref.activeUser = new User();
        await DisplayAlert("Logged out", "", "OK");
    }
}


