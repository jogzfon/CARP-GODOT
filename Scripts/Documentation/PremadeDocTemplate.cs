using Godot;
using System;

public partial class PremadeDocTemplate : Node
{
	[Export] private VBoxContainer _contentHolder;
    [Export] private VBoxContainer _contentScrollbar;
    [Export] private TextureRect _contentTexture;
    [Export] private ColorRect _backgroundTexture;
    [Export] private Button _exitBtn;

    private DocumentationAbler _documentationAbler;

    public override void _Ready()
    {
        _backgroundTexture.Visible = false;
        _contentTexture.Visible = false;
        // Ensure _contentHolder and _contentScrollbar are initialized
        if (_contentHolder == null)
        {
            GD.PrintErr("_contentHolder is not assigned in the editor.");
        }

        if (_contentScrollbar == null)
        {
            GD.PrintErr("_contentScrollbar is not assigned in the editor.");
        }
        _contentScrollbar.Visible = false;

        _exitBtn.Connect("pressed", new Callable(this, nameof(OpenOrCloseDocument)));
    }

    public void AddDocumentBtn(Button btn)
	{
        btn.Connect("pressed", new Callable(this, nameof(OpenOrCloseDocument)));
    }

    public void SetDocumentationAbler(DocumentationAbler _docAbler)
    {
        this._documentationAbler = _docAbler;
    }

    public void AddContent(Node node)
	{
        // Check if _contentHolder is not null
        if (_contentHolder != null)
        {
            // Ensure the node is of type MarginContainer before adding
            if (node is MarginContainer marginContainer)
            {
                _contentHolder.AddChild(marginContainer);
            }
            else
            {
                GD.PrintErr("The node is not a MarginContainer and will not be added.");
            }
        }
        else
        {
            GD.PrintErr("_contentHolder is not initialized.");
        }
    }
	public void OpenOrCloseDocument()
	{
        if (_contentScrollbar.Visible == true)
		{
            _documentationAbler.HideAllDocument();
            _contentScrollbar.Visible = false;
            _contentTexture.Visible = false;
            _backgroundTexture.Visible = false;

        }
		else
		{
            _documentationAbler.HideAllDocument();
            _contentScrollbar.Visible = true;
            _contentTexture.Visible = true;
            _backgroundTexture.Visible = true;

        }
	}
    public void CloseDocument()
    {
        _contentScrollbar.Visible = false;
        _contentTexture.Visible = false;
        _backgroundTexture.Visible = false;
    }
}
