using Godot;
using System;
using Newtonsoft.Json.Linq;
using Aspose.Pdf;
using System.Threading.Tasks;

public partial class DocumentationAdder : Control
{
    [Export] private VBoxContainer boxContainer;

    [Export] private Button parSenBtn;
    [Export] private Button imageBtn;
    [Export] private Button saveBtn;
    [Export] private Button backBtn;

    [Export] private Control docAdderPanel;

    [Export] private LineEdit doc_Title;

    [Export] private FileDialog _imageFileDialogue;
    [Export] private FileDialog _saveFileDialogue;

    [Export] private NotificationHandler _notificationHandler;

    [Export] private DocumentationsList _documentationsList;

    [ExportCategory("Adder Templates")]
    [Export] private PackedScene _textTemplate;
    [Export] private PackedScene _imgTemplate;

    private string _selectedImagePath;

    private string directoryLoc = "Carp_Documentation";

    [ExportCategory("Loading")]
    [Export] private PackedScene _loading;
    [Export] private Node _loadingParent;
    Node loading;
    String _filepath;
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
        _saveFileDialogue.Connect("file_selected", new Callable(this, nameof(SaveDocumentationFilePath)));
        _saveFileDialogue.Connect("confirmed", new Callable(this, nameof(SaveDocumentationFile)));
    }
    private void BackPressed()
    {
        docAdderPanel.Visible = false;
    }

    private void AddParagraphAndSentence()
    {
        var marginContainer = new MarginContainer();

        var textDocTemplate = (DocAdderTextTemplate)_textTemplate.Instantiate();
        /*var txtEdit = new TextEdit
        {
            CustomMinimumSize = new Vector2(0, 100), // Set an initial manageable height
            AutowrapMode = TextServer.AutowrapMode.Word,
            PlaceholderText = "Type Here"
        };
        txtEdit.SizeFlagsVertical = (int)Control.SizeFlags.ShrinkBegin; // Allow the height to shrink if needed
        txtEdit.SizeFlagsHorizontal = SizeFlags.ExpandFill;

        txtEdit.WrapMode = TextEdit.LineWrappingMode.Boundary;
        txtEdit.AutowrapMode = TextServer.AutowrapMode.WordSmart;

        // Adjust the height after adding the text
        txtEdit.ScrollFitContentHeight = true;

        var font = ResourceLoader.Load<Font>("res://Fonts/Inter-Regular.ttf");

        // Set bold font
        txtEdit.AddThemeFontSizeOverride("font_size", 20);
        txtEdit.AddThemeFontOverride("font", font);*/

        //marginContainer.AddChild(txtEdit);
        marginContainer.AddChild(textDocTemplate);
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

            var rectDocTemplate = (DocAdderTextureTemplate)_imgTemplate.Instantiate();
            rectDocTemplate.SetImage(GD.Load<Texture2D>(_selectedImagePath));
            /*// Create the TextureRect
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
            }*/
            
            // Add the TextureRect to the MarginContainer
            marginContainer.AddChild(rectDocTemplate);

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

    public void SaveDocumentationFilePath(string path)
    {
        _filepath = path;
    }
    private async void SaveDocumentationFile()
    {
        await ShowLoading();
        SaveDoc();
        HideLoading();
        this.Visible = false;
    }
    private void SaveDoc()
    {
        if(_filepath != null )
        {
            using var file = FileAccess.Open(_filepath, FileAccess.ModeFlags.Write);

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
        }

    }

    private async Task ShowLoading()
    {
        // Instantiate and display the loading screen
        var loadingInstance = (LoadingManager)_loading.Instantiate();
        _loadingParent.AddChild(loadingInstance); // Add the loading screen to its parent
        loading = loadingInstance; // Store reference to the loading instance for future removal

        await Task.Delay(2000); // Adjust the delay as needed for better UX
    }

    private void HideLoading()
    {
        // Safely remove the loading screen
        if (loading != null && loading.IsInsideTree())
        {
            loading.QueueFree(); // Remove the loading indicator from the scene
        }
    }
    public string GetAllDocumentationData()
    {
        string allData = "";

        allData += "Title: " + doc_Title.Text +"\n\n";

        int childCount = boxContainer.GetChildCount();
        for (int i = 0; i < childCount; i++)
        {
            Node child = boxContainer.GetChild(i);

            if (child.GetChild(0) is DocAdderTextTemplate textEdit)
            {
                // Append TextEdit data
                allData += "Text: " + textEdit.GetText() + "\n";
            }
            else if (child.GetChild(0) is DocAdderTextureTemplate textureRect)
            {
                // Convert the texture to Base64 and append
                if (textureRect.GetTexture() is Texture2D texture2D)
                {
                    string base64Image = Converter.TextureToBase64(texture2D);
                    allData += "Image: " + base64Image + "\n";
                }
            }
        }

        return allData;
    }
}
