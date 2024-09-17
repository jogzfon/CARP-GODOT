using Godot;
using System;
using Newtonsoft.Json.Linq;

public partial class DocumentationAdder : Control
{
    [Export] private VBoxContainer boxContainer;

    [Export] private Button parSenBtn;
    [Export] private Button imageBtn;
    [Export] private Button saveBtn;
    [Export] private TextureButton backBtn;

    [Export] private Control docAdderPanel;

    [Export] private LineEdit doc_Title;

    [Export] private FileDialog _imageFileDialogue;
    [Export] private FileDialog _saveFileDialogue;

    [Export] private NotificationHandler _notificationHandler;

    [Export] private DocumentationsList _documentationsList; 

    private string _selectedImagePath;

    private string directoryLoc = "Carp_Documentation";
    public override void _Ready()
    {
        DirAccess dir = DirAccess.Open(directoryLoc);
        if (dir == null)
        {
            dir.MakeDir(directoryLoc);
        }

        this.Visible = false;

        parSenBtn.Connect("pressed", new Callable(this, nameof(AddParagraphAndSentence)));
        imageBtn.Connect("pressed", new Callable(this, nameof(OpenImageFileDialog)));
        saveBtn.Connect("pressed", new Callable(this, nameof(OpenSaveFileDialog)));
        backBtn.Connect("pressed", new Callable(this, nameof(BackPressed)));

        _imageFileDialogue.Connect("file_selected", new Callable(this, nameof(OnImageFileSelected)));
        _saveFileDialogue.Connect("file_selected", new Callable(this, nameof(SaveDocumentationFile)));
    }
    private void BackPressed()
    {
        docAdderPanel.Visible = false;
    }

    private void AddParagraphAndSentence()
    {
        var marginContainer = new MarginContainer();
        var txtEdit = new TextEdit
        {
            CustomMinimumSize = new Vector2(0, 100), // Set an initial manageable height
            AutowrapMode = TextServer.AutowrapMode.Word,
            PlaceholderText = "Type Here"
        };
        txtEdit.SizeFlagsVertical = (int)Control.SizeFlags.ShrinkBegin; // Allow the height to shrink if needed


        // Adjust the height after adding the text
        txtEdit.ScrollFitContentHeight = true;

        marginContainer.AddChild(txtEdit);
        boxContainer.AddChild(marginContainer);
        // Move the TextureRect to be the second-to-last child
        boxContainer.MoveChild(marginContainer, boxContainer.GetChildCount() - 2);
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
                Texture = GD.Load<Texture2D>(_selectedImagePath)
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

            // Add the TextureRect to the MarginContainer
            marginContainer.AddChild(textureRect);

            // Add the MarginContainer to the VBoxContainer
            boxContainer.AddChild(marginContainer);

            // Move the MarginContainer to be the second-to-last child
            boxContainer.MoveChild(marginContainer, boxContainer.GetChildCount() - 2);
        }
    }
    private void OpenImageFileDialog()
    {
        // Open the file dialog for the user to select an image
        _imageFileDialogue.PopupCentered();
    }
    private void OpenSaveFileDialog()
    {
        // Open the file dialog for the user to select an image
        _saveFileDialogue.CurrentDir = directoryLoc;
        _saveFileDialogue.PopupCentered();
    }

    private void OnImageFileSelected(string path)
    {
        _selectedImagePath = path;
        AddImage();
    }

    public void SaveDocumentationFile(string path)
    {
        using var file = FileAccess.Open(path, FileAccess.ModeFlags.Write);

        if (file != null)
        {
            file.StoreString(GetAllDocumentationData()); //Save All Data to a file
            file.Close();
        }

        //Reset Datas
        while (boxContainer.GetChildCount() > 2)
        {
            Node child = boxContainer.GetChild(1);
            boxContainer.RemoveChild(child);
            child.QueueFree(); // Optionally free the node from memory
        }
        doc_Title.Text = "";

        _notificationHandler.MessageBox("Documentation Saved", 0);
        _documentationsList.RefreshAllDocumentFilesInCarp();
        this.Visible = false;
    }

    public string GetAllDocumentationData()
    {
        string allData = "";

        allData += "Title: " + doc_Title.Text +"\n\n";

        int childCount = boxContainer.GetChildCount();
        for (int i = 0; i < childCount; i++)
        {
            Node child = boxContainer.GetChild(i);

            if (child.GetChild(0) is TextEdit textEdit)
            {
                // Append TextEdit data
                allData += "Text: " + textEdit.Text + "\n";
            }
            else if (child.GetChild(0) is TextureRect textureRect)
            {
                // Convert the texture to Base64 and append
                if (textureRect.Texture is Texture2D texture2D)
                {
                    string base64Image = Converter.TextureToBase64(texture2D);
                    allData += "Image: " + base64Image + "\n";
                }
            }
        }

        return allData;
    }
}
