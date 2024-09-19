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
    [Export]
    Button goToSystem;
    [Export]
    HBoxContainer directToSystem;
    [Export]
    HBoxContainer manageFile;

    [Export] PackedScene projectPage;

    [Export] PackedScene recentFile;
    [Export] VBoxContainer recentFileList;

    [Export] private Control documentationAdder;

    string path;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        newProject.Connect("pressed", new Callable(this, nameof(NewProject)));

        openProject.Connect("pressed", new Callable(this, nameof(OpenProject)));

        goToSystem.Connect("pressed", new Callable(this, nameof(GoToSystem)));

        dialogue.Connect("file_selected", new Callable(this, nameof(OnFileSelected)));
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{       
        if (AccountManager.GetUser() != null && (AccountManager.GetRole().Contains("Teacher") || AccountManager.GetUser().Subscription == "Subscribed"))
        {
            manageFile.Visible = true;
            directToSystem.Visible = false;
        }
        else
        {
            manageFile.Visible = false;
            directToSystem.Visible = true;
            // Clear the list of recent files
            foreach (Node child in recentFileList.GetChildren())
            {
                child.QueueFree(); // Remove each child from the scene
            }
        }
    }

    private void GoToSystem() {
        Node simultaneous = projectPage.Instantiate();
        GetTree().Root.AddChild(simultaneous);
        //GetTree().ChangeSceneToPacked(projectPage);

    }
    #region FileHandler
    private void NewProject()
    {
        if (documentationAdder != null)
        {
            documentationAdder.Visible = false;
        }

        DataToSave.ResetDatas();
        dialogue.FileMode = FileDialog.FileModeEnum.SaveFile;
        dialogue.PopupCentered();
    }
    private void OpenProject()
    {
        if (documentationAdder != null)
        {
            documentationAdder.Visible = false;
        }

        dialogue.FileMode = FileDialog.FileModeEnum.OpenFile;
        dialogue.PopupCentered();
    }
    private void OnFileSelected(string path)
    {
        DataToSave.filePath = path;
        if (dialogue.FileMode == FileDialog.FileModeEnum.SaveFile)
        {
            DataToSave.SaveFile();
        }
        else if (dialogue.FileMode == FileDialog.FileModeEnum.OpenFile)
        {
            DataToSave.OpenFile();
        }

        var file = (Node)recentFile.Instantiate();
        if (file is RecentFile recentFileScript)
        {
            recentFileScript.path = path;
            recentFileScript.SetFileName();
        }
        int counter = 0;
        foreach (RecentFile child in recentFileList.GetChildren())
        {
            if (child.path == path)
            {
                counter++;
            }
        }
        if(counter == 0)
        {
            recentFileList.AddChild(file);
        }

        Node simultaneous = projectPage.Instantiate();
        GetTree().Root.AddChild(simultaneous);
    }
    #endregion
}
