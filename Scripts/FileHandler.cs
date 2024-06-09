using Godot;
using System;

public partial class FileHandler : Node
{
    [Export]
    FileDialog dialogue;
    [Export]
    Button newProject;
    [Export]
    Button openProject;

    #region DocumentationBtn
    [Export]
    public ColorRect documentationRect;

    [Export]
    public Button witbtn;
    [Export]
    public Button witexit;
    [Export]
    public PanelContainer WIT;
    [Export]
    public Button htdwbtn;
    [Export]
    public Button htdwexit;
    [Export]
    public PanelContainer HDTW;
    [Export]
    public Button pexbtn;
    [Export]
    public Button pexexit;
    [Export]
    public PanelContainer PEX;
    [Export]
    public Button ncbtn;
    [Export]
    public Button ncexit;
    [Export]
    public PanelContainer NC;
    [Export]
    public Button adbtn;
    [Export]
    public Button adexit;
    [Export]
    public PanelContainer AD;
    [Export]
    public Button aapbtn;
    [Export]
    public Button aapexit;
    [Export]
    public PanelContainer AAP;
    #endregion

    string path;
    public static string message = "Hello World";
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        newProject.Connect("pressed", new Callable(this, nameof(NewProject)));

        openProject.Connect("pressed", new Callable(this, nameof(OpenProject)));

        dialogue.Connect("file_selected", new Callable(this, nameof(OnFileSelected)));

        witbtn.Connect("pressed", new Callable(this, nameof(WitOpen)));
        htdwbtn.Connect("pressed", new Callable(this, nameof(HdtwOpen)));
        pexbtn.Connect("pressed", new Callable(this, nameof(PexOpen)));
        ncbtn.Connect("pressed", new Callable(this, nameof(NcOpen)));
        adbtn.Connect("pressed", new Callable(this, nameof(AdOpen)));
        aapbtn.Connect("pressed", new Callable(this, nameof(AapOpen)));

        witexit.Connect("pressed", new Callable(this, nameof(WitClose)));
        htdwexit.Connect("pressed", new Callable(this, nameof(HdtwClose)));
        pexexit.Connect("pressed", new Callable(this, nameof(PexClose)));
        ncexit.Connect("pressed", new Callable(this, nameof(NcClose)));
        adexit.Connect("pressed", new Callable(this, nameof(AdClose)));
        aapexit.Connect("pressed", new Callable(this, nameof(AapClose)));

        documentationRect.Visible = false;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if(!WIT.Visible && !HDTW.Visible && !PEX.Visible && !NC.Visible && !AD.Visible && !AAP.Visible)
        {
            documentationRect.Visible = false;
        }
	}

    #region FileHandler
    private void NewProject()
    {
        dialogue.FileMode = FileDialog.FileModeEnum.SaveFile;
        dialogue.PopupCentered();
    }
    private void OpenProject()
    {
        dialogue.FileMode = FileDialog.FileModeEnum.OpenFile;
        dialogue.PopupCentered();
    }
    private void OnFileSelected(string path)
    {
        if (dialogue.FileMode == FileDialog.FileModeEnum.SaveFile)
        {
            SaveToFile(path);
        }
        else if (dialogue.FileMode == FileDialog.FileModeEnum.OpenFile)
        {
            OpenFromFile(path);
        }
        Node simultaneousScene = ResourceLoader.Load<PackedScene>("res://Scenes/project_page.tscn").Instantiate();
        GetTree().Root.AddChild(simultaneousScene);
        GetTree().Root.Hide();
    }
    private void SaveToFile(string path)
    {
        string dataToSave = message;
        using var file = FileAccess.Open(path, FileAccess.ModeFlags.Write);
        file.StoreString(dataToSave);
        file.Close();
        GD.Print("File saved: " + path);
    }

    private void OpenFromFile(string path)
    {
        if (!FileAccess.FileExists(path))
        {
            GD.PrintErr("File does not exist: " + path);
            return;
        }

        using var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        string content = file.GetAsText();
        file.Close();
        GD.Print("File opened: " + path);
        GD.Print("Content: " + content);
        message = content;
    }

    #endregion

    #region Documentations
    private void WitOpen()
    {
        documentationRect.Visible = true;
        WIT.Visible = true;
        HDTW.Visible = false;
        PEX.Visible = false;
        NC.Visible = false;
        AD.Visible = false;
        AAP.Visible = false;

        HdtwClose();
        PexClose();
        NcClose();
        AdClose();
        AapClose();
    }
    private void HdtwOpen()
    {
        documentationRect.Visible = true;
        WIT.Visible = false;
        HDTW.Visible = true;
        PEX.Visible = false;
        NC.Visible = false;
        AD.Visible = false;
        AAP.Visible = false;

        WitClose();
        PexClose();
        NcClose();
        AdClose();
        AapClose();
    }   
    private void PexOpen()
    {
        documentationRect.Visible = true;
        WIT.Visible = false;
        HDTW.Visible = false;
        PEX.Visible = true;
        NC.Visible = false;
        AD.Visible = false;
        AAP.Visible = false;

        HdtwClose();
        WitClose();
        NcClose();
        AdClose();
        AapClose();
    }
    private void NcOpen()
    {
        documentationRect.Visible = true;
        WIT.Visible = false;
        HDTW.Visible = false;
        PEX.Visible = false;
        NC.Visible = true;
        AD.Visible = false;
        AAP.Visible = false;

        HdtwClose();
        PexClose();
        WitClose();
        AdClose();
        AapClose();
    }
    private void AdOpen()
    {
        documentationRect.Visible = true;
        WIT.Visible = false;
        HDTW.Visible = false;
        PEX.Visible = false;
        NC.Visible = false;
        AD.Visible = true;
        AAP.Visible = false;

        HdtwClose();
        PexClose();
        NcClose();
        WitClose();
        AapClose();
    }
    private void AapOpen()
    {
        documentationRect.Visible = true;
        WIT.Visible = false;
        HDTW.Visible = false;
        PEX.Visible = false;
        NC.Visible = false;
        AD.Visible = false;
        AAP.Visible = true;

        HdtwClose();
        PexClose();
        NcClose();
        AdClose();
        WitClose();
    }

    private void WitClose()
    {
        WIT.Visible = false;
    }
    private void HdtwClose()
    {
        HDTW.Visible = false;
    }
    private void PexClose()
    {
        PEX.Visible = false;
    }
    private void NcClose()
    {
        NC.Visible = false;
    }
    private void AdClose()
    {
        AD.Visible = false;
    }
    private void AapClose()
    {
        AAP.Visible = false;
    }
    #endregion
}