using Godot;
using System;

public partial class DocumentationAdder : Control
{
	[Export] VBoxContainer boxContainer;

	[Export] Button parSenBtn;
    [Export] Button imageBtn;

    public override void _Ready()
	{
        parSenBtn.Connect("pressed", new Callable(this, nameof(AddParagraphAndSentence)));
        imageBtn.Connect("pressed", new Callable(this, nameof(AddImage)));
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void AddParagraphAndSentence()
	{

	}
    private void AddImage()
    {

    }
}
