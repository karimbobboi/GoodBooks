using static Android.Telephony.CarrierConfigManager;

namespace GoodBooks;

public partial class SearchPage : ContentPage
{
    App globalref = (App)Application.Current;
    public SearchPage()
	{
		InitializeComponent();
        Shell.SetNavBarIsVisible(this, true);
        NavigationPage.SetHasNavigationBar(this, false);
        searchResults.ItemsSource = globalref.bks;
    }

    void SearchBar_TextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue;
        if (!string.IsNullOrEmpty(searchText))
        {
            var filteredBooks = globalref.bks.Where(book =>
                    book.name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    book.author.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    book.genre.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            searchResults.ItemsSource = filteredBooks;
        } else
        {
            searchResults.ItemsSource = globalref.bks;
        }
    }

    async void searchResults_ItemSelected(System.Object sender, Microsoft.Maui.Controls.SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
            return;
        var selectedBook = (Book)e.SelectedItem;
        ((ListView)sender).SelectedItem = null;
        await Navigation.PushAsync(new BookPage(selectedBook));
    }
}
