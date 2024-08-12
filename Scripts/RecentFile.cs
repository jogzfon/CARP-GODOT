using Godot;
using System;

public partial class RecentFile : VBoxContainer
{
    private bool isClicked = false;
    public string path = "";
	[Export] private Button FileName;
	[Export] private Button FileOpen;
	[Export] private Button FileRemove;

    [Export] PackedScene projectPage;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        FileName.Connect("pressed", new Callable(this, nameof(OpenButtons)));
        FileOpen.Connect("pressed", new Callable(this, nameof(OpenFile)));
        FileRemove.Connect("pressed", new Callable(this, nameof(RemoveFile)));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (!isClicked)
        {
            FileOpen.Visible = false;
            FileRemove.Visible = false;
        }
        else
        {
            FileOpen.Visible = true;
            FileRemove.Visible = true;
        }
    }
    public void SetFileName()
    {
        FileName.Text = System.IO.Path.GetFileName(path);
    }
    private void OpenButtons()
	{
        if (!isClicked)
        {
            isClicked = true;
        }
        else
        {
            isClicked = false;
        }
    }
    private void OpenFile()
    {
        if (path != "")
        {
            DataToSave.ResetDatas();
            DataToSave.filePath = path;
            DataToSave.OpenFile();
        }
        isClicked = false;
        Node simultaneous = projectPage.Instantiate();
        GetTree().Root.AddChild(simultaneous);
    }
    private void RemoveFile()
    {
        GetParent().RemoveChild(this);
        this.QueueFree();
    }
}
