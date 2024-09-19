using Godot;
using System;

public partial class DocumentationRefresher : HBoxContainer
{
    [Export] private Button _refresher;
    [Export] private DocumentationsList _documents;

    [Export] private ShaderMaterial _shader;

    [Export] private DocumentationAbler _abler;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _refresher.Connect("pressed", new Callable(this, nameof(RefreshDocs)));
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (AccountManager.GetUser() != null)
        {
            if (AccountManager.GetRole().Contains("Teacher") || AccountManager.GetUser().Subscription == "Subscribed")
            {
                _refresher.Visible = true;

            }
            else
            {
                _refresher.Visible = false;
            }
        }
        else
        {
            _refresher.Visible = false;
        }
    }
    public void RefreshDocs()
    {
        _abler.RemoveAllTemplates();

        _refresher.Material = _shader;
        _documents.RefreshAllDocumentFilesInCarp();
        _refresher.Material = null;
    }
}
