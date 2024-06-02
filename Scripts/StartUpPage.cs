using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Godot;
using System;


public partial class StartUpPage : Control
{
	IFirebaseConfig config = new FirebaseConfig
	{
		AuthSecret = "sl5J6RLP0fMsh6OJNNj978xIelPyaSCuwr6hOf8R",
		BasePath = "https://carp-70436-default-rtdb.asia-southeast1.firebasedatabase.app/",
	};

	IFirebaseClient client;
	VBoxContainer createAccount;
    VBoxContainer loginAccount;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		client = new FireSharp.FirebaseClient(config);
		
		createAccount = GetNode<VBoxContainer>("CreateAccount");
        createAccount.Visible = false;
        loginAccount = GetNode<VBoxContainer>("Login");
        loginAccount.Visible = true;
		var registerButton = GetNode<Button>("Login/register");
		registerButton.Connect("pressed", new Callable(this, nameof(RegisterPressed)));
		
		var loginButton = GetNode<Button>("Login/login");
		loginButton.Connect("pressed", new Callable(this, nameof(LogInPressed)));
		
		var createButton = GetNode<Button>("CreateAccount/create");
		createButton.Connect("pressed", new Callable(this, nameof(CreatePressed)));
        var cancelButton = GetNode<Button>("CreateAccount/cancel");
        cancelButton.Connect("pressed", new Callable(this, nameof(CancelPressed)));
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
		String username = GetNode<LineEdit>("Login/username").Text;
		String password = GetNode<LineEdit>("Login/password").Text;

		FirebaseResponse response = await client.GetAsync("Users/" + username);
		UserData userObj = response.ResultAs<UserData>();

		if (userObj != null)
		{
			if (password.Equals(userObj.Password))
			{
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
			Username = GetNode<LineEdit>("CreateAccount/cusername").Text,
			Password = GetNode<LineEdit>("CreateAccount/cusername").Text,
			Firstname = GetNode<LineEdit>("CreateAccount/HBoxContainer/firstname").Text,
			Lastname = GetNode<LineEdit>("CreateAccount/HBoxContainer/lastname").Text,
			Subscription = "NONE",
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

			GD.Print("User Inserted " + result.Username);
		}
	}
	private void CancelPressed()
	{
        loginAccount.Visible = true;

        createAccount.Visible = false;

    }
}

