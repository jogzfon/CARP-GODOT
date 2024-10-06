using Godot;
using System;

public partial class LoadingManager : Node
{
	[Export] private String message;
	[Export] private TextureRect loadingImg;
	[Export] private Label messageBox;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        loadingImg.Visible = false;
        messageBox.Visible = false;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void OpenLoadingScreen()
	{
        if (message != null)
        {
            loadingImg.Visible = true;
            messageBox.Visible = true;
            messageBox.Text = message;
        }
        else
        {
            loadingImg.Visible = true;
            messageBox.Visible = false;
        }
    }
}
