using Godot;
using System;

public partial class DocAdderTextTemplate : HBoxContainer
{
    [Export] private Button _trashBtn;
    [Export] private TextEdit _text;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _trashBtn.Connect("pressed", new Callable(this, nameof(DeleteText)));
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    public void DeleteText()
    {
        var parent = this.GetParent();
        if (parent != null)
        {
            parent.QueueFree(); // Free the parent node
        }
    }
    public string GetText()
    {
        return _text.Text;
    }
}
