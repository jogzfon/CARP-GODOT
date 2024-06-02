using Godot;
using System;

public partial class system_menu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        var openProject = GetNode<TextureButton>("HBoxContainer/ExitViewSystem");
        openProject.Connect("pressed", new Callable(this, nameof(CloseViewSystem)));
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void CloseViewSystem()
	{
        Node simultaneousScene = ResourceLoader.Load<PackedScene>("res://Scenes/project_page.tscn").Instantiate();
        GetTree().Root.AddChild(simultaneousScene);
		Hide();
    }
}
