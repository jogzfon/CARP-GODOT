using Godot;
using OpenAI_API.Files;
using System;
using System.Collections.Generic;
using System.IO;

public partial class main_page : Control
{
    [Export] private Button signIn;

    [Export] private VBoxContainer options;

    [Export] private Button logout;
    [Export] private Button settings;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        options.Hide();

        UserData user = AccountManager.GetUser();
        if (user != null)
        {
            signIn.Text = user.Username;
            signIn.Connect("pressed", new Callable(this, nameof(Options)));

            logout.Connect("pressed", new Callable(this, nameof(Logout)));
            //settings.Connect("pressed", new Callable(this, nameof(Options)));
        }
        else
        {
            signIn.Text = "Sign-In";
            signIn.Connect("pressed", new Callable(this, nameof(SignIn)));
        }
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        
    }
    private void SignIn()
    {
        Node simultaneousScene = ResourceLoader.Load<PackedScene>("res://Scenes/StartUpPage.tscn").Instantiate();
        GetTree().Root.AddChild(simultaneousScene);
        Hide();
    }
    private void Options()
    {
        if (options.Visible)
        {
            options.Visible = false;
        }
        else
        {
            options.Visible = true;
        }
    }
    private void Logout()
    {
        options.Visible = false;
        AccountManager.SetUser(null);
        signIn.Text = "Sign-In";
        signIn.Disconnect("pressed", new Callable(this, nameof(Options)));
        signIn.Connect("pressed", new Callable(this, nameof(SignIn)));
    }
}
