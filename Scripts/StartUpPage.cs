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
    #endregion
    #region Buttons
    [Export] private Button registerBtn;
    [Export] private Button loginBtn;
    [Export] private Button createBtn;
    [Export] private Button cancelBtn;

    [Export] private TextureButton backBtn;
    #endregion

    #region PackedScenes
    [Export] private PackedScene mainPage;
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
		if (Input.IsKeyPressed(Key.Enter))
		{
            LogInPressed();
        }
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
				Node simultaneousScene = ResourceLoader.Load<PackedScene>("res://Scenes/main_page.tscn").Instantiate();
				GetTree().Root.AddChild(simultaneousScene);
				Hide();
			}
			else
			{
				GD.Print("Wrong Password!!!");
			}
		}
		else
		{
			GD.Print("User Does Not Exist!!!");
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
			Role = rolePick.Selected.ToString()
        };
		FirebaseResponse getresponse = await client.GetAsync("Users/" + GetNode<LineEdit>("CreateAccount/cusername").Text);
		UserData userObj = getresponse.ResultAs<UserData>();
		if (userObj != null)
		{
			GD.Print("User already exists!!!");
		}
		else if (data.Password.Length < 6)
		{
			GD.Print("Password must be 6 characters or more!!!");
		}
		else
		{
			SetResponse response = await client.SetAsync("Users/" + GetNode<LineEdit>("CreateAccount/cusername").Text, data);
			UserData result = response.ResultAs<UserData>();

			createAccount.Visible = false;
			loginAccount.Visible = true;
            GD.Print("User Inserted " + result.Username);
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
        Node simultaneousScene = mainPage.Instantiate();
        GetTree().Root.AddChild(simultaneousScene);
    }
}

