using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Godot;
using System;
using System.IO;

public partial class StartUpPage : Control
{
    #region Login and Create Details
    [Export] private LineEdit username;
    [Export] private LineEdit password;
    [Export] private LineEdit firstname;
    [Export] private LineEdit lastname;

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

    IFirebaseConfig config = new FirebaseConfig
	{
		AuthSecret = "sl5J6RLP0fMsh6OJNNj978xIelPyaSCuwr6hOf8R",
		BasePath = "https://carp-70436-default-rtdb.asia-southeast1.firebasedatabase.app/",
	};

	IFirebaseClient client;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		client = new FireSharp.FirebaseClient(config);
		
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
	
	}
	
	private void RegisterPressed()
	{
        loginAccount.Visible = false;

        createAccount.Visible = true;
	}
	private async void LogInPressed()
	{
		String username = loginUsername.Text;
		String password = logInPassword.Text;

		FirebaseResponse response = await client.GetAsync("Users/" + username);
		UserData userObj = response.ResultAs<UserData>();

		if (userObj != null)
		{
			if (password.Equals(userObj.Password))
			{
				AccountManager.SetUser(userObj);
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
	private async void CreatePressed()
	{
		var data = new UserData
		{
			Username = username.Text,
			Password = password.Text,
			Firstname = firstname.Text,
			Lastname = lastname.Text,
			Subscription = "NONE",
			Role = rolePick.Selected.ToString(),
			ProfileImage = "NONE"
        };

		if (data.Role.Equals("1"))
		{
			data.Role = "Teacher";
		}
		else
		{
            data.Role = "Student";
        }

		FirebaseResponse getresponse = await client.GetAsync("Users/" + GetNode<LineEdit>("CreateAccount/cusername").Text);
		UserData userObj = getresponse.ResultAs<UserData>();
		if (userObj != null)
		{
            notification.MessageBox("User already exists...", 1);
        }
		else if (data.Password.Length < 6)
		{
            notification.MessageBox("Password must be 6 characters or more...", 1);
		}
		else
		{
			SetResponse response = await client.SetAsync("Users/" + GetNode<LineEdit>("CreateAccount/cusername").Text, data);
			UserData result = response.ResultAs<UserData>();

			createAccount.Visible = false;
			loginAccount.Visible = true;

            notification.MessageBox("Account Created Successfully \n Welcome " + result.Username, 0);
            AccountManager.SetUser(result);
        }
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

