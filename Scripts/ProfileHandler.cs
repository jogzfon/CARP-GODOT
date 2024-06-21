using Godot;
using System;

public partial class ProfileHandler : Node
{
    [Export]
    Button schoolBtn;
    [Export]
    MarginContainer centerMargin;
    [Export]
    TextureButton accountProfile;
    [Export]
    Panel profilePanel;
    [Export]
    Button exitProfile;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        if (AccountManager.GetUser() != null)
        {
            if (AccountManager.schoolOn)
            {
                schoolBtn.Visible = true;
                centerMargin.Visible = true;
                schoolBtn.Connect("pressed", new Callable(this, nameof(SchoolPage)));
            }
        }
        else
        {
            accountProfile.Disabled = true;
            profilePanel.Visible = false;
            schoolBtn.Visible = false;
            centerMargin.Visible = false;
        }
        accountProfile.Connect("pressed", new Callable(this, nameof(OpenProfilePage)));
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (AccountManager.GetUser() != null)
        {
            accountProfile.Disabled = false;
        }
	}
    private void SchoolPage()
    {

    }
    private void OpenProfilePage()
    {
        profilePanel.Visible = true;
        exitProfile.Connect("pressed", new Callable(this, nameof(CloseProfilePage)));
    }
    private void CloseProfilePage()
    {
        profilePanel.Visible = false;
    }
}
