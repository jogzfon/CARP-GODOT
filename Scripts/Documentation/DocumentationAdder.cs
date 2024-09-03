using Godot;
using System;

public partial class DocumentationAdder : Control
{
    [Export] private VBoxContainer boxContainer;

    [Export] private Button parSenBtn;
    [Export] private Button imageBtn;
    [Export] private Button saveBtn;

    [Export] private FileDialog _imageFileDialogue;

    private string _selectedImagePath;
    public override void _Ready()
    {
        parSenBtn.Connect("pressed", new Callable(this, nameof(AddParagraphAndSentence)));
        imageBtn.Connect("pressed", new Callable(this, nameof(OpenFileDialog)));

        _imageFileDialogue.Connect("file_selected", new Callable(this, nameof(OnImageFileSelected)));
    }

    private void AddParagraphAndSentence()
    {
        var marginContainer = new MarginContainer();
        var txtEdit = new TextEdit
        {
            CustomMinimumSize = new Vector2(0, 100), // Set an initial manageable height
            AutowrapMode = TextServer.AutowrapMode.Word,
            Text = "Type Here"
        };
        txtEdit.SizeFlagsVertical = (int)Control.SizeFlags.ShrinkBegin; // Allow the height to shrink if needed


        // Adjust the height after adding the text
        txtEdit.ScrollFitContentHeight = true;

        marginContainer.AddChild(txtEdit);
        boxContainer.AddChild(marginContainer);
        // Move the TextureRect to be the second-to-last child
        boxContainer.MoveChild(marginContainer, boxContainer.GetChildCount() - 3);
    }

    private void AddImage()
    {
        if (!string.IsNullOrEmpty(_selectedImagePath))
        {
            // Create a MarginContainer to hold the TextureRect
            var marginContainer = new MarginContainer();
            marginContainer.AddThemeConstantOverride("margin_top", 10);
            marginContainer.AddThemeConstantOverride("margin_bottom", 10);
            marginContainer.AddThemeConstantOverride("margin_left", 10);
            marginContainer.AddThemeConstantOverride("margin_right", 10);
            // Create the TextureRect
            var textureRect = new TextureRect
            {
                Texture = GD.Load<Texture2D>(_selectedImagePath),
                ExpandMode = TextureRect.ExpandModeEnum.KeepSize,
                SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter,
            };

            // Add the TextureRect to the MarginContainer
            marginContainer.AddChild(textureRect);

            // Add the MarginContainer to the VBoxContainer
            boxContainer.AddChild(marginContainer);

            // Move the MarginContainer to be the second-to-last child
            boxContainer.MoveChild(marginContainer, boxContainer.GetChildCount() - 3);
        }
    }
    private void OpenFileDialog()
    {
        // Open the file dialog for the user to select an image
        _imageFileDialogue.PopupCentered();
    }
    private void OnImageFileSelected(string path)
    {
        _selectedImagePath = path;
        AddImage();
    }
}
