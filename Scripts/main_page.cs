using Godot;
using OpenAI_API.Files;
using System;
using System.Collections.Generic;
using System.IO;

public partial class main_page : Control
{
	VBoxContainer projectList;
	Panel projectNamePnl;
	LineEdit projectName;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        projectList = GetNode<VBoxContainer>("ProjectList");
		projectNamePnl = GetNode<Panel>("ProjectNamePanel");
        projectName = GetNode<LineEdit>("ProjectNamePanel/ProjectName");

        projectNamePnl.Hide();

        var submitName = GetNode<Button>("ProjectNamePanel/SubmitName");
        submitName.Connect("pressed", new Callable(this, nameof(SubmitName)));

        var signIn = GetNode<TextureButton>("Sign-In");
        signIn.Connect("pressed", new Callable(this, nameof(SignIn)));

        var back = GetNode<TextureButton>("ProjectNamePanel/Back");
        back.Connect("pressed", new Callable(this, nameof(CancelNewProject)));
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

    }
	private void SubmitName()
	{
        Button btn = new Button();
        btn.Text = projectName.Text;
        projectList.AddChild(btn);
        projectNamePnl.Hide();

        Node simultaneousScene = ResourceLoader.Load<PackedScene>("res://Scenes/project_page.tscn").Instantiate();
        GetTree().Root.AddChild(simultaneousScene);
        Hide();
    }
    private void CancelNewProject()
    {
        projectNamePnl.Hide();
    }
    private void SignIn()
    {
        Node simultaneousScene = ResourceLoader.Load<PackedScene>("res://Scenes/StartUpPage.tscn").Instantiate();
        GetTree().Root.AddChild(simultaneousScene);
        Hide();
    }
}
