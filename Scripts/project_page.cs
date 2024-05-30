using Godot;
using System;

public partial class project_page : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        var toSystem = GetNode<Button>("VBoxContainer2/ViewSystem");
        toSystem.Connect("pressed", new Callable(this, nameof(GoToSystem)));

        var toAI = GetNode<Button>("VBoxContainer/HBoxContainer/AIBtn");
        toAI.Connect("pressed", new Callable(this, nameof(GoToAI)));
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    private void GoToAI()
    {
        Node simultaneousScene = ResourceLoader.Load<PackedScene>("res://Scenes/ai_system.tscn").Instantiate();
        GetTree().Root.AddChild(simultaneousScene);
        Hide();
    }

    private void GoToSystem()
	{
        Node simultaneousScene = ResourceLoader.Load<PackedScene>("res://Scenes/main.tscn").Instantiate();
        GetTree().Root.AddChild(simultaneousScene);
        Hide();
    }
}
