using Godot;
using System;

public partial class DocBtnContainer : HBoxContainer
{
    [Export] private Button _docBtn;
    [Export] private Button _trashBtn;
    private DocumentationAbler _abler;
    private DocumentationTemplate _template;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _trashBtn.Connect("pressed", new Callable(this, nameof(DeleteBtnAndFile)));
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void DeleteBtnAndFile()
	{
        _abler.RemoveDocTemplate(_template);
        this.QueueFree();
	}

	public Button GetButton()
	{
		return _docBtn;
	}
	public void SetTemplate(DocumentationTemplate template)
	{
        _template = template;
    }
    public void SetAbler(DocumentationAbler abler)
    {
        _abler = abler;
    }
}
