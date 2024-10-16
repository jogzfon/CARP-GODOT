using Godot;
using System;

public partial class DocBtnContainer : HBoxContainer
{
    [Export] private Button _docBtn;
    [Export] private Button _hideBtnOptionBtn;
    [Export] private Button _deleteOptionBtn;
    [Export] private Button _cancelOptionBtn;

    [Export] private Button _confirmBtn;
    [Export] private Button _cancelBtn;

    [Export] private VBoxContainer _optionsContainer;
    [Export] private VBoxContainer _deleteConfirmationContainer;

    private DocumentationAbler _abler;
    private DocumentationTemplate _template;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _optionsContainer.Visible = false;
        _deleteConfirmationContainer.Visible = false;

        _hideBtnOptionBtn.Connect("pressed", new Callable(this, nameof(HideFromView)));
        _deleteOptionBtn.Connect("pressed", new Callable(this, nameof(ShowDeleteConfirmation)));
        _cancelOptionBtn.Connect("pressed", new Callable(this, nameof(HideOption)));

        _confirmBtn.Connect("pressed", new Callable(this, nameof(DeleteDocumentFile)));
        _cancelBtn.Connect("pressed", new Callable(this, nameof(HideDeleteConfirmation)));

        // Connect the _docBtn input event
        _docBtn.Connect("gui_input", new Callable(this, nameof(OnDocBtnGuiInput)));
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        
	}

    // Handles the right-click input on the _docBtn
    public void OnDocBtnGuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            // Button index 2 is the right mouse button
            if (mouseEvent.ButtonIndex == MouseButton.Right && mouseEvent.Pressed)
            {
                //GD.Print("Right-click detected on the button.");
                // You can add additional logic here, such as showing options or a context menu
                if (_optionsContainer.Visible == true) {
                    _optionsContainer.Visible = false;
                    _deleteConfirmationContainer.Visible = false;
                }
                else
                {
                    _optionsContainer.Visible = true;
                    _deleteConfirmationContainer.Visible = false;
                }
            }
        }
    }
    
    private void HideFromView()
	{
        _abler.RemoveDocTemplate(_template);
        this.QueueFree();
	}

    private void ShowDeleteConfirmation()
    {
        _optionsContainer.Visible = false;
        _deleteConfirmationContainer.Visible = true;
    }
    private void HideOption()
    {
        _optionsContainer.Visible = false;
    }
    private void HideDeleteConfirmation()
    {
        _optionsContainer.Visible = true;
        _deleteConfirmationContainer.Visible = false;
    }
    private void DeleteDocumentFile()
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
