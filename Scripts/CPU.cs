using Godot;
using System;

public partial class CPU : Node
{
    public long IOint = 0;
    public string IO = "00000000";
    private int ar = 0x00000000;
    private int pc = 0x00000000;
    private int dr = 0x00000000;
    private int tr = 0x00000000;
    private int ir = 0x00000000;
    private int r = 0x00000000;
    private int ac = 0x00000000;
    private int z = 0;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
