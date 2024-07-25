using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

public partial class Animations : Node
{
    [Export] public Control systemMenu;

    private Label cpuStatus;
    private Label rtlStatement;
    private Label dataMovement;
    private LineEdit currentMemoryLocation;

    private List<int> breakpoints;
    private bool breaks = false;

    int i = 0;
    private bool animationRunning = false;
    private bool stepThroughCycle = false;
    private bool stepThroughInstruction = false;
    private bool stopAnimation = false;

    #region Animation Settings
    [Export] private float moveSpeed = 50f;

    Vector2 destination;
    bool redBallExist = false;
    string moveDirection = "right";
    #endregion

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

    #region Labels
    [Export] Label en1;
    [Export] Label en2;
    [Export] Label readL;
    [Export] Label writeL;
    [Export] Label clock;
    [Export] Label clk;
    [Export] Label ar;
    [Export] Label pc;
    [Export] Label dr;
    [Export] Label tr;
    [Export] Label ir;
    [Export] Label r;
    [Export] Label ac;
    [Export] Label z;
    [Export] Label a;
    [Export] Label d;
    [Export] Label cu;
    [Export] Label alu;
    [Export] Label ioB;
    #endregion

    #region LineEdits
    LineEdit AR_txt;
    LineEdit PC_txt;
    LineEdit DR_txt;
    LineEdit TR_txt;
    LineEdit IR_txt;
    LineEdit R_txt;
    LineEdit AC_txt;
    LineEdit Z_txt;

    bool isHex = false;
    #endregion

    #region VisualizationOpcodes
    public const short opcodeNOP = 0x00;
    public const short opcodeLDAC = 0x01;
    public const short opcodeSTAC = 0x02;
    public const short opcodeMVAC = 0x03;
    public const short opcodeMOVR = 0x04;
    public const short opcodeJUMP = 0x05;
    public const short opcodeJMPZ = 0x06;
    public const short opcodeJPNZ = 0x07;
    public const short opcodeADD = 0x08;
    public const short opcodeSUB = 0x09;
    public const short opcodeINAC = 0x0a;
    public const short opcodeCLAC = 0x0b;
    public const short opcodeAND = 0x0c;
    public const short opcodeOR = 0x0d;
    public const short opcodeXOR = 0x0e;
    public const short opcodeNOT = 0x0f;
    public const short opcodeEND = 0xff;

    short[] memorycode;
  
    short instructadv1;
    short instructadv2;

    public long IOint = 0;
    public string IO = "00000000";
    private int ar_bit = 0x00000000;
    private int pc_bit = 0x00000000;
    private int dr_bit = 0x00000000;
    private int tr_bit = 0x00000000;
    private int ir_bit = 0x00000000;
    private int r_bit = 0x00000000;
    private int ac_bit = 0x00000000;
    private int z_bit = 0;
    #endregion

    system_menu sysmenu;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        sysmenu = systemMenu as system_menu;
        #region Animation Requirements
        var node = GetNode<Control>("/root/ProjectPage");
        project_page proj = node as project_page;

        this.breakpoints = proj.breakpointList;

        if(AccountManager.GetUser() != null)
        {
            Int32.TryParse(DataToSave.currentMemoryLocation, out i);
        }
        
        memorycode = Memory.contents;
        TraceResults.RemoveAllStatements();
        #endregion

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

        #region LineEdits Registers
        AR_txt = systemMenu.GetNode<LineEdit>("HBoxContainer/VBoxContainer/HBoxContainer/AR");
        PC_txt = systemMenu.GetNode<LineEdit>("HBoxContainer/VBoxContainer/HBoxContainer2/PC");
        DR_txt = systemMenu.GetNode<LineEdit>("HBoxContainer/VBoxContainer/HBoxContainer3/DR");
        TR_txt = systemMenu.GetNode<LineEdit>("HBoxContainer/VBoxContainer/HBoxContainer4/TR");
        IR_txt = systemMenu.GetNode<LineEdit>("HBoxContainer/VBoxContainer2/HBoxContainer4/IR");
        R_txt = systemMenu.GetNode<LineEdit>("HBoxContainer/VBoxContainer2/HBoxContainer5/R");
        AC_txt = systemMenu.GetNode<LineEdit>("HBoxContainer/VBoxContainer2/HBoxContainer6/AC");
        Z_txt = systemMenu.GetNode<LineEdit>("HBoxContainer/VBoxContainer2/HBoxContainer7/Z");

        #endregion

        DrawSystemLines();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        ioB.Text = IO;
        if (currentMemoryLocation != null)
        {
            if (animationRunning && !stepThroughCycle && !stepThroughInstruction)
            {
                currentMemoryLocation.Text = i.ToString();
                sysmenu.start_location.Text = i.ToString();
            }
            else {

                i = Int32.Parse(sysmenu.start_location.Text);
            }

            //Save Data
            if (AccountManager.GetUser() != null)
            {
                DataToSave.currentMemoryLocation = i.ToString();

                DataToSave.ar = ar_bit;
                DataToSave.pc = pc_bit;
                DataToSave.dr = dr_bit;
                DataToSave.tr = tr_bit;
                DataToSave.ir = ir_bit;
                DataToSave.r = r_bit;
                DataToSave.ac = ac_bit;
                DataToSave.z = z_bit;
            }
            foreach(int brake in breakpoints){
                if (i == brake)
                {
                    breaks = true;
                    break;
                }
            }
        }
    }
    #region Animation Controls
    public async void StartAnimation(int memoryStartLocation)
    {
        if (stepThroughCycle || stepThroughInstruction)
        {
            stepThroughInstruction = false;
            stepThroughCycle = false;
        }
        else if (!animationRunning && !breaks)
        {
            animationRunning = true;

            ar_bit = BinaryStringToInt(AR_txt.Text);
            pc_bit = BinaryStringToInt(PC_txt.Text);
            dr_bit = BinaryStringToInt(DR_txt.Text);
            tr_bit = BinaryStringToInt(TR_txt.Text);
            ir_bit = BinaryStringToInt(IR_txt.Text);
            r_bit = BinaryStringToInt(R_txt.Text);
            ac_bit = BinaryStringToInt(AC_txt.Text);
            z_bit = BinaryStringToInt(Z_txt.Text);

            for (i = memoryStartLocation; i < memorycode.Length; i++)
            {
                await FETCH1();
                await ClockPulse();
                if (PauseOrStopAnimation())
                {
                    return;
                }
                await StepCycle();
                await FETCH2();
                await ClockPulse();
                if (PauseOrStopAnimation())
                {
                    return;
                }
                await StepCycle();
                await FETCH3();
                await ClockPulse();
                if (PauseOrStopAnimation())
                {
                    return;
                }
                await StepCycle();
                switch (memorycode[i])
                {
                    case opcodeNOP:
                        Debug.Print("NOP encountered");
                        await NOP();
                        await StepCycle();
                        break;
                    case opcodeLDAC:
                        Debug.Print("LDAC encountered");
                        i++;
                        await LDAC1();
                        await ClockPulse();
                        if (PauseOrStopAnimation())
                        {
                            return;
                        }
                        await StepCycle();
                        await LDAC2();
                        await ClockPulse();
                        if (PauseOrStopAnimation())
                        {
                            return;
                        }
                        await StepCycle();
                        await LDAC3();
                        await ClockPulse();
                        if (PauseOrStopAnimation())
                        {
                            return;
                        }
                        await StepCycle();
                        await LDAC4();
                        await ClockPulse();
                        if (PauseOrStopAnimation())
                        {
                            return;
                        }
                        await StepCycle();
                        await LDAC5();
                        break;
                    case opcodeSTAC:
                        Debug.Print("STAC encountered");
                        i++;
                        await STAC1();
                        await ClockPulse();
                        if (PauseOrStopAnimation())
                        {
                            return;
                        }
                        await StepCycle();
                        await STAC2();
                        await ClockPulse();
                        if (PauseOrStopAnimation())
                        {
                            return;
                        }
                        await StepCycle();
                        await STAC3();
                        await ClockPulse();
                        if (PauseOrStopAnimation())
                        {
                            return;
                        }
                        await StepCycle();
                        await STAC4();
                        await ClockPulse();
                        if (PauseOrStopAnimation())
                        {
                            return;
                        }
                        await StepCycle();
                        await STAC5();
                        break;
                    case opcodeMVAC:
                        Debug.Print("MVAC encountered");
                        await MVAC();
                        break;
                    case opcodeMOVR:
                        Debug.Print("MOVR encountered");
                        await MOVR();
                        break;
                    case opcodeJUMP:
                        Debug.Print("JUMP encountered");
                        i++;
                        await JUMP();
                        break;
                    case opcodeJMPZ:
                        Debug.Print("JMPZ encountered");
                        i++;
                        await JMPZ();
                        break;
                    case opcodeJPNZ:
                        Debug.Print("JPNZ encountered");
                        i++;
                        await JPNZ();
                        break;
                    case opcodeADD:
                        Debug.Print("ADD encountered");
                        await ADD();
                        break;
                    case opcodeSUB:
                        Debug.Print("SUB encountered");
                        await SUB();
                        break;
                    case opcodeINAC:
                        Debug.Print("INAC encountered");
                        await INAC();
                        break;
                    case opcodeCLAC:
                        Debug.Print("CLAC encountered");
                        await CLAC();
                        break;
                    case opcodeAND:
                        Debug.Print("AND encountered");
                        await AND();
                        break;
                    case opcodeOR:
                        Debug.Print("OR encountered");
                        await OR();
                        break;
                    case opcodeXOR:
                        Debug.Print("XOR encountered");
                        await XOR();
                        break;
                    case opcodeNOT:
                        Debug.Print("NOT encountered");
                        await NOT();
                        break;
                    case opcodeEND:
                        Debug.Print("END encountered");
                        animationRunning = false;
                        await END();
                        return;
                    default:
                        // Handle unknown instructions or implement additional instructions
                        Debug.Print("Instruction Does not Exist");
                        break;
                }
                await ClockPulse();
                if (PauseOrStopAnimation())
                {
                    return;
                }
                await StepCycle();
                await StepInstruction();
            }
            animationRunning = false;
        }
        else if (!animationRunning && breaks)
        {
            breaks = false;
            animationRunning = true;

            ar_bit = BinaryStringToInt(AR_txt.Text);
            pc_bit = BinaryStringToInt(PC_txt.Text);
            dr_bit = BinaryStringToInt(DR_txt.Text);
            tr_bit = BinaryStringToInt(TR_txt.Text);
            ir_bit = BinaryStringToInt(IR_txt.Text);
            r_bit = BinaryStringToInt(R_txt.Text);
            ac_bit = BinaryStringToInt(AC_txt.Text);
            z_bit = BinaryStringToInt(Z_txt.Text);


            for (i = i+1; i < memorycode.Length; i++)
            {
                await FETCH1();
                await ClockPulse();
                if (PauseOrStopAnimation())
                {
                    return;
                }
                await StepCycle();
                await FETCH2();
                await ClockPulse();
                if (PauseOrStopAnimation())
                {
                    return;
                }
                await StepCycle();
                await FETCH3();
                await ClockPulse();
                if (PauseOrStopAnimation())
                {
                    return;
                }
                await StepCycle();
                switch (memorycode[i])
                {
                    case opcodeNOP:
                        Debug.Print("NOP encountered");
                        await NOP();
                        await StepCycle();
                        break;
                    case opcodeLDAC:
                        Debug.Print("LDAC encountered");
                        i++;
                        await LDAC1();
                        await ClockPulse();
                        if (PauseOrStopAnimation())
                        {
                            return;
                        }
                        await StepCycle();
                        await LDAC2();
                        await ClockPulse();
                        if (PauseOrStopAnimation())
                        {
                            return;
                        }
                        await StepCycle();
                        await LDAC3();
                        await ClockPulse();
                        if (PauseOrStopAnimation())
                        {
                            return;
                        }
                        await StepCycle();
                        await LDAC4();
                        await ClockPulse();
                        if (PauseOrStopAnimation())
                        {
                            return;
                        }
                        await StepCycle();
                        await LDAC5();
                        break;
                    case opcodeSTAC:
                        Debug.Print("STAC encountered");
                        i++;
                        await STAC1();
                        await ClockPulse();
                        if (PauseOrStopAnimation())
                        {
                            return;
                        }
                        await StepCycle();
                        await STAC2();
                        await ClockPulse();
                        if (PauseOrStopAnimation())
                        {
                            return;
                        }
                        await StepCycle();
                        await STAC3();
                        await ClockPulse();
                        if (PauseOrStopAnimation())
                        {
                            return;
                        }
                        await StepCycle();
                        await STAC4();
                        await ClockPulse();
                        if (PauseOrStopAnimation())
                        {
                            return;
                        }
                        await StepCycle();
                        await STAC5();
                        break;
                    case opcodeMVAC:
                        Debug.Print("MVAC encountered");
                        await MVAC();
                        break;
                    case opcodeMOVR:
                        Debug.Print("MOVR encountered");
                        await MOVR();
                        break;
                    case opcodeJUMP:
                        Debug.Print("JUMP encountered");
                        i++;
                        await JUMP();
                        break;
                    case opcodeJMPZ:
                        Debug.Print("JMPZ encountered");
                        i++;
                        await JMPZ();
                        break;
                    case opcodeJPNZ:
                        Debug.Print("JPNZ encountered");
                        i++;
                        await JPNZ();
                        break;
                    case opcodeADD:
                        Debug.Print("ADD encountered");
                        await ADD();
                        break;
                    case opcodeSUB:
                        Debug.Print("SUB encountered");
                        await SUB();
                        break;
                    case opcodeINAC:
                        Debug.Print("INAC encountered");
                        await INAC();
                        break;
                    case opcodeCLAC:
                        Debug.Print("CLAC encountered");
                        await CLAC();
                        break;
                    case opcodeAND:
                        Debug.Print("AND encountered");
                        await AND();
                        break;
                    case opcodeOR:
                        Debug.Print("OR encountered");
                        await OR();
                        break;
                    case opcodeXOR:
                        Debug.Print("XOR encountered");
                        await XOR();
                        break;
                    case opcodeNOT:
                        Debug.Print("NOT encountered");
                        await NOT();
                        break;
                    case opcodeEND:
                        Debug.Print("END encountered");
                        animationRunning = false;
                        await END();
                        return;
                    default:
                        // Handle unknown instructions or implement additional instructions
                        Debug.Print("Instruction Does not Exist");
                        break;
                }
                await ClockPulse();
                if (PauseOrStopAnimation())
                {
                    return;
                }
                await StepCycle();
                await StepInstruction();
            }
            animationRunning = false;
        }
    }
    public bool PauseOrStopAnimation()
    {
        if (stopAnimation == true)
        {
            stopAnimation = false;
            animationRunning = false;
            cpuStatus.Text = "Stopped";
            return true;
        }
        if(breaks == true)
        {
            stopAnimation = false;
            animationRunning = false;
            cpuStatus.Text = "Breaks";
            return true;
        }
        return false;
    }
    public void StopAnimation()
    {
        stopAnimation = true;  
    }
    public void StepThroughCycle()
    {
        if (!animationRunning)
        {
            StartAnimation(Int32.Parse(currentMemoryLocation.Text));
        }

        if (stepThroughCycle)
        {
            stepThroughCycle = false;
        }
        else
        {
            stepThroughCycle = true;
            stepThroughInstruction = false;
        }
    }
    public void StepThroughInstruction()
    {
        if (!animationRunning)
        {
            StartAnimation(Int32.Parse(currentMemoryLocation.Text));
        }

        if (stepThroughInstruction)
        {
            stepThroughInstruction = false;
        }
        else
        {
            stepThroughInstruction = true;
            stepThroughCycle = false;
        }
    }
    public void ResetRegisters()
    {
        AR_txt.Text = "0000 0000 0000 0000";
        PC_txt.Text = "0000 0000 0000 0000";
        DR_txt.Text = "0000 0000";
        TR_txt.Text = "0000 0000";
        IR_txt.Text = "0000 0000";
        R_txt.Text = "0000 0000";
        AC_txt.Text = "0000 0000";
        Z_txt.Text = "0";

        IOint = 0;
        IO = "00000000";
        ar_bit = 0x00000000;
        pc_bit = 0x00000000;
        dr_bit = 0x00000000;
        tr_bit = 0x00000000;
        ir_bit = 0x00000000;
        r_bit = 0x00000000;
        ac_bit = 0x00000000;
        z_bit = 0;
    }
    #endregion

    #region Animations
    #region FETCH
    public async Task FETCH1()
    {
        Write(0);
        Read(0);
        EN1(0);
        WriteColor(0);
        ReadColor(0);
        MemoryColor(0);

        AR(1);
        PC(1);
        DR(0);
        TR(0);
        IR(0);
        R(0);
        AC(0);
        Z(0);

        ARLineColor(1);
        PCLineColor(1);
        DRLineColor(0);
        TRLineColor(0);
        IRLineColor(0);
        RLineColor(0);
        ACLineColor(0);
        ZLineColor(0);

        ar_bit = pc_bit;
        cpuStatus.Text = "Running";
        rtlStatement.Text = "Fetch 1";
        dataMovement.Text = "AR <- PC";

        if (sysmenu.animationOn)
        {
            await Task.WhenAll(CPUToA());
            await Task.WhenAll(AToVDc());
            await Task.WhenAll(VDcToM(), vDcTohDc());
            await Task.WhenAll(hDcToDc());
        }

        AR_txt.Text = SpaceInserter(ar_bit, "ar");
        PC_txt.Text = SpaceInserter(pc_bit, "pc");
        DR_txt.Text = SpaceInserter(dr_bit, "dr");
        TR_txt.Text = SpaceInserter(tr_bit, "tr");
        IR_txt.Text = SpaceInserter(ir_bit, "ir");
        R_txt.Text = SpaceInserter(r_bit, "r");
        AC_txt.Text = SpaceInserter(ac_bit, "ac");
        Z_txt.Text = SpaceInserter(z_bit, "z");

        Read(1);
        EN1(1);
        ReadColor(1);
        MemoryColor(1);

        TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public async Task FETCH2()
    {
        AR(0);
        PC(1);
        DR(1);

        ARLineColor(0);
        PCLineColor(1);
        DRLineColor(1);

        pc_bit = pc_bit + 1;
        dr_bit = memorycode[i];
        rtlStatement.Text = "Fetch 2";
        dataMovement.Text = "DR <- M, PC <- PC+1";
        
        if (sysmenu.animationOn)
        {
            await Task.WhenAll(MToioInV());
            await Task.WhenAll(ioInVToD());
            await Task.WhenAll(DToCPU());
        }

        PC_txt.Text = SpaceInserter(pc_bit, "pc");
        DR_txt.Text = SpaceInserter(dr_bit, "dr");

        Read(0);
        EN1(0);
        ReadColor(0);
        MemoryColor(0);

        TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public async Task FETCH3()
    {
        PC(1);
        DR(1);
        IR(1);
        AR(1);

        ARLineColor(1);
        PCLineColor(1);
        DRLineColor(1);
        IRLineColor(1);

        ar_bit = pc_bit;
        ir_bit = dr_bit;
        rtlStatement.Text = "Fetch 3";
        dataMovement.Text = "IR <- DR, AR <- PC";

        if (sysmenu.animationOn)
        {
            await Task.WhenAll(CPUToA());
            await Task.WhenAll(AToVDc());
            await Task.WhenAll(VDcToM(), vDcTohDc());
            await Task.WhenAll(hDcToDc());
        }

        AR_txt.Text = SpaceInserter(ar_bit, "ar");
        IR_txt.Text = SpaceInserter(ir_bit, "ir");

        TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    #endregion
    #region DataMovement
    #region LDAC
    public async Task LDAC1()
    {
        Read(1);
        EN1(1);
        ReadColor(1);
        MemoryColor(1);

        AR(1);
        PC(1);
        DR(1);
        TR(0);
        IR(0);
        R(0);
        AC(0);
        Z(0);
        Read(0);
        Write(0);

        ARLineColor(1);
        PCLineColor(1);
        DRLineColor(1);
        TRLineColor(0);
        IRLineColor(0);
        RLineColor(0);
        ACLineColor(0);
        ZLineColor(0);

        dr_bit = memorycode[i];
        ar_bit = ar_bit + 1;
        pc_bit = pc_bit + 1;

        rtlStatement.Text = "LDAC 1";
        dataMovement.Text = "DR <- M, PC <- PC + 1, AR <- AR + 1";

        if (sysmenu.animationOn)
        {
            await Task.WhenAll(CPUToA(), MToioInV());
            await Task.WhenAll(AToVDc(), ioInVToD());
            await Task.WhenAll(VDcToM(), vDcTohDc());
            await Task.WhenAll(hDcToDc(), DToCPU());
        }

        AR_txt.Text = SpaceInserter(ar_bit, "ar");
        PC_txt.Text = SpaceInserter(pc_bit, "pc");
        DR_txt.Text = SpaceInserter(dr_bit, "dr");

        TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public async Task LDAC2()
    {
        AR(0);
        PC(1);
        DR(1);
        TR(1);
        IR(0);
        R(0);
        AC(0);
        Z(0);
        Read(0);
        Write(0);

        ARLineColor(0);
        PCLineColor(1);
        DRLineColor(1);
        TRLineColor(1);
        IRLineColor(0);
        RLineColor(0);
        ACLineColor(0);
        ZLineColor(0);

        tr_bit = dr_bit;
        dr_bit = memorycode[i];
        pc_bit = pc_bit + 1;

        rtlStatement.Text = "LDAC 2";
        dataMovement.Text = "TR <- DR, DR <- M, PC = PC + 1";

        if (sysmenu.animationOn)
        {
            await Task.WhenAll(MToioInV());
            await Task.WhenAll(ioInVToD());
            await Task.WhenAll(DToCPU());
        }

        PC_txt.Text = SpaceInserter(pc_bit, "pc");
        DR_txt.Text = SpaceInserter(dr_bit, "dr");
        TR_txt.Text = SpaceInserter(tr_bit, "tr");

        Read(0);
        EN1(0);
        ReadColor(0);
        MemoryColor(0);

        TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public async Task LDAC3()
    {
        AR(1);
        PC(0);
        DR(1);
        TR(1);
        IR(0);
        R(0);
        AC(0);
        Z(0);
        Read(0);
        Write(0);

        ARLineColor(1);
        PCLineColor(0);
        DRLineColor(1);
        TRLineColor(1);
        IRLineColor(0);
        RLineColor(0);
        ACLineColor(0);
        ZLineColor(0);

        ar_bit = dr_bit | tr_bit;

        rtlStatement.Text = "LDAC 3";
        dataMovement.Text = "AR <- DR | TR";

        if (sysmenu.animationOn)
        {
            await Task.WhenAll(CPUToA());
            await Task.WhenAll(AToVDc());
            await Task.WhenAll(VDcToM(), vDcTohDc());
            await Task.WhenAll(hDcToDc());
        }

        AR_txt.Text = SpaceInserter(ar_bit, "ar");

        Read(1);
        EN1(1);
        ReadColor(1);
        MemoryColor(1);

        TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public async Task LDAC4()
    {
        AR(0);
        PC(0);
        DR(1);
        TR(0);
        IR(0);
        R(0);
        AC(0);
        Z(0);
        Read(0);
        Write(0);

        ARLineColor(0);
        PCLineColor(0);
        DRLineColor(1);
        TRLineColor(0);
        IRLineColor(0);
        RLineColor(0);
        ACLineColor(0);
        ZLineColor(0);

        dr_bit = memorycode[i];

        rtlStatement.Text = "LDAC 4";
        dataMovement.Text = "DR <- M";

        if (sysmenu.animationOn)
        {
            await Task.WhenAll(MToioInV());
            await Task.WhenAll(ioInVToD());
            await Task.WhenAll(DToCPU());
        }

        DR_txt.Text = SpaceInserter(dr_bit, "dr");

        Read(0);
        EN1(0);
        ReadColor(0);
        MemoryColor(0);

        TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public async Task LDAC5()
    {
        AR(0);
        PC(0);
        DR(1);
        TR(0);
        IR(0);
        R(0);
        AC(1);
        Z(0);
        Read(0);
        Write(0);

        ARLineColor(0);
        PCLineColor(0);
        DRLineColor(1);
        TRLineColor(0);
        IRLineColor(0);
        RLineColor(0);
        ACLineColor(1);
        ZLineColor(0);

        ac_bit = dr_bit;

        rtlStatement.Text = "LDAC 5";
        dataMovement.Text = "AC <- DR";

        if (sysmenu.animationOn)
        {
            await Task.Delay(1000);
        }
        
        AC_txt.Text = SpaceInserter(ac_bit, "ac");

        TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    #endregion

    #region STAC
    public async Task STAC1()
    {
        Read(1);
        EN1(1);
        ReadColor(1);
        MemoryColor(1);

        AR(1);
        PC(1);
        DR(1);
        TR(0);
        IR(0);
        R(0);
        AC(0);
        Z(0);
        Read(0);
        Write(0);

        ARLineColor(1);
        PCLineColor(1);
        DRLineColor(1);
        TRLineColor(0);
        IRLineColor(0);
        RLineColor(0);
        ACLineColor(0);
        ZLineColor(0);

        dr_bit = memorycode[i];
        ar_bit = ar_bit + 1;
        pc_bit = pc_bit + 1;

        rtlStatement.Text = "STAC 1";
        dataMovement.Text = "DR <- M, PC <- PC + 1, AR <- AR + 1";

        if (sysmenu.animationOn)
        {
            await Task.WhenAll(CPUToA(), MToioInV());
            await Task.WhenAll(AToVDc(), ioInVToD());
            await Task.WhenAll(VDcToM(), vDcTohDc());
            await Task.WhenAll(hDcToDc(), DToCPU());
        }

        AR_txt.Text = SpaceInserter(ar_bit, "ar");
        PC_txt.Text = SpaceInserter(pc_bit, "pc");
        DR_txt.Text = SpaceInserter(dr_bit, "dr");

        TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public async Task STAC2()
    {
        AR(0);
        PC(1);
        DR(1);
        TR(1);
        IR(0);
        R(0);
        AC(0);
        Z(0);
        Read(0);
        Write(0);

        ARLineColor(0);
        PCLineColor(1);
        DRLineColor(1);
        TRLineColor(1);
        IRLineColor(0);
        RLineColor(0);
        ACLineColor(0);
        ZLineColor(0);

        tr_bit = dr_bit;
        dr_bit = memorycode[i];
        pc_bit = pc_bit + 1;

        rtlStatement.Text = "STAC 2";
        dataMovement.Text = "TR <- DR, DR <- M, PC <- PC + 1";

        if (sysmenu.animationOn)
        {
            await Task.WhenAll(MToioInV());
            await Task.WhenAll(ioInVToD());
            await Task.WhenAll(DToCPU());
        }

        PC_txt.Text = SpaceInserter(pc_bit, "pc");
        DR_txt.Text = SpaceInserter(dr_bit, "dr");
        TR_txt.Text = SpaceInserter(tr_bit, "tr");

        TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public async Task STAC3()
    {
        Read(0);
        EN1(0);
        ReadColor(0);
        MemoryColor(0);

        AR(1);
        PC(0);
        DR(1);
        TR(1);
        IR(0);
        R(0);
        AC(0);
        Z(0);
        Read(0);
        Write(0);

        ARLineColor(1);
        PCLineColor(0);
        DRLineColor(1);
        TRLineColor(1);
        IRLineColor(0);
        RLineColor(0);
        ACLineColor(0);
        ZLineColor(0);

        ar_bit = dr_bit | tr_bit;

        rtlStatement.Text = "STAC 3";
        dataMovement.Text = "AR <- DR | TR";

        if (sysmenu.animationOn)
        {
            await Task.WhenAll(CPUToA());
            await Task.WhenAll(AToVDc());
            await Task.WhenAll(VDcToM(), vDcTohDc());
            await Task.WhenAll(hDcToDc());
        }

        AR_txt.Text = SpaceInserter(ar_bit, "ar");

        TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public async Task STAC4()
    {
        AR(0);
        PC(0);
        DR(1);
        TR(0);
        IR(0);
        R(0);
        AC(1);
        Z(0);
        Read(0);
        Write(0);

        ARLineColor(0);
        PCLineColor(0);
        DRLineColor(1);
        TRLineColor(0);
        IRLineColor(0);
        RLineColor(0);
        ACLineColor(1);
        ZLineColor(0);

        dr_bit = ac_bit;

        rtlStatement.Text = "STAC 4";
        dataMovement.Text = "DR <- AC";

        if (sysmenu.animationOn)
        {
            await Task.Delay(1000);
        }

        DR_txt.Text = SpaceInserter(dr_bit, "dr");

        TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public async Task STAC5()
    {
        Write(1);
        EN1(1);
        WriteColor(1);
        MemoryColor(1);

        AR(0);
        PC(0);
        DR(1);
        TR(0);
        IR(0);
        R(0);
        AC(0);
        Z(0);
        Read(0);
        Write(1);

        ARLineColor(0);
        PCLineColor(0);
        DRLineColor(1);
        TRLineColor(0);
        IRLineColor(0);
        RLineColor(0);
        ACLineColor(0);
        ZLineColor(0);

        memorycode[i] = (short)dr_bit;

        rtlStatement.Text = "STAC 5";
        dataMovement.Text = "M <- DR";

        if (sysmenu.animationOn)
        {
            await Task.WhenAll(CPUToD());
            await Task.WhenAll(DToioInV());
            await Task.WhenAll(ioInVToM());
        }

        TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    #endregion

    public async Task MVAC()
    {
        AR(1);
        PC(0);
        DR(0);
        TR(0);
        IR(0);
        R(1);
        AC(0);
        Z(0);
        Read(0);
        Write(0);

        ARLineColor(1);
        PCLineColor(0);
        DRLineColor(0);
        TRLineColor(0);
        IRLineColor(0);
        RLineColor(1);
        ACLineColor(0);
        ZLineColor(0);

        r_bit = ac_bit;
        rtlStatement.Text = "MVAC";
        dataMovement.Text = "R <- AC";

        if (sysmenu.animationOn)
        {
            await Task.Delay(1000);
        }

        R_txt.Text = SpaceInserter(r_bit, "r");

        TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public async Task MOVR()
    {
        AR(0);
        PC(0);
        DR(0);
        TR(0);
        IR(0);
        R(1);
        AC(1);
        Z(0);
        Read(0);
        Write(0);

        ARLineColor(0);
        PCLineColor(0);
        DRLineColor(0);
        TRLineColor(0);
        IRLineColor(0);
        RLineColor(1);
        ACLineColor(1);
        ZLineColor(0);

        ac_bit = r_bit;
        rtlStatement.Text = "MOVR";
        dataMovement.Text = "AC <- R";

        if (sysmenu.animationOn)
        {
            await Task.Delay(1000);
        }

        AC_txt.Text = SpaceInserter(ac_bit, "ac");

        TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public async Task JUMP()
    {
        int position = memorycode[i]-1;
        rtlStatement.Text = "JUMP";
        if (position >= 0 && position < memorycode.Length)
        {
            dataMovement.Text = "Go To " + position;
            TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
            i = position;
        }
        else
        {
            // Handle invalid jump, e.g., by throwing an exception or setting position to a default value
            dataMovement.Text = "Invalid Jump";
            Debug.Print("Invalid memory line JUMP");
        }
        if (sysmenu.animationOn)
        {
            await Task.Delay(1000);
        }
    }
    public async Task JMPZ()
    {
        rtlStatement.Text = "JMPZ";
        if (z_bit == 1)
        {
            int position = memorycode[i]-1;
            dataMovement.Text = "Go To " + position;
            TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
            i = position;
        }
        else
        {
            dataMovement.Text = "Continue";
            TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
        }
        if (sysmenu.animationOn)
        {
            await Task.Delay(1000);
        }
    }
    public async Task JPNZ()
    {
        rtlStatement.Text = "JPNZ";
        if (z_bit == 0)
        {
            int position = memorycode[i] - 1;
            dataMovement.Text = "Go To " + position;
            TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
            i = position;
        }
        else
        {
            dataMovement.Text = "Continue";
            TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
        }
        if (sysmenu.animationOn)
        {
            await Task.Delay(1000);
        }
    }

    public async Task NOP()
    {
        AR(0);
        PC(0);
        DR(0);
        TR(0);
        IR(0);
        R(0);
        AC(0);
        Z(0);
        Read(0);
        Write(0);

        ARLineColor(0);
        PCLineColor(0);
        DRLineColor(0);
        TRLineColor(0);
        IRLineColor(0);
        RLineColor(0);
        ACLineColor(0);
        ZLineColor(0);

        rtlStatement.Text = "NOP";
        dataMovement.Text = "No Operation";

        if (sysmenu.animationOn)
        {
            await Task.Delay(1000);
        }

        TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }

    public async Task END()
    {
        AR(0);
        PC(0);
        DR(0);
        TR(0);
        IR(0);
        R(0);
        AC(0);
        Z(0);
        Read(0);
        Write(0);

        ARLineColor(0);
        PCLineColor(0);
        DRLineColor(0);
        TRLineColor(0);
        IRLineColor(0);
        RLineColor(0);
        ACLineColor(0);
        ZLineColor(0);

        cpuStatus.Text = "Ended";
        rtlStatement.Text = "END";
        dataMovement.Text = "NONE";
        AR_txt.Text = SpaceInserter(ar_bit, "ar");
        PC_txt.Text = SpaceInserter(pc_bit, "pc");
        DR_txt.Text = SpaceInserter(dr_bit, "dr");
        TR_txt.Text = SpaceInserter(tr_bit, "tr");
        IR_txt.Text = SpaceInserter(ir_bit, "ir");
        R_txt.Text = SpaceInserter(r_bit, "r");
        AC_txt.Text = SpaceInserter(ac_bit, "ac");
        Z_txt.Text = SpaceInserter(z_bit, "z");

        if (sysmenu.animationOn)
        {
            await Task.Delay(1000);
        }

        TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    #endregion
    #region Arithmetic
    public async Task ADD()
    {
        AR(0);
        PC(0);
        DR(0);
        TR(0);
        IR(0);
        R(1);
        AC(1);
        Z(1);
        Read(0);
        Write(0);

        ARLineColor(0);
        PCLineColor(0);
        DRLineColor(0);
        TRLineColor(0);
        IRLineColor(0);
        RLineColor(1);
        ACLineColor(1);
        ZLineColor(1);

        ac_bit = ac_bit + r_bit;

        // Set z based on the result
        z_bit = (ac_bit == 0) ? 1 : 0;

        rtlStatement.Text = "ADD";
        dataMovement.Text = "AC <- AC + R";

        if (sysmenu.animationOn)
        {
            await Task.Delay(1000);
        }

        AC_txt.Text = SpaceInserter(ac_bit, "ac");
        Z_txt.Text = SpaceInserter(z_bit, "z");

        TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public async Task SUB()
    {
        AR(0);
        PC(0);
        DR(0);
        TR(0);
        IR(0);
        R(1);
        AC(1);
        Z(1);
        Read(0);
        Write(0);

        ARLineColor(0);
        PCLineColor(0);
        DRLineColor(0);
        TRLineColor(0);
        IRLineColor(0);
        RLineColor(1);
        ACLineColor(1);
        ZLineColor(1);

        ac_bit = ac_bit - r_bit;

        // Set z based on the result
        z_bit = (ac_bit == 0) ? 1 : 0;

        rtlStatement.Text = "SUB";
        dataMovement.Text = "AC <- AC - R";

        if (sysmenu.animationOn)
        {
            await Task.Delay(1000);
        }

        AC_txt.Text = SpaceInserter(ac_bit, "ac");
        Z_txt.Text = SpaceInserter(z_bit, "z");

        TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public async Task INAC()
    {
        AR(0);
        PC(0);
        DR(0);
        TR(0);
        IR(0);
        R(0);
        AC(1);
        Z(1);
        Read(0);
        Write(0);

        ARLineColor(0);
        PCLineColor(0);
        DRLineColor(0);
        TRLineColor(0);
        IRLineColor(0);
        RLineColor(0);
        ACLineColor(1);
        ZLineColor(1);

        ac_bit = ac_bit + 1;

        // Set z based on the result
        z_bit = (ac_bit == 0) ? 1 : 0;

        rtlStatement.Text = "INAC";
        dataMovement.Text = "AC <- AC + 1";

        if (sysmenu.animationOn)
        {
            await Task.Delay(1000);
        }

        AC_txt.Text = SpaceInserter(ac_bit, "ac");
        Z_txt.Text = SpaceInserter(z_bit, "z");

        TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public async Task CLAC()
    {
        AR(0);
        PC(0);
        DR(0);
        TR(0);
        IR(0);
        R(0);
        AC(1);
        Z(1);
        Read(0);
        Write(0);

        ARLineColor(0);
        PCLineColor(0);
        DRLineColor(0);
        TRLineColor(0);
        IRLineColor(0);
        RLineColor(0);
        ACLineColor(1);
        ZLineColor(1);

        ac_bit = 0;
        z_bit = 1;

        rtlStatement.Text = "CLAC";
        dataMovement.Text = "AC <- 0, Z <- 1";

        if (sysmenu.animationOn)
        {
            await Task.Delay(1000);
        }

        AC_txt.Text = SpaceInserter(ac_bit, "ac");
        Z_txt.Text = SpaceInserter(z_bit, "z");

        TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    #endregion
    #region Logical
    public async Task AND()
    {
        AR(0);
        PC(0);
        DR(0);
        TR(0);
        IR(0);
        R(1);
        AC(1);
        Z(1);
        Read(0);
        Write(0);

        ARLineColor(0);
        PCLineColor(0);
        DRLineColor(0);
        TRLineColor(0);
        IRLineColor(0);
        RLineColor(1);
        ACLineColor(1);
        ZLineColor(1);

        // Perform bitwise AND on ac and r
        ac_bit = ac_bit & r_bit;

        // Set z based on the result
        z_bit = (ac_bit == 0) ? 1 : 0;

        rtlStatement.Text = "AND";
        dataMovement.Text = "AC <- AC & R";

        if (sysmenu.animationOn)
        {
            await Task.Delay(1000);
        }

        AC_txt.Text = SpaceInserter(ac_bit, "ac");
        Z_txt.Text = SpaceInserter(z_bit, "z");

        TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public async Task OR()
    {
        AR(0);
        PC(0);
        DR(0);
        TR(0);
        IR(0);
        R(0);
        AC(1);
        Z(1);
        Read(0);
        Write(0);

        ARLineColor(0);
        PCLineColor(0);
        DRLineColor(0);
        TRLineColor(0);
        IRLineColor(0);
        RLineColor(0);
        ACLineColor(1);
        ZLineColor(1);

        ac_bit = ac_bit | 1;
        z_bit = (ac_bit == 0) ? 1 : 0;

        rtlStatement.Text = "OR";
        dataMovement.Text = "AC <- AC | R";

        if (sysmenu.animationOn)
        {
            await Task.Delay(1000);
        }

        AC_txt.Text = SpaceInserter(ac_bit, "ac");
        Z_txt.Text = SpaceInserter(z_bit, "z");

        TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public async Task XOR()
    {
        AR(0);
        PC(0);
        DR(0);
        TR(0);
        IR(0);
        R(0);
        AC(1);
        Z(1);
        Read(0);
        Write(0);

        ARLineColor(0);
        PCLineColor(0);
        DRLineColor(0);
        TRLineColor(0);
        IRLineColor(0);
        RLineColor(0);
        ACLineColor(1);
        ZLineColor(1);

        // Perform bitwise XOR on ac and 1
        ac_bit = ac_bit ^ 1;

        // Set z based on the result
        z_bit = (ac_bit == 0) ? 1 : 0;

        rtlStatement.Text = "XOR";
        dataMovement.Text = "AC <- AC ^ R";

        if (sysmenu.animationOn)
        {
            await Task.Delay(1000);
        }

        AC_txt.Text = SpaceInserter(ac_bit, "ac");
        Z_txt.Text = SpaceInserter(z_bit, "z");

        TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public async Task NOT()
    {
        AR(0);
        PC(0);
        DR(0);
        TR(0);
        IR(0);
        R(0);
        AC(1);
        Z(1);
        Read(0);
        Write(0);

        ARLineColor(0);
        PCLineColor(0);
        DRLineColor(0);
        TRLineColor(0);
        IRLineColor(0);
        RLineColor(0);
        ACLineColor(1);
        ZLineColor(1);

        // Perform bitwise NOT on ac
        ac_bit = ~ac_bit;

        // Set z based on the result
        z_bit = (ac_bit == 0) ? 1 : 0;

        rtlStatement.Text = "NOT";
        dataMovement.Text = "AC <- ~AC";

        if (sysmenu.animationOn)
        {
            await Task.Delay(1000);
        }

        AC_txt.Text = SpaceInserter(ac_bit, "ac");
        Z_txt.Text = SpaceInserter(z_bit, "z");

        TraceResults.AddResult(rtlStatement.Text, dataMovement.Text, ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    #endregion
    #endregion

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
        GetParent().FindChild("Databalls").AddChild(redBall);
        redBall.Position = cpuOut.Position;
        redBall.Name = "RedBall";

        destination = aIn.Position;

        await MoveToPosition(redBall, destination);
    }
    public async Task AToVDc()
    {
        CharacterBody2D redBall = (CharacterBody2D)ResourceLoader.Load<PackedScene>("res://Scenes/data_ball.tscn").Instantiate();
        GetParent().FindChild("Databalls").AddChild(redBall);
        redBall.Position = aOut.Position;
        redBall.Name = "RedBall";

        destination = vDc.Position;

        await MoveToPosition(redBall, destination);
    }
    public async Task VDcToM()
    {
        CharacterBody2D redBall = (CharacterBody2D)ResourceLoader.Load<PackedScene>("res://Scenes/data_ball.tscn").Instantiate();
        GetParent().FindChild("Databalls").AddChild(redBall);
        redBall.Position = vDc.Position;
        redBall.Name = "RedBall";

        destination = mIn.Position;

        await MoveToPosition(redBall, destination);
    }
    public async Task DToCPU()
    {
        CharacterBody2D redBall = (CharacterBody2D)ResourceLoader.Load<PackedScene>("res://Scenes/data_ball.tscn").Instantiate();
        GetParent().FindChild("Databalls").AddChild(redBall);
        redBall.Position = dOut.Position;
        redBall.Name = "RedBall";

        destination = cpuIn.Position;

        await MoveToPosition(redBall, destination);
    }
    public async Task CPUToD()
    {
        CharacterBody2D redBall = (CharacterBody2D)ResourceLoader.Load<PackedScene>("res://Scenes/data_ball.tscn").Instantiate();
        GetParent().FindChild("Databalls").AddChild(redBall);
        redBall.Position = cpuIn.Position;
        redBall.Name = "RedBall";

        destination = dOut.Position;

        await MoveToPosition(redBall, destination);
    }
    public async Task ioInVToD()
    {
        CharacterBody2D redBall = (CharacterBody2D)ResourceLoader.Load<PackedScene>("res://Scenes/data_ball.tscn").Instantiate();
        GetParent().FindChild("Databalls").AddChild(redBall);
        redBall.Position = ioInV.Position;
        redBall.Name = "RedBall";

        destination = dIn.Position;

        await MoveToPosition(redBall, destination);
    }
    public async Task DToioInV()
    {
        CharacterBody2D redBall = (CharacterBody2D)ResourceLoader.Load<PackedScene>("res://Scenes/data_ball.tscn").Instantiate();
        GetParent().FindChild("Databalls").AddChild(redBall);
        redBall.Position = dIn.Position;
        redBall.Name = "RedBall";

        destination = ioInV.Position;

        await MoveToPosition(redBall, destination);
    }
    public async Task MToioInV()
    {
        CharacterBody2D redBall = (CharacterBody2D)ResourceLoader.Load<PackedScene>("res://Scenes/data_ball.tscn").Instantiate();
        GetParent().FindChild("Databalls").AddChild(redBall);
        redBall.Position = mOut1.Position;
        redBall.Name = "RedBall";

        destination = ioInV.Position;

        await MoveToPosition(redBall, destination);
    }
    public async Task ioInVToM()
    {
        CharacterBody2D redBall = (CharacterBody2D)ResourceLoader.Load<PackedScene>("res://Scenes/data_ball.tscn").Instantiate();
        GetParent().FindChild("Databalls").AddChild(redBall);
        redBall.Position = ioInV.Position;
        redBall.Name = "RedBall";

        destination = mOut1.Position;

        await MoveToPosition(redBall, destination);
    }
    public async Task vDcTohDc()
    {
        CharacterBody2D redBall = (CharacterBody2D)ResourceLoader.Load<PackedScene>("res://Scenes/data_ball.tscn").Instantiate();
        GetParent().FindChild("Databalls").AddChild(redBall);
        redBall.Position = vDc.Position;
        redBall.Name = "RedBall";

        destination = hDc.Position;

        await MoveToPosition(redBall, destination);
    }
    public async Task hDcToDc()
    {
        CharacterBody2D redBall = (CharacterBody2D)ResourceLoader.Load<PackedScene>("res://Scenes/data_ball.tscn").Instantiate();
        GetParent().FindChild("Databalls").AddChild(redBall);
        redBall.Position = hDc.Position;
        redBall.Name = "RedBall";

        destination = dcIn.Position;

        await MoveToPosition(redBall, destination);
    }

    private async Task MoveToPosition(CharacterBody2D character, Vector2 destination)
    {
        while (character.Position != destination)
        {   
            if(destination == dIn.Position)
            {
                character.Position = character.Position.MoveToward(destination, (float)((moveSpeed * 5) * GetProcessDeltaTime()));
                await ToSignal(GetTree().CreateTimer(0.01f), "timeout");
            }
            else if(destination == mIn.Position )
            {
                character.Position = character.Position.MoveToward(destination, (float)((moveSpeed*2.7) * GetProcessDeltaTime()));
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
    public void MemoryColor(int num)
    {
        if(num == 0)
        {
            line4.DefaultColor = Colors.White;
        }
        else
        {
            line4.DefaultColor = Colors.Red;
        }
    }
    public void ReadColor(int num)
    {

        if (num == 0)
        {
            line6.DefaultColor = Colors.White;
        }
        else
        {
            line6.DefaultColor = Colors.Red;
        }
    }
    public void WriteColor(int num)
    {
        if (num == 0)
        {
            line7.DefaultColor = Colors.White;
        }
        else
        {
            line7.DefaultColor = Colors.Red;
        }
    }
    public void ClkColor(int num)
    {
        if (num == 0)
        {
            line8.DefaultColor = Colors.White;
        }
        else
        {
            line8.DefaultColor = Colors.Red;
        }
    }
    #endregion

    #region Color Me Registers
    // 0 for white 1 for red
    Color red = Colors.Red;
    Color white = Colors.White;
    public void AR(int num)
    {
        if(num == 0)
        {
            ar.AddThemeColorOverride("font_color", white);
        }
        else
        {
            ar.AddThemeColorOverride("font_color", red);
        }
    }
    public void PC(int num)
    {
        if (num == 0)
        {
            pc.AddThemeColorOverride("font_color", white);
        }
        else
        {
            pc.AddThemeColorOverride("font_color", red);
        }
    }
    public void DR(int num)
    {
        if (num == 0)
        {
            dr.AddThemeColorOverride("font_color", white);
        }
        else
        {
            dr.AddThemeColorOverride("font_color", red);
        }
    }
    public void TR(int num)
    {
        if (num == 0)
        {
            tr.AddThemeColorOverride("font_color", white);
        }
        else
        {
            tr.AddThemeColorOverride("font_color", red);
        }
    }
    public void IR(int num)
    {
        if (num == 0)
        {
            ir.AddThemeColorOverride("font_color", white);
        }
        else
        {
            ir.AddThemeColorOverride("font_color", red);
        }
    }
    public void R(int num)
    {
        if (num == 0)
        {
            r.AddThemeColorOverride("font_color", white);
        }
        else
        {
            r.AddThemeColorOverride("font_color", red);
        }
    }
    public void AC(int num)
    {
        if (num == 0)
        {
            ac.AddThemeColorOverride("font_color", white);
        }
        else
        {
            ac.AddThemeColorOverride("font_color", red);
        }
    }
    public void Z(int num)
    {
        if (num == 0)
        {
            z.AddThemeColorOverride("font_color", white);
        }
        else
        {
            z.AddThemeColorOverride("font_color", red);
        }
    }
    public void CU(int num)
    {
        if (num == 0)
        {
            cu.AddThemeColorOverride("font_color", white);
        }
        else
        {
            cu.AddThemeColorOverride("font_color", red);
        }
    }
    public void ALU(int num)
    {
        if (num == 0)
        {
            alu.AddThemeColorOverride("font_color", white);
        }
        else
        {
            alu.AddThemeColorOverride("font_color", red);
        }
    }
    public void Clock(int num)
    {
        if (num == 0)
        {
            clock.AddThemeColorOverride("font_color", white);
        }
        else
        {
            clock.AddThemeColorOverride("font_color", red);
        }
    }
    public void Read(int num)
    {
        if (num == 0)
        {
            readL.AddThemeColorOverride("font_color", white);
        }
        else
        {
            readL.AddThemeColorOverride("font_color", red);
        }
    }
    public void Write(int num)
    {
        if (num == 0)
        {
            writeL.AddThemeColorOverride("font_color", white);
        }
        else
        {
            writeL.AddThemeColorOverride("font_color", red);
        }
    }
    public void CLK(int num)
    {
        if (num == 0)
        {
            clk.AddThemeColorOverride("font_color", white);
        }
        else
        {
            clk.AddThemeColorOverride("font_color", red);
        }
    }
    public void EN1(int num)
    {
        if (num == 0)
        {
            en1.AddThemeColorOverride("font_color", white);
        }
        else
        {
            en1.AddThemeColorOverride("font_color", red);
        }
    }
    public void EN2(int num)
    {
        if (num == 0)
        {
            en2.AddThemeColorOverride("font_color", white);
        }
        else
        {
            en2.AddThemeColorOverride("font_color", red);
        }
    }
    #endregion

    #region Color Me LineEdits
    private void ARLineColor(int num)
    {
        if (num == 0)
        {
            AR_txt.AddThemeColorOverride("font_color", white);
        }
        else
        {
            AR_txt.AddThemeColorOverride("font_color", red);
        }
    }
    private void PCLineColor(int num)
    {
        if (num == 0)
        {
            PC_txt.AddThemeColorOverride("font_color", white);
        }
        else
        {
            PC_txt.AddThemeColorOverride("font_color", red);
        }
    }
    private void DRLineColor(int num)
    {
        if (num == 0)
        {
            DR_txt.AddThemeColorOverride("font_color", white);
        }
        else
        {
            DR_txt.AddThemeColorOverride("font_color", red);
        }
    }
    private void TRLineColor(int num)
    {
        if (num == 0)
        {
            TR_txt.AddThemeColorOverride("font_color", white);
        }
        else
        {
            TR_txt.AddThemeColorOverride("font_color", red);
        }
    }
    private void IRLineColor(int num)
    {
        if (num == 0)
        {
            IR_txt.AddThemeColorOverride("font_color", white);
        }
        else
        {
            R_txt.AddThemeColorOverride("font_color", red);
        }
    }
    private void RLineColor(int num)
    {
        if (num == 0)
        {
            R_txt.AddThemeColorOverride("font_color", white);
        }
        else
        {
            R_txt.AddThemeColorOverride("font_color", red);
        }
    }
    private void ACLineColor(int num)
    {
        if (num == 0)
        {
            AC_txt.AddThemeColorOverride("font_color", white);
        }
        else
        {
            AC_txt.AddThemeColorOverride("font_color", red);
        }
    }
    private void ZLineColor(int num)
    {
        if (num == 0)
        {
            Z_txt.AddThemeColorOverride("font_color", white);
        }
        else
        {
            Z_txt.AddThemeColorOverride("font_color", red);
        }
    }
    #endregion

    #region Miscellaneous
    public string SpaceInserter(int reg, string regname)
    {
        string binaryString;
        if (regname == "ar" || regname == "pc")
        {
            // Convert the integer to a binary string with spaces every 4 digits
            binaryString = Convert.ToString(reg, 2).PadLeft(16, '0');
        }
        else if (regname == "z")
        {
            // Convert the integer to a binary string with spaces every 4 digits
            binaryString = Convert.ToString(reg);
        }
        else
        {
            // Convert the integer to a binary string with spaces every 4 digits
            binaryString = Convert.ToString(reg, 2).PadLeft(8, '0');
        }

        // Insert spaces after every 4 digits
        int groupSize = 4;
        for (int i = groupSize; i < binaryString.Length; i += (groupSize + 1))
        {
            binaryString = binaryString.Insert(i, " ");
        }
        return binaryString;
    }

    public void SetRequirements(Label cpuStatus, Label rtlStatement, Label dataMovement, LineEdit currentMemoryLocation)
    {
        this.cpuStatus = cpuStatus;
        this.rtlStatement = rtlStatement;
        this.dataMovement = dataMovement;
        this.currentMemoryLocation = currentMemoryLocation;
    }

    public async Task ClockPulse()
    {
        Clock(1);
        CLK(1);
        ClkColor(1);
        await Task.Delay(500);
        Clock(0);
        CLK(0);
        ClkColor(0);
    }

    public async Task StepInstruction()
    {
        while (stepThroughInstruction)
        {
            cpuStatus.Text = "Instruction Ended";
            await Task.Delay(100);
            if (!stepThroughInstruction)
            {
                cpuStatus.Text = "Running";
                break;
            }
        }
    }
    public async Task StepCycle()
    {
        while (stepThroughCycle)
        {
            cpuStatus.Text = "Cycle Ended";
            await Task.Delay(100);
            if (!stepThroughCycle)
            {
                cpuStatus.Text = "Running";
                break;
            }
        }
    }

    static int BinaryStringToInt(string binaryString)
    {
        // Remove spaces
        string cleanedBinaryString = binaryString.Replace(" ", "");

        // Convert binary string to integer
        int result = Convert.ToInt32(cleanedBinaryString, 2);

        return result;
    }
    #endregion
}
