using Godot;
using System;
using System.Threading.Tasks;

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
    public async void RefreshDocs()
    {
        await RefreshIt();

        _abler.RemoveAllTemplates();

        _documents.RefreshAllDocumentFilesInCarp();
        _refresher.Material = null;
    }
    private async Task RefreshIt()
    {
        _refresher.Material = _shader;
        await Task.Delay(1000);
    }
}
