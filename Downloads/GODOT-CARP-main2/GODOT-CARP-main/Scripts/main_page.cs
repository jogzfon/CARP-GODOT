using Godot;
using OpenAI_API.Files;
using System;
using System.Collections.Generic;
using System.IO;

public partial class main_page : Control
{
    [Export] private Button signIn;
    [Export] private Button signOut;

    [Export] private VBoxContainer options;

    [Export] private Button logout;
    [Export] private Button settings;

    [Export] PackedScene logInPage;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        options.Hide();
        signIn.Connect("pressed", new Callable(this, nameof(SignIn)));
        signOut.Connect("pressed", new Callable(this, nameof(Options)));

        logout.Connect("pressed", new Callable(this, nameof(Logout)));
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        UserData user = AccountManager.GetUser();
        if (user != null)
        {
            signIn.Hide();
            signOut.Show();
            signOut.Text = user.Username;
            //settings.Connect("pressed", new Callable(this, nameof(Options)));
        }
        else
        {
            signOut.Hide();
            signIn.Show();
            signIn.Text = "Sign-In";
        }
    }
    private void SignIn()
    {
        Node simultaneous = logInPage.Instantiate();
        GetTree().Root.AddChild(simultaneous);
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
        /*signIn.Disconnect("pressed", new Callable(this, nameof(Options)));
        signIn.Connect("pressed", new Callable(this, nameof(SignIn)));*/
    }
}
