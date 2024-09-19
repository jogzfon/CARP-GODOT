using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Godot;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

public partial class StartUpPage : Control
{
    #region Login and Create Details
    [Export] private LineEdit username;
    [Export] private LineEdit password;
    [Export] private LineEdit confirm_password;
    [Export] private LineEdit firstname;
    [Export] private LineEdit lastname;
    [Export] private LineEdit email;

    [Export] private OptionButton rolePick;

    [Export] private LineEdit loginUsername;
	[Export] private LineEdit logInPassword;

    [Export] private VBoxContainer createAccount;
    [Export] private VBoxContainer loginAccount;

    [Export] private NotificationHandler notification;
    #endregion
    #region Buttons
    [Export] private Button registerBtn;
    [Export] private Button loginBtn;
    [Export] private Button createBtn;
    [Export] private Button cancelBtn;

    [Export] private TextureButton backBtn;
    #endregion

/*    IFirebaseConfig config = new FirebaseConfig
	{
		AuthSecret = "sl5J6RLP0fMsh6OJNNj978xIelPyaSCuwr6hOf8R",
		BasePath = "https://carp-70436-default-rtdb.asia-southeast1.firebasedatabase.app/",
	};*/

	IFirebaseClient client;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        createAccount.Visible = false;
        loginAccount.Visible = true;

		registerBtn.Connect("pressed", new Callable(this, nameof(RegisterPressed)));
		
		loginBtn.Connect("pressed", new Callable(this, nameof(LogInPressed)));
		
		createBtn.Connect("pressed", new Callable(this, nameof(CreatePressed)));

        cancelBtn.Connect("pressed", new Callable(this, nameof(CancelPressed)));

        backBtn.Connect("pressed", new Callable(this, nameof(BackPressed)));
    }
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (client == null)
        {
            //client = new FireSharp.FirebaseClient(config);
            client = Connector.ConnectToClient();
        }
    }
	
	private void RegisterPressed()
	{
        loginAccount.Visible = false;

        createAccount.Visible = true;
	}
	private async void LogInPressed()
	{
        if (client == null)
        {
            notification.MessageBox("You are offline, try logging in after turning back online.", 1);
        }
        else
        {
            String username = loginUsername.Text;
            String password = logInPassword.Text;

            FirebaseResponse response = await client.GetAsync("Users/" + username);
            UserData userObj = response.ResultAs<UserData>();

            if (userObj != null)
            {
                if(userObj.Status == "Endorsed")
                {
                    notification.MessageBox("Account waiting for approval...", 0);
                    return;
                }else if(userObj.Status == "Declined")
                {
                    notification.MessageBox("Account request has been declined.\n Please contact the admin for more info.", 1);
                    return;
                }

                if (password.Equals(userObj.Password))
                {
                    AccountManager.SetUser(userObj);
                    //AccountFileSaver
                    AccountFileSaver.SaveAccount(userObj);
                    this.QueueFree();
                }
                else
                {
                    notification.MessageBox("Wrong Password!", 1);
                }
            }
            else
            {
                notification.MessageBox("User Does Not Exist!", 1);
            }
        }
	}
	private async void CreatePressed()
	{
        if(client == null)
        {
            notification.MessageBox("You are offline, try logging in after turning back online.", 1);
            return;
        }
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if(!Regex.IsMatch(email.Text, emailPattern))
        {
            notification.MessageBox("Invalid email format!", 1);
            return;
        }
        if (!PasswordStrengthChecker(password.Text))
        {
            return;
        }
        if(password.Text != confirm_password.Text)
        {
            notification.MessageBox("The confirmation password does not match the original password.\nPlease ensure both passwords are identical and try again.", 1);
            return;
        }
        if (!IsUsernameValid(username.Text))
        {
            return;
        }
        var data = new UserData
        {
            Username = username.Text,
            Password = password.Text,
            Firstname = firstname.Text,
            Lastname = lastname.Text,
            Email = email.Text,
            Status = "",
            Subscription = "No Subscription",
            Role = rolePick.Selected.ToString(),
            SubscriptionStart = "",
            SubscriptionEnd = "",
            ProfileImage = "NONE",
            ProofOfPayment = "NONE"
        };

		if (data.Role.Equals("1"))
		{
			data.Role = "Teacher";
            data.Status = "Endorsed";
		}
		else
		{
            data.Role = "Student";
            data.Status = "Accepted";
        }

		FirebaseResponse getresponse = await client.GetAsync("Users/" + GetNode<LineEdit>("CreateAccount/cusername").Text);
		UserData userObj = getresponse.ResultAs<UserData>();
		if (userObj != null)
		{
            notification.MessageBox("User already exists...", 1);
            return;
        }
		else if (data.Password.Length < 6)
		{
            notification.MessageBox("Password must be 6 characters or more...", 1);
            return;
        }
		else
		{
			SetResponse response = await client.SetAsync("Users/" + GetNode<LineEdit>("CreateAccount/cusername").Text, data);
			UserData result = response.ResultAs<UserData>();

			createAccount.Visible = false;
			loginAccount.Visible = true;

            notification.MessageBox("Account Created Successfully \n Welcome " + result.Username, 0);

            username.Text = string.Empty;
            password.Text = string.Empty;
            firstname.Text = string.Empty;
            lastname.Text = string.Empty;
            email.Text = string.Empty;
        }
	}
    private bool IsUsernameValid(string username)
    {
        bool isValid = true;

        // Check for minimum length of 3 characters
        if (username.Length < 5)
        {
            notification.MessageBox("Username must be at least 5 characters long.",1);
            isValid = false;
        }

        // Check for allowed characters (alphanumeric and underscores only)
        if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_]+$"))
        {
            notification.MessageBox("Username can only contain letters, numbers, and underscores.",1);
            isValid = false;
        }

        // Check for spaces (should not contain spaces)
        if (username.Contains(" "))
        {
            notification.MessageBox("Username cannot contain spaces.",1);
            isValid = false;
        }

        return isValid;
    }
    private bool PasswordStrengthChecker(string password)
    {
        bool isStrong = true;

        // Check for minimum length of 8 characters
        if (password.Length < 6)
        {
            notification.MessageBox("Password must be at least 6 characters long.", 1);
            isStrong = false;
        }

        // Check for at least one lowercase letter
        if (!Regex.IsMatch(password, @"[a-z]"))
        {
            notification.MessageBox("Password must contain at least one lowercase letter.",1);
            isStrong = false;
        }

        // Check for at least one uppercase letter
        if (!Regex.IsMatch(password, @"[A-Z]"))
        {
            notification.MessageBox("Password must contain at least one uppercase letter.", 1);
            isStrong = false;
        }

        // Check for at least one digit
        if (!Regex.IsMatch(password, @"\d"))
        {
            notification.MessageBox("Password must contain at least one digit.",1);
            isStrong = false;
        }

        // Check for at least one special character
        if (!Regex.IsMatch(password, @"[@$!%*?&]"))
        {
            notification.MessageBox("Password must contain at least one special character (@$!%*?&).", 1);
            isStrong = false;
        }

        return isStrong;
    }

    private void CancelPressed()
	{
        loginAccount.Visible = true;

        createAccount.Visible = false;
    }

	private void BackPressed()
	{
        this.QueueFree();
    }
}

