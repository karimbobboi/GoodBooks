using System;
using Microsoft.Maui.Controls;

namespace GoodBooks;

public partial class AllBooks : ContentPage
{
    App globalref = (App)Application.Current;
    public AllBooks()
	{
		InitializeComponent();
        Shell.SetNavBarIsVisible(this, true);
        NavigationPage.SetHasNavigationBar(this, false);

        booksList.ItemsSource = globalref.activeUserBooks;
    }

    async void RemoveBtn_Clicked(System.Object sender, System.EventArgs e)
    {
        Button button = (Button)sender;
        HorizontalStackLayout hstack = (HorizontalStackLayout)button.Parent;

        Grid grid = (Grid)hstack.Parent;
        Label bkName = (Label)grid.ElementAt(1);
        bool answer = await DisplayAlert($"Remove {bkName.Text}?", "Are you sure you want to delete and unlist this book?", "Yes", "No");
        if (answer)
        {
            bool res = globalref.RemoveBook(bkName.Text);
            if (res)
            {
                await DisplayAlert($"{bkName.Text} successfully removed", "", "OK");
            }
            else {
                await DisplayAlert("An error occured", "", "OK");
            }
        }

    }
}
