using Godot;
using System;

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

        var newProject = GetNode<TextureButton>("NewProject");
        newProject.Connect("pressed", new Callable(this, nameof(NewProject)));

        var openProject = GetNode<TextureButton>("OpenProject");
        openProject.Connect("pressed", new Callable(this, nameof(OpenProject)));

        var signIn = GetNode<TextureButton>("Sign-In");
        signIn.Connect("pressed", new Callable(this, nameof(SignIn)));
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
    private void NewProject()
    {
        projectNamePnl.Show();
    }
    private void OpenProject()
    {
       
    }
    private void SignIn()
    {
        Node simultaneousScene = ResourceLoader.Load<PackedScene>("res://Scenes/StartUpPage.tscn").Instantiate();
        GetTree().Root.AddChild(simultaneousScene);
        Hide();
    }
}
