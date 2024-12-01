using Godot;
using System;
using System.Threading.Tasks;

public partial class ExitController : Node
{
	[Export] private Button exitbtn;
	[Export] private Panel savePanel;
	[Export] private Button yesbtn;
	[Export] private Button nobtn;

    [Export] private bool saveAvailable = true;

    [Export] private TextureRect _load;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        if (savePanel != null)
        {
            _load.Visible = false;
            savePanel.Visible = false;
            yesbtn.Connect("pressed", new Callable(this, nameof(Save)));
            nobtn.Connect("pressed", new Callable(this, nameof(NotSave)));
        }
        exitbtn.Connect("pressed", new Callable(this, nameof(Exit)));
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
	private async void Exit()
	{
        if (AccountManager.GetUser() != null && saveAvailable && (AccountManager.GetUser().Subscription.Equals("Subscribed") || AccountManager.GetUser().Role == "Teacher"))
        {
            savePanel.Visible = true;
		}
		else
		{
            await WaitTime();
            GetTree().Quit();
        }
	}
    private async void Save()
    {
        await LoadSave();

        DataToSave.status = "Idle";
        DataToSave.SaveFile();
        DataToSave.ResetDatas();

        _load.Visible = false;

        GetTree().Quit();
    }
    private void NotSave()
    {
        GetTree().Quit();
    }
    private async Task WaitTime()
    {
        await Task.Delay(1000);
    }

    private async Task LoadSave()
    {
        _load.Visible = true;

        await Task.Delay(1500);
    }
}
