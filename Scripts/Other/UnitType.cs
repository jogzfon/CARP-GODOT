using Godot;
using System;

public partial class UnitType : Label
{
	[Export] private CheckButton _microprogrammed;
    [Export] private CheckButton _hardwired;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _microprogrammed.ButtonPressed = true;
        _hardwired.ButtonPressed = false;

        AnimationManager.unitType = UnitTypes.Microprogrammed;

        _microprogrammed.Connect("pressed", new Callable(this, nameof(MicroprogrammedUnit)));
        _hardwired.Connect("pressed", new Callable(this, nameof(HardwiredUnit)));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    public void HardwiredUnit()
    {
        _microprogrammed.ButtonPressed = false;
        _hardwired.ButtonPressed = true;
        AnimationManager.unitType = UnitTypes.Hardwired;
    }
    public void MicroprogrammedUnit()
    {
        _microprogrammed.ButtonPressed = true;
        _hardwired.ButtonPressed = false;
        AnimationManager.unitType = UnitTypes.Microprogrammed;
    }
}
