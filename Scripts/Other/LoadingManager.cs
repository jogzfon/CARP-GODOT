using Godot;
using System;

public partial class LoadingManager : TextureRect
{
	[Export] private Label messageBox;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        messageBox.Visible = false;
    }

    public void SetMessage(string message)
    {
        if (message != null && messageBox.Text == String.Empty)
        {
            messageBox.Visible = true;
            messageBox.Text = message;
        }
        else
        {
            messageBox.Visible = false;
        }
    }
}
