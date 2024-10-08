using Godot;
using OpenAI_API.Files;
using System;
using System.Collections.Generic;
using System.IO;

public partial class main_page : Control
{
    [Export] private Button signIn;
    [Export] private Button signOut;

    [Export] private Label userName;

    [Export] private VBoxContainer options;

    [Export] private Button logout;
    [Export] private Button settings;

    [Export] PackedScene logInPage;

    [Export] private Control documentationAdder;

    [ExportCategory("Documentation Disabler")]
    [Export] private DocumentationAbler _documentationAbler;

    [ExportCategory("Profile Things")]
    [Export] private SubscriptionHandler _subscriptionHandler;
    [Export] private ProfileHandler _profileHandler;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        options.Hide();
        signIn.Connect("pressed", new Callable(this, nameof(SignIn)));
        signOut.Connect("pressed", new Callable(this, nameof(Options)));

        logout.Connect("pressed", new Callable(this, nameof(Logout)));

        AccountFileSaver.GetAccountFile();
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
        _documentationAbler.HideAllDocument();
        if (documentationAdder != null)
        {
            documentationAdder.Visible = false;
        }

        Node simultaneous = logInPage.Instantiate();
        GetTree().Root.AddChild(simultaneous);
    }
    private void Options()
    {
        _documentationAbler.HideAllDocument();
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
        _profileHandler.CloseProfile();
        _subscriptionHandler.CloseSubscription();
        _documentationAbler.HideAllDocument();
        options.Visible = false;
        AccountManager.SetUser(null);
        if(userName != null && signIn != null)
        {
            userName.Text = "John Doe";
            signIn.Text = "Sign-In";
        }

        AccountFileSaver.DeleteAccountFile();
    }
}
