using Godot;
using System;

public partial class DocumentationTemplate : Control
{
	[Export] private VBoxContainer _contentContainer;
	[Export] private Container _docContainer;
	[Export] private Button _exitBtn;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        _docContainer.Visible = false;
        _exitBtn.Connect("pressed", new Callable(this, nameof(DocContentDisplayer)));
        this.ZIndex = 1;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void DocumentationBtn(Button btn)
	{
        btn.Connect("pressed", new Callable(this, nameof(DocContentDisplayer)));
    }
	public void DocContentDisplayer()
	{
		if(_docContainer.Visible == true)
		{
			_docContainer.Visible = false;

        }
		else
		{
			_docContainer.Visible = true;
        }
    }
    public void AddTitle(string content)
    {
        var marginContainer = new MarginContainer();
        var txtEdit = new Label
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            CustomMinimumSize = new Vector2(0, 100), // Set an initial manageable height
            AutowrapMode = TextServer.AutowrapMode.Word,
            Text = content
        };

        txtEdit.SizeFlagsVertical = (int)Control.SizeFlags.ShrinkBegin; // Allow the height to shrink if needed
        txtEdit.SizeFlagsHorizontal = SizeFlags.ExpandFill;

        var font = ResourceLoader.Load<Font>("res://Fonts/Inter-Bold.ttf");

        // Set bold font
        txtEdit.AddThemeFontSizeOverride("font_size", 30);
        txtEdit.AddThemeFontOverride("font", font);

        marginContainer.AddChild(txtEdit);

        _contentContainer.AddChild(marginContainer);
    }
    public void AddText(string content)
	{
        var marginContainer = new MarginContainer();
        var txtEdit = new TextEdit
        {
            Editable = false,
            CustomMinimumSize = new Vector2(0, 100), // Set an initial manageable height
            AutowrapMode = TextServer.AutowrapMode.Word,
            PlaceholderText = "Type Here",
            Text = content  
        };
        txtEdit.SizeFlagsVertical = (int)Control.SizeFlags.ShrinkBegin; // Allow the height to shrink if needed
        txtEdit.SizeFlagsHorizontal = SizeFlags.ExpandFill;

        // Adjust the height after adding the text
        txtEdit.ScrollFitContentHeight = true;

        txtEdit.AddThemeColorOverride("font_readonly_color", new Color(1, 1, 1)); // White font color when disabled

        var font = ResourceLoader.Load<Font>("res://Fonts/Inter-Regular.ttf");

        // Set bold font
        txtEdit.AddThemeFontSizeOverride("font_size", 20);
        txtEdit.AddThemeFontOverride("font", font);

        marginContainer.AddChild(txtEdit);

		_contentContainer.AddChild(marginContainer);
	}
	public void AddImage(string texture)
	{
        var marginContainer = new MarginContainer();
        marginContainer.AddThemeConstantOverride("margin_top", 10);
        marginContainer.AddThemeConstantOverride("margin_bottom", 10);
        marginContainer.AddThemeConstantOverride("margin_left", 10);
        marginContainer.AddThemeConstantOverride("margin_right", 10);
        TextureRect img = new TextureRect
        {
            Texture = Converter.Base64ToTexture(texture),
            ExpandMode = TextureRect.ExpandModeEnum.KeepSize,
            SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter,
            SizeFlagsVertical = Control.SizeFlags.ShrinkCenter
        };

        img.CustomMinimumSize = new Vector2(100, 100);
        marginContainer.AddChild(img);
        _contentContainer.AddChild(marginContainer);
	}
}
