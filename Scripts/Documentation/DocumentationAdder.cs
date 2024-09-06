using Godot;
using System;
using Newtonsoft.Json.Linq;

public partial class DocumentationAdder : Control
{
    [Export] private VBoxContainer boxContainer;

    [Export] private VBoxContainer _addedDocumentationList;

    [Export] private Button parSenBtn;
    [Export] private Button imageBtn;
    [Export] private Button saveBtn;

    [Export] private LineEdit doc_Title;

    [Export] private FileDialog _imageFileDialogue;
    [Export] private FileDialog _saveFileDialogue;

    [Export] private NotificationHandler _notificationHandler;

    [Export] private DocumentationsList _documentationsList; 

    private string _selectedImagePath;
    public override void _Ready()
    {
        parSenBtn.Connect("pressed", new Callable(this, nameof(AddParagraphAndSentence)));
        imageBtn.Connect("pressed", new Callable(this, nameof(OpenImageFileDialog)));
        saveBtn.Connect("pressed", new Callable(this, nameof(OpenSaveFileDialog)));

        _imageFileDialogue.Connect("file_selected", new Callable(this, nameof(OnImageFileSelected)));
        _saveFileDialogue.Connect("file_selected", new Callable(this, nameof(SaveDocumentationFile)));
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
                Texture = GD.Load<Texture2D>(_selectedImagePath),
                ExpandMode = TextureRect.ExpandModeEnum.KeepSize,
                SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter,
                SizeFlagsVertical = Control.SizeFlags.ShrinkCenter
            };

            textureRect.CustomMinimumSize = new Vector2(100, 100);

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
        _documentationsList.GetAllCarpdocFilesInDirectory();
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
