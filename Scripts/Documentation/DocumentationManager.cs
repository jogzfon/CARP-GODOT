using Godot;
using System;

public partial class DocumentationManager : Node
{
	[Export] private TextureButton _documentationAdderBtn;
    [Export] private Control _documentationList;
    [Export] private VBoxContainer _documentationBtnList;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (AccountManager.GetUser() != null)
		{
			if(AccountManager.GetRole().Contains("Teacher") || AccountManager.GetSubscription().Contains("Subscribed"))
			{
				_documentationAdderBtn.Visible = true;
				_documentationList.Visible = true;
				_documentationBtnList.Visible = true;


            }
			else
			{
                _documentationAdderBtn.Visible = false;
                _documentationList.Visible = false;
                _documentationBtnList.Visible = false;
            }
		}
		else
		{
            _documentationAdderBtn.Visible = false;
            _documentationList.Visible = false;
            _documentationBtnList.Visible = false;
        }
	}
}
