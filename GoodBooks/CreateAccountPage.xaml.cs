namespace GoodBooks;

public partial class CreateAccountPage : ContentPage
{
    bool email_valid = false, password_valid = false;
    App globalref = (App)Application.Current;

    public CreateAccountPage()
	{
		InitializeComponent();
	}

    private async void RegisterCompleteBtn_Clicked(System.Object sender, System.EventArgs e)
    {
        if (fname.Text != "" && lname.Text != "" &&
            email.Text != "" && password.Text != "" && username.Text != "") {

            var result =
                from User user in globalref.allUsers
                where user.email.ToLower() == email.Text.ToLower()
                || user.userName == username.Text
                select user;

            var userItem = result.FirstOrDefault();
            if (userItem == null)
            {
                validate_email(email.Text);
                validate_password(password.Text);
                if (email_valid && password_valid)
                {
                    User newUsr = new User(fname.Text, lname.Text, username.Text, email.Text, password.Text);
                    globalref.AddUser(newUsr);
                    await DisplayAlert("Account created", $"Welcome to GoodBooks, {username.Text}", "OK");
                    await Navigation.PopAsync();
                }
            }
            else
            {
                if (userItem.userName == username.Text)
                {
                    await DisplayAlert("Username is already taken", "", "OK");
                }
                else if (userItem.email == email.Text)
                {
                    await DisplayAlert("Email is taken", "There is already a GoodBooks account registered with this email address.", "OK");
                }
            }
        }
        else {
            await DisplayAlert("Input invalid", "Please fill all fields", "OK");
        }
    }

    async void validate_email(string eml)
    {
        if (Char.IsLetterOrDigit(eml[0]) && eml.Contains('@') && (eml.Contains(".com") || eml.Contains(".co.uk")))
        {
            email_valid = true;
        }
        else
        {
            email_valid = false;
            await DisplayAlert("Input invalid", "Please input a valid email address", "OK");
        }
    }

    async void validate_password(string prd)
    {
        bool upper = false, lower = false, digit = false, sym = false;
        for (int i = 0; i < prd.Length; i++)
        {
            if (Char.IsUpper(prd[i]))
            {
                upper = true;
                continue;
            }
            if (Char.IsLower(prd[i]))
            {
                lower = true;
                continue;
            }
            if (Char.IsDigit(prd[i]))
            {
                digit = true;
                continue;
            }
            if (Char.IsSymbol(prd[i]) || prd[i] == '@')
            {
                sym = true;
                continue;
            }
        }
        if (upper && lower && digit && sym)
        {
            password_valid = true;
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("fail " + prd);
            password_valid = false;
            await DisplayAlert("Input invalid",
                "Password must include atleast 1 uppercase, atleast 1 lowercase, atleast 1 number and atleast 1 symbol",
                "OK");
        }
    }
}
