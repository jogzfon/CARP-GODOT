using Godot;
using System;
using System.Threading.Tasks;

public partial class AnimationHandler : Node
{
    [Export]
    private Node AnimationsScript;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (Input.IsKeyPressed(Key.Q))
        {
            AnimationsScript.Call("CPUToA");
        }
        if (Input.IsKeyPressed(Key.W))
        {
            AnimationsScript.Call("AToVDc");
        }
        if (Input.IsKeyPressed(Key.E))
        {
            AnimationsScript.Call("VDcToM");
        }
        if (Input.IsKeyPressed(Key.R))
        {
            AnimationsScript.Call("DToCPU");
        }
        if (Input.IsKeyPressed(Key.T))
        {
            AnimationsScript.Call("ioInVToD");
        }
        if (Input.IsKeyPressed(Key.Y))
        {
            AnimationsScript.Call("MToioInV");
        }
        if (Input.IsKeyPressed(Key.U))
        {
            AnimationsScript.Call("vDcTohDc");
        }
        if (Input.IsKeyPressed(Key.I))
        {
            AnimationsScript.Call("hDcToDc");
        }
        if (Input.IsKeyPressed(Key.H))
        {
            AnimationsScript.Call("MemoryColor");
        }
        if (Input.IsKeyPressed(Key.J))
        {
            AnimationsScript.Call("ReadColor");
        }
        if (Input.IsKeyPressed(Key.K))
        {
            AnimationsScript.Call("WriteColor");
        }
        if (Input.IsKeyPressed(Key.L))
        {
            AnimationsScript.Call("ClkColor"); 
        }
    }
 


}
