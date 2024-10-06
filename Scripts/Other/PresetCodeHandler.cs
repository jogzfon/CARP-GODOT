using Godot;
using System;
using System.Threading.Tasks;
public partial class PresetCodeHandler : Node
{
	[Export] private TextEdit _assembledCode;
    [Export] private LineEdit _codeKeyword;

    [Export] private Button _exitBtn;
    [Export] private Button _savePresetBtn;
    [Export] private Button _finalizeSavePresetBtn;

    [Export] private TextureRect _presetTextureRect;
	[Export] private HBoxContainer _presetBtnContainer;

	[Export] private InstructionCodeHandler _instructionCodeHandler;
    [Export] private NotificationHandler _notificationHandler;

	[Export] private TextureRect _loading;

    private PremadeCodeList _premadeCodeList;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_presetTextureRect.Visible = false;

        _exitBtn.Connect("pressed", new Callable(this, nameof(ExitPresetPanel)));

        _savePresetBtn.Connect("pressed", new Callable(this, nameof(OpenKeywordInput)));
        _finalizeSavePresetBtn.Connect("pressed", new Callable(this, nameof(SaveInstructionsAsPreset)));
		_presetBtnContainer.Visible = false;

        _premadeCodeList = _instructionCodeHandler.GetPrecodes();

        _loading.Visible = false;

        if (AccountManager.GetUser() != null && AccountManager.GetUser().Role == "Teacher")
        {
			PresetCodeFileSaver.GetPresetCodeFile();
        }
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(AccountManager.GetUser() != null && AccountManager.GetUser().Role == "Teacher")
		{
			_presetBtnContainer.Visible = true;
		}
		else
		{
            _presetBtnContainer.Visible = false;
        }
	}
	private void OpenKeywordInput()
	{
		if(_presetTextureRect.Visible == false)
		{
			_presetTextureRect.Visible = true;
		}
		else
		{
			_presetTextureRect.Visible = false;
		}
	}
    private async void SaveInstructionsAsPreset()
    {
		await SavePresetCode();

        var isOk = _premadeCodeList.AddSetOfCodes(_codeKeyword.GetText(), _assembledCode.GetText()); 
		if(isOk.errNum > 0)
		{
			_notificationHandler.MessageBox(isOk.message, 1);

            _loading.Visible = false;

            return;
		}
		else
		{
            _notificationHandler.MessageBox(isOk.message, 0);
        }
        _codeKeyword.Clear();

        _loading.Visible = false;

        _presetTextureRect.Visible = false;
    }

	private void ExitPresetPanel()
	{
		_presetTextureRect.Visible = false;
    }
	private async Task SavePresetCode()
	{
        _loading.Visible = true;
        await Task.Delay(2000);
    }
}
