using Godot;
using System;
using System.Threading.Tasks;

[GlobalClass]
public partial class Animations : Node
{
    #region CARPVar
    Sprite2D cpuOut;
	Sprite2D cpuIn;
	Sprite2D aOut;
	Sprite2D aIn;
	Sprite2D dOut;
	Sprite2D dIn;
    Sprite2D mIn;
    Sprite2D mOut1;
    Sprite2D mOut2;
    Sprite2D ioInV;
    Sprite2D ioInH;
    Sprite2D io;
    Sprite2D outIO;
    Sprite2D vDc;
    Sprite2D hDc;
    Sprite2D dcIn;
    Sprite2D vDc2;
    Sprite2D hDc2;
    Sprite2D dcIn2;
    Sprite2D dcIn3;
    Sprite2D dcOut1;
    Sprite2D dcOut2;
    Sprite2D read;
    Sprite2D write;
    Sprite2D clkOut;
    Sprite2D clkV;
    Sprite2D clkH;
    Sprite2D clkEnd;

    Line2D line;
    Line2D line1;
    Line2D line2;
    Line2D line3;
    Line2D line4;
    Line2D line5;
    Line2D line6;
    Line2D line7;
    Line2D line8;
    Line2D line9;
    Line2D line10;
    #endregion

    [Export] private float moveSpeed = 50f;

    Vector2 destination;
    bool redBallExist = false;
    string moveDirection = "right";
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        #region CARPNodes
        cpuIn = GetNode<Sprite2D>("CpuIn");
        cpuOut = GetNode<Sprite2D>("CpuOut");
		aIn = GetNode<Sprite2D>("aIn");
        aOut = GetNode<Sprite2D>("aOut");
        dIn = GetNode<Sprite2D>("dIn");
        dOut = GetNode<Sprite2D>("dOut");
        mIn = GetNode<Sprite2D>("Min");
        mOut1 = GetNode<Sprite2D>("Mout1");
        mOut2 = GetNode<Sprite2D>("Mout2");
        ioInV = GetNode<Sprite2D>("ioInV");
        ioInH = GetNode<Sprite2D>("ioInH");
        io = GetNode<Sprite2D>("IO");
        vDc = GetNode<Sprite2D>("vDc");
        hDc = GetNode<Sprite2D>("hDc");
        dcIn = GetNode<Sprite2D>("dcIn");
        vDc2 = GetNode<Sprite2D>("vDc2");
        hDc2 = GetNode<Sprite2D>("hDc2");
        dcIn2 = GetNode<Sprite2D>("dcIn2");
        outIO = GetNode<Sprite2D>("outIO");
        dcIn3 = GetNode<Sprite2D>("dcIn3");
        dcOut1 = GetNode<Sprite2D>("dcOut1");
        dcOut2 = GetNode<Sprite2D>("dcOut2");
        read = GetNode<Sprite2D>("read");
        write = GetNode<Sprite2D>("write");
        clkOut = GetNode<Sprite2D>("clkOut");
        clkV = GetNode<Sprite2D>("clkV");
        clkH = GetNode<Sprite2D>("clkH");
        clkEnd = GetNode<Sprite2D>("clkEnd");
        //Lines
        line = GetNode<Line2D>("Line");
        line1 = GetNode<Line2D>("Line2");
        line2 = GetNode<Line2D>("Line3");
        line3 = GetNode<Line2D>("Line4");
        line4 = GetNode<Line2D>("Line5");
        line5 = GetNode<Line2D>("Line6");
        line6 = GetNode<Line2D>("Line7");
        line7 = GetNode<Line2D>("Line8");
        line8 = GetNode<Line2D>("Line9");
        line9 = GetNode<Line2D>("Line10");
        line10 = GetNode<Line2D>("Line11");
        #endregion

        DrawSystemLines();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (Input.IsKeyPressed(Key.Z))
        {
            FETCH1();
        }
        if (Input.IsKeyPressed(Key.X))
        {
            FETCH2();
        }
        if (Input.IsKeyPressed(Key.C))
        {
            FETCH3();
        }
    }

    public async void FETCH1()
    {
        await Task.WhenAll(CPUToA());
        await Task.WhenAll(AToVDc());
        await Task.WhenAll(VDcToM(),vDcTohDc());
        await Task.WhenAll(hDcToDc());
    }
    public async void FETCH2()
    {
        await Task.WhenAll(MToioInV());
        await Task.WhenAll(ioInVToD());
        await Task.WhenAll(DToCPU());
    }
    public async void FETCH3()
    {
        await Task.WhenAll(CPUToA());
        await Task.WhenAll(AToVDc());
        await Task.WhenAll(VDcToM(), vDcTohDc());
        await Task.WhenAll(hDcToDc());
    }

    #region Draw System Lines
    public void DrawSystemLines()
    {
        Line1();
        Line2();
        Line3();
        Line4();
        Line5();
        Line6();
        Line7();
        Line8();
        Line9();
        Line10();
        Line11();
    }
    public void Line1()
    {
        line.AddPoint(aOut.Position);
        line.AddPoint(vDc.Position);
        line.AddPoint(mIn.Position);
    }
    public void Line2()
    {
        line1.AddPoint(dIn.Position);
        line1.AddPoint(ioInV.Position);
        line1.AddPoint(mOut1.Position);
    }
    public void Line3()
    {
        line2.AddPoint(vDc.Position);
        line2.AddPoint(hDc.Position);
        line2.AddPoint(dcIn.Position);
    }
    public void Line4()
    {
        line3.AddPoint(ioInV.Position);
        line3.AddPoint(ioInH.Position);
        line3.AddPoint(io.Position);
    }
    public void Line5()
    {
        line4.AddPoint(mOut2.Position);
        line4.AddPoint(vDc2.Position);
        line4.AddPoint(hDc2.Position);
        line4.AddPoint(dcIn2.Position);
    }
    public void Line6()
    {
        line5.AddPoint(outIO.Position);
        line5.AddPoint(dcIn3.Position);
    }
    public void Line7()
    {
        line6.AddPoint(dcOut1.Position);
        line6.AddPoint(read.Position);
    }
    public void Line8()
    {
        line7.AddPoint(dcOut2.Position);
        line7.AddPoint(write.Position);
    }
    public void Line9()
    {
        line8.AddPoint(clkOut.Position);
        line8.AddPoint(clkV.Position);
        line8.AddPoint(clkH.Position);
        line8.AddPoint(clkEnd.Position);
    }
    public void Line10()
    {
        line9.AddPoint(cpuOut.Position);
        line9.AddPoint(aIn.Position);
    }
    public void Line11()
    {
        line10.AddPoint(cpuIn.Position);
        line10.AddPoint(dOut.Position);
    }
    #endregion
    #region Data Animation Movements
    public async Task CPUToA()
    {
        CharacterBody2D redBall = (CharacterBody2D)ResourceLoader.Load<PackedScene>("res://Scenes/data_ball.tscn").Instantiate();
        GetTree().Root.AddChild(redBall);
        redBall.Position = cpuOut.Position;
        redBall.Name = "RedBall";

        destination = aIn.Position;

        await MoveToPosition(redBall, destination);
    }
    public async Task AToVDc()
    {
        CharacterBody2D redBall = (CharacterBody2D)ResourceLoader.Load<PackedScene>("res://Scenes/data_ball.tscn").Instantiate();
        GetTree().Root.AddChild(redBall);
        redBall.Position = aOut.Position;
        redBall.Name = "RedBall";

        destination = vDc.Position;

        await MoveToPosition(redBall, destination);
    }
    public async Task VDcToM()
    {
        CharacterBody2D redBall = (CharacterBody2D)ResourceLoader.Load<PackedScene>("res://Scenes/data_ball.tscn").Instantiate();
        GetTree().Root.AddChild(redBall);
        redBall.Position = vDc.Position;
        redBall.Name = "RedBall";

        destination = mIn.Position;

        await MoveToPosition(redBall, destination);
    }
    public async Task DToCPU()
    {
        CharacterBody2D redBall = (CharacterBody2D)ResourceLoader.Load<PackedScene>("res://Scenes/data_ball.tscn").Instantiate();
        GetTree().Root.AddChild(redBall);
        redBall.Position = dOut.Position;
        redBall.Name = "RedBall";

        destination = cpuIn.Position;

        await MoveToPosition(redBall, destination);
    }
    public async Task ioInVToD()
    {
        CharacterBody2D redBall = (CharacterBody2D)ResourceLoader.Load<PackedScene>("res://Scenes/data_ball.tscn").Instantiate();
        GetTree().Root.AddChild(redBall);
        redBall.Position = ioInV.Position;
        redBall.Name = "RedBall";

        destination = dIn.Position;

        await MoveToPosition(redBall, destination);
    }
    public async Task MToioInV()
    {
        CharacterBody2D redBall = (CharacterBody2D)ResourceLoader.Load<PackedScene>("res://Scenes/data_ball.tscn").Instantiate();
        GetTree().Root.AddChild(redBall);
        redBall.Position = mOut1.Position;
        redBall.Name = "RedBall";

        destination = ioInV.Position;

        await MoveToPosition(redBall, destination);
    }
    public async Task vDcTohDc()
    {
        CharacterBody2D redBall = (CharacterBody2D)ResourceLoader.Load<PackedScene>("res://Scenes/data_ball.tscn").Instantiate();
        GetTree().Root.AddChild(redBall);
        redBall.Position = vDc.Position;
        redBall.Name = "RedBall";

        destination = hDc.Position;

        await MoveToPosition(redBall, destination);
    }
    public async Task hDcToDc()
    {
        CharacterBody2D redBall = (CharacterBody2D)ResourceLoader.Load<PackedScene>("res://Scenes/data_ball.tscn").Instantiate();
        GetTree().Root.AddChild(redBall);
        redBall.Position = hDc.Position;
        redBall.Name = "RedBall";

        destination = dcIn.Position;

        await MoveToPosition(redBall, destination);
    }

    private async Task MoveToPosition(CharacterBody2D character, Vector2 destination)
    {
        while (character.Position != destination)
        {
            if(destination == mIn.Position || destination == dIn.Position)
            {
                character.Position = character.Position.MoveToward(destination, (float)((moveSpeed*2.1) * GetProcessDeltaTime()));
                await ToSignal(GetTree().CreateTimer(0.01f), "timeout");
            }
            else
            {
                character.Position = character.Position.MoveToward(destination, (float)(moveSpeed * GetProcessDeltaTime()));
                await ToSignal(GetTree().CreateTimer(0.01f), "timeout");
            }
        }
        character.QueueFree();
    }
    #endregion
    #region Color Me Lines
    public void MemoryColor()
    {
        if(line4.DefaultColor == Colors.White)
        {
            line4.DefaultColor = Colors.Red;
        }
        else
        {
            line4.DefaultColor = Colors.White;
        }
    }
    public void ReadColor()
    {
        if (line6.DefaultColor == Colors.White)
        {
            line6.DefaultColor = Colors.Red;
        }
        else
        {
            line6.DefaultColor = Colors.White;
        }
    }
    public void WriteColor()
    {
        if (line7.DefaultColor == Colors.White)
        {
            line7.DefaultColor = Colors.Red;
        }
        else
        {
            line7.DefaultColor = Colors.White;
        }
    }
    public void ClkColor()
    {
        if (line8.DefaultColor == Colors.White)
        {
            line8.DefaultColor = Colors.Red;
        }
        else
        {
            line8.DefaultColor = Colors.White;
        }
    }
    #endregion
}
