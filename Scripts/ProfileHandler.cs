using Godot;
using System;

public partial class ProfileHandler : Node
{
    [Export]
    Button schoolBtn;
    [Export]
    MarginContainer centerMargin;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        if (AccountManager.GetUser() != null && AccountManager.schoolOn)
        {
            schoolBtn.Visible = true;
            centerMargin.Visible = true;
            schoolBtn.Connect("pressed", new Callable(this, nameof(SchoolPage)));
        }
        else
        {
            schoolBtn.Visible = false;
            centerMargin.Visible = false;
        }
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    private void SchoolPage()
    {

    }
}
