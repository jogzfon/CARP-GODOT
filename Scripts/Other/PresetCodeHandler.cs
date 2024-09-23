using Godot;
using System;

public partial class PresetCodeHandler : Node
{
	[Export] private TextEdit _assembledCode;
    [Export] private LineEdit _codeKeyword;
    [Export] private Button _savePresetBtn;
    [Export] private Button _finalizeSavePresetBtn;
    [Export] private TextureRect _presetTextureRect;

	[Export] private InstructionCodeHandler _instructionCodeHandler;
    [Export] private NotificationHandler _notificationHandler;

    private PremadeCodeList _premadeCodeList;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_presetTextureRect.Visible = false;
        _savePresetBtn.Connect("pressed", new Callable(this, nameof(OpenKeywordInput)));
        _finalizeSavePresetBtn.Connect("pressed", new Callable(this, nameof(SaveInstructionsAsPreset)));

        _premadeCodeList = _instructionCodeHandler.GetPrecodes();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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
    private void SaveInstructionsAsPreset()
    {
		var isOk = _premadeCodeList.AddSetOfCodes(_codeKeyword.Text, _assembledCode.Text); 
		if(isOk.errNum > 0)
		{
			_notificationHandler.MessageBox(isOk.message, 1);
			return;
		}
		else
		{
            _notificationHandler.MessageBox(isOk.message, 0);
        }
        _codeKeyword.Clear();
		_presetTextureRect.Visible = false;
    }
}
