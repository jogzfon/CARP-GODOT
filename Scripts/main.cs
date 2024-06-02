using Godot;
using System;
using System.Collections.Generic;
public partial class main : Node2D
{
	[Export]
	private Node animationNode;
	private Animations animations;

    [Export] public Label cpuStatus;
    [Export] public Label rtlStatement;
    [Export] public Label dataMovement;
    [Export] public LineEdit currentMemoryLocation;

    LineEdit memoryLocation;
    int memoryStartLocation;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		animations = animationNode as Animations;

        var startAnimation = GetNode<Button>("SystemMenu/HBoxContainer2/VBoxContainer/HBoxContainer2/Start");
        startAnimation.Connect("pressed", new Callable(this, nameof(StartAnimation)));

        var stopAnimation = GetNode<Button>("SystemMenu/HBoxContainer2/VBoxContainer/HBoxContainer2/Stop");
        stopAnimation.Connect("pressed", new Callable(this, nameof(StopAnimation)));

        var resetRegisters = GetNode<Button>("SystemMenu/HBoxContainer2/VBoxContainer2/ResetRegisters");
        resetRegisters.Connect("pressed", new Callable(this, nameof(ResetRegisters)));

        var stepThroughCycle = GetNode<Button>("SystemMenu/HBoxContainer2/VBoxContainer2/StepThroughCycle");
        stepThroughCycle.Connect("pressed", new Callable(this, nameof(StepThroughCycle)));

        var stepThroughInstruction = GetNode<Button>("SystemMenu/HBoxContainer2/VBoxContainer2/StepThroughInstruction");
        stepThroughInstruction.Connect("pressed", new Callable(this, nameof(StepThroughInstruction)));

        memoryLocation = GetNode<LineEdit>("SystemMenu/HBoxContainer2/VBoxContainer/HBoxContainer/MemoryLocation");
        memoryStartLocation = Int32.Parse(memoryLocation.Text);

        animations.SetRequirements(cpuStatus, rtlStatement, dataMovement, currentMemoryLocation);
    }
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void StartAnimation()
	{
		animations.StartAnimation(memoryStartLocation);
    }
    private void StopAnimation()
    {
        animations.StopAnimation();
    }
    private void ResetRegisters()
    {
        animations.ResetRegisters();
    }
    private void StepThroughCycle()
    {
        animations.StepThroughCycle();
    }
    private void StepThroughInstruction()
    {
        animations.StepThroughInstruction();
    }
}
