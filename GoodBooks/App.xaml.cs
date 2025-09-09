using System.Collections.ObjectModel;
using System.ComponentModel;
using Android.App.AppSearch;
using IntelliJ.Lang.Annotations;
using static Android.Provider.ContactsContract.CommonDataKinds;

namespace GoodBooks;

public class Book
{
    public string name { get; set; }
    public string author { get; set; }
    public string language { get; set; }
    public string publisher { get; set; }
    public string genre { get; set; }
    public string seller { get; set; }
    public List<string> images { get; set; }
    public string mainImage { get; set; }

    public int year { get; set; }
    public double price { get; set; }

    public Book(string _name, string _author, string _language, string _publisher,
                string _genre, List<string> _images, string _seller, int _year, double _price)
    {
        name = _name;
        author = _author;
        language = _language;
        publisher = _publisher;
        genre = _genre;
        seller = _seller;
        images = _images;
        mainImage = images[0];

        year = _year;
        price = _price;
    }

}

public class User
{
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string userName { get; set; }
    public string email { get; set; }
    public string password { get; set; }

    public User(string fname, string lname, string usrname, string userEmail, string pass)
    {
        firstName = fname;
        lastName = lname;
        userName = usrname;
        email = userEmail;
        password = pass;
    }

    public User()
    {
        firstName = "";
        lastName = "";
        email = "";
        password = "";
    }

    public void AddUser(User newUsr)
    {
        App globalPage = (App)Application.Current;
        globalPage.allUsers.Add(newUsr);
    }
}


public partial class App : Application, INotifyPropertyChanged
{
    public ObservableCollection<Book> bks = MakeBookDB();
    public ObservableCollection<User> allUsers = MakeUserDB();
    public ObservableCollection<Book> recentBks = new ObservableCollection<Book>();
    public ObservableCollection<Book> activeUserBooks = new ObservableCollection<Book>();
    public User activeUser = new User();
    public App()
	{
		InitializeComponent();

        MainPage = new NavigationPage(new AppShell());
	}

    private bool _signedIn;
    public bool SignedIn
    {
        get { return _signedIn; }
        set
        {
            if (_signedIn != value)
            {
                _signedIn = value;
                OnPropertyChanged(nameof(SignedIn));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public static ObservableCollection<Book> MakeBookDB() {
        ObservableCollection<Book> temp = new ObservableCollection<Book>();
        List<string> img = new List<string>() {
            "name_of_the_rose.jpg", "crime_and_punishment.jpg", "one_hundred_years.jpg"
        };
        Book bk = new Book("The Name of The Rose", "Umberto Eco", "English", "Penguin",
                    "Comedy", img, "karimbobboi", 1965, 30.5);
        temp.Add(bk);

        List<string> img2 = new List<string>() {
            "one_hundred_years.jpg", "name_of_the_rose.jpg", "one_hundred_years.jpg"
        };
        bk = new Book("One Hundred Years of Solitude", "Gabriel Garcia Marquez", "Spanish", "Penguin",
                    "Comedy", img2, "karimbobboi", 1977, 50.5);
        temp.Add(bk);

        List<string> img3 = new List<string>() {
            "crime_and_punishment.jpg", "name_of_the_rose.jpg", "one_hundred_years.jpg"
        };
        bk = new Book("Crime and Punishment", "Fyodor Dostoevsky", "Russian", "Penguin",
                    "Horror", img3, "karimbobboi", 1905, 10.5);
        temp.Add(bk);
        return temp;
    }

    public static ObservableCollection<User> MakeUserDB()
    {
        ObservableCollection<User> temp = new ObservableCollection<User>();
        User usr = new User
        {
            firstName = "Roy",
            lastName = "Tillman",
            userName = "rTillman",
            email = "RT@USA.com",
            password = "Us@001"
        };
        temp.Add(usr);

        usr = new User
        {
            firstName = "Karim",
            lastName = "Bobboi",
            userName = "karimbobboi",
            email = "KB@Buck.com",
            password = "Us@001"
        };
        temp.Add(usr);

        usr = new User
        {
            firstName = "Place",
            lastName = "Holder",
            userName = "pholder",
            email = "temp",
            password = "tt"
        };
        temp.Add(usr);

        usr = new User
        {
            firstName = "Adso",
            lastName = "Melk",
            userName = "adsoMelk",
            email = "Adso@Melk.com",
            password = "Us"
        };
        temp.Add(usr);
        return temp;
    }

    public void AddBook(Book newBook)
    {
        App globalPage = (App)Application.Current;
        globalPage.bks.Add(newBook);
        System.Diagnostics.Debug.WriteLine(newBook.images[0]);
    }

    public bool RemoveBook(string newBook)
    {
        var actBooks =
                    (from Book book in activeUserBooks
                    where book.seller == activeUser.userName
                    && book.name == newBook
                    select book).ToList();
        Book bk = actBooks.FirstOrDefault();

        if (bk != null)
        {
            activeUserBooks.Remove(bk);
            bks.Remove(bk);
            recentBks.Remove(bk);
            return true;
        }
        else {
            return false;
        }
    }

    public void AddUser(User newUser)
    {
        App globalPage = (App)Application.Current;
        globalPage.allUsers.Add(newUser);
    }

    public void AddToRecent(Book bk) {
        
        if (bk != null)
        {
            var filteredBooks = recentBks.Where(book =>
                    book.name == bk.name && book.author == bk.author &&
                    book.publisher == bk.publisher && book.year == bk.year &&
                    book.language == bk.language && book.seller == bk.seller
            ).ToList();
            if (!(filteredBooks.Any()))
            {
                if (recentBks.Count > 4) {
                    recentBks.RemoveAt(4);
                }
                recentBks.Insert(0, bk);
            }
            else {
                int idx = recentBks.IndexOf(bk);
                recentBks.Move(idx, 0);
            }
        }
    }
}
