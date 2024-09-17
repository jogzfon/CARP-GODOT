using Godot;
using System;

public partial class DocumentationTemplate : Node
{
	[Export] private VBoxContainer _docContainer;
    [Export] private TextureRect _docRect;
	[Export] private VBoxContainer _contentContainer;
	[Export] private Button _exitBtn;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        _docContainer.Visible = false;
        _docRect.Visible = false;
        _exitBtn.Connect("pressed", new Callable(this, nameof(DocContentDisplayer)));
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
            _docRect.Visible = false;
        }
		else
		{
			_docContainer.Visible = true;
            _docRect.Visible = true;
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
        txtEdit.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;

        txtEdit.AutowrapMode = TextServer.AutowrapMode.WordSmart;

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
        txtEdit.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;

        txtEdit.WrapMode = TextEdit.LineWrappingMode.Boundary;

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
        var textureRect = new TextureRect
        {
            Texture = Converter.Base64ToTexture(texture),
        };
        var textureSize = textureRect.Texture.GetSize();  // Get the size of the loaded texture
        if (textureSize.X < 1400)
        {
            textureRect.ExpandMode = TextureRect.ExpandModeEnum.KeepSize;
            textureRect.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
            textureRect.SizeFlagsVertical = Control.SizeFlags.ShrinkCenter;
            textureRect.CustomMinimumSize = new Vector2(100, 100);
        }
        else
        {
            textureRect.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
            textureRect.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
            textureRect.SizeFlagsVertical = Control.SizeFlags.ShrinkCenter;
            var maxWidth = 1400f;
            var maxHeight = 1000f;
            // Calculate scaling factor to maintain aspect ratio within max width and height
            var scaleFactor = Math.Min(maxWidth / textureSize.X, maxHeight / textureSize.Y);

            // Apply the scaling factor to both dimensions to preserve aspect ratio
            textureRect.CustomMinimumSize = new Vector2(textureSize.X * scaleFactor, textureSize.Y * scaleFactor);
        }
        marginContainer.AddChild(textureRect);
        _contentContainer.AddChild(marginContainer);
	}
}
