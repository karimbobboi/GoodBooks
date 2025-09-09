using IntelliJ.Lang.Annotations;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics.Text;
using Org.Apache.Http.Authentication;
using static System.Net.Mime.MediaTypeNames;
using static Android.InputMethodServices.Keyboard;
using static Android.Print.PrintAttributes;
using static Android.Provider.MediaStore;
using static Java.Util.Concurrent.Flow;

namespace GoodBooks;

public partial class AddBook : ContentPage
{
    App globalref = (App)Microsoft.Maui.Controls.Application.Current;
    List<string> imgPaths = new List<string>();
    string selectedLanguage = "";
    string selectedGenre = "";

    public AddBook()
	{
		InitializeComponent();
        Shell.SetNavBarIsVisible(this, true);
        NavigationPage.SetHasNavigationBar(this, false);

        mainAcctPage.BindingContext = globalref;
        InitialAcctPage.BindingContext = globalref;
        mainAcctPage.SetBinding(IsVisibleProperty, new Binding("SignedIn"));

        PopulateLanguagePicker();
        PopulateGenrePicker();
    }

    private void PopulateLanguagePicker()
    {
        string[] languages = { "Select language", "English", "Hausa", "Fulfulde", "German", "Mandarin", "Japanese", "Arabic", "Russian" };
        foreach (string language in languages)
        {
            bookLanguage.Items.Add(language);
        }
        bookLanguage.SelectedIndex = 0;
    }

    private void PopulateGenrePicker()
    {
        string[] genres = { "Select genre", "Drama", "Romance", "Sci-fi", "Self-help", "History", "Fantasy", "Horror"};
        foreach (string genre in genres)
        {
            bookGenre.Items.Add(genre);
        }
        bookGenre.SelectedIndex = 0;
    }

    void yearEntry_Completed(System.Object sender, System.EventArgs e)
    {
        int inputtedYear;
        string currYear = DateTime.Now.Year.ToString();
        int.TryParse(bookYear.Text, out inputtedYear);
        if (inputtedYear > 0)
        {
            
            if (!(inputtedYear <= DateTime.Now.Year))
            {
                DisplayAlert("Error", $"Please enter a valid year (between 1 and {currYear})", "OK");
                bookYear.Text = "";
            }
        }
        else
        {
            DisplayAlert("Error", "Please enter a valid year.", "OK");
            bookYear.Text = "";
        }
    }

    void languagePicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
    {
        if (bookLanguage.SelectedIndex > 0)
        {
            selectedLanguage = bookLanguage.Items[bookLanguage.SelectedIndex];
        }
    }

    void genrePicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
    {
        if (bookGenre.SelectedIndex > 0)
        {
            selectedGenre = bookGenre.Items[bookGenre.SelectedIndex];
        }
    }

    private async void SignInBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }

    private async void Img_Clicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Do you want to remove this image?", "", "Yes", "No");
        
        if (answer)
        {
            if (sender is ImageButton imgButton)
            {
                if (imgButton.Parent is Frame frame)
                {
                    imgPaths.Remove(imgButton.Source.ToString().Substring(6));
                    RemoveImage(frame);
                }
            }
        }
    }

    async void ImageButton_Clicked(System.Object sender, System.EventArgs e)
    {
        try {
            var photo = await MediaPicker.PickPhotoAsync();
            if (photo != null) {
                var PhotoPath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                using (var stream = await photo.OpenReadAsync())
                using (var newStream = File.OpenWrite(PhotoPath))
                {
                    await stream.CopyToAsync(newStream);
                }
                
                var image = new ImageButton
                {
                    Source = ImageSource.FromFile(PhotoPath),
                    HeightRequest = 100,
                    WidthRequest = 70,
                    Aspect = Aspect.AspectFit
                };
                image.Clicked += Img_Clicked;
                if (sender is Button button)
                {
                    if (button.Parent is Frame frame)
                    {
                        frame.Content = null;
                        frame.Content = image;
                        frame.BorderColor = Colors.Black;
                        frame.BackgroundColor = Colors.Transparent;

                        imgPaths.Add(PhotoPath);
                    }
                }
            }
        }
        catch (Exception ex) {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    async void AddButton_Clicked(System.Object sender, System.EventArgs e)
    {
        bool imgAdded = imgPaths.Any();
        if (imgAdded) {
            if (bookName.Text != "" && bookAuthor.Text != ""
                && bookPrice.Text != "" && bookPublisher.Text != ""
                && bookYear.Text != "" && selectedLanguage != ""
            && selectedGenre != "") {
                List<string> newImgs = imgPaths;
                Book bk = new Book(bookName.Text, bookAuthor.Text, selectedLanguage, bookPublisher.Text,
                            selectedGenre, newImgs, globalref.activeUser.userName,
                            Convert.ToInt32(bookYear.Text), Convert.ToDouble(bookPrice.Text));
                globalref.AddBook(bk);
                ClearAllFields();
                await DisplayAlert("Book added successfully", $"{bk.name} has been successfully listed on GoodBooks", "OK");
            }
            else {
                await DisplayAlert("Unsuccessful", "Please complete all fields", "OK");
            }
        }
        else{
            await DisplayAlert("Unsuccessful", "Please add at least one image", "OK");
        }
    }

    private void ClearAllFields() {
        bookName.Text = "";
        bookAuthor.Text = "";
        bookPrice.Text = "";
        bookPublisher.Text = "";
        bookYear.Text = "";
        selectedLanguage = "";
        selectedGenre = "";
        bookGenre.SelectedIndex = 0;
        bookLanguage.SelectedIndex = 0;

        imgPaths = new List<string>();
        RemoveImage(img1);
        RemoveImage(img2);
        RemoveImage(img3);
    }

    private void RemoveImage(Frame frame) {
        var button = new Button
        {
            HeightRequest = 50,
            WidthRequest = 50,
            CornerRadius = 25,
            HorizontalOptions = LayoutOptions.Center,
            Text = "+",
            FontSize = 35,
            Padding = 0,
            Margin = 0
        };
        button.Clicked += ImageButton_Clicked;
        frame.Content = null;
        frame.Content = button;
        frame.BorderColor = Colors.Transparent;
        frame.BackgroundColor = Colors.White;
    }
}
