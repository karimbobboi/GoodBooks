using Android.Graphics;
using Android.Widget;
using Java.Lang;
using Org.Apache.Http.Authentication;
using static Android.Icu.Text.CaseMap;
using static Android.Telephony.CarrierConfigManager;
using static Java.Util.Concurrent.Flow;

namespace GoodBooks;


public partial class BookPage : ContentPage
{

	Book rose;
    readonly App globalref = (App)Application.Current;
    public BookPage(Book bk)
    {
        InitializeComponent();
        Shell.SetNavBarIsVisible(this, true);
        NavigationPage.SetHasNavigationBar(this, false);
        if (globalref.SignedIn)
        {
            globalref.AddToRecent(bk);
        }

        rose = bk;
        initLabels();
        backImg.BindingContext = rose;
        backImg.SetBinding(Image.SourceProperty, "images[0]");

        imgs.BindingContext = rose;
        imgs.SetBinding(CarouselView.ItemsSourceProperty, "images");
    }

    public void initLabels()
    {
        title.Text = rose.name;
        author.Text = rose.author;
        price.Text = "£" + rose.price.ToString();
        seller.Text = rose.seller;
        year.Text = rose.year.ToString();
        lang.Text = rose.language;
        genre.Text = rose.genre;
        publisher.Text = rose.publisher;
    }
}
