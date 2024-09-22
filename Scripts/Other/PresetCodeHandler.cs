using Godot;
using System;

public partial class PresetCodeHandler : Node
{
	[Export] private TextEdit _assembledCode;
    [Export] private Button _savePresetBtn;
    [Export] private TextureRect _presetTextureRect;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_presetTextureRect.Visible = false;
        _savePresetBtn.Connect("pressed", new Callable(this, nameof(OpenKeywordInput)));
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
}
