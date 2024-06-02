using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

[GlobalClass, Icon("res://icon.svg")]
public partial class project_page : Control
{
    CheckButton hardwired;
    CheckButton microprogrammed;

    Panel memoryPnl;
    Panel breakPointsPnl;
    Panel traceResultsPnl;
    SubViewportContainer viewSystem;

    TextEdit instructionCodes;
    LineEdit memoryLocation;

    private Assembler assembler = new Assembler();
    public List<int> breakpoints = new List<int>();

    //Memory
    private bool isHex = false;
    private TextEdit memoryBox;

    //Break Points
    private List<int> breakpointList = new List<int>();
    private LineEdit addressInput;
    private VBoxContainer breakPointAddressList;

    //Trace Results
    private TextEdit traceResultBox;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        #region Buttons
        //Side buttons
        var toSystem = GetNode<Button>("VBoxContainer/HBoxContainer/ViewSystem");
        toSystem.Connect("pressed", new Callable(this, nameof(GoToSystem)));

        var assemble = GetNode<Button>("LeftContainer/Assemble");
        assemble.Connect("pressed", new Callable(this, nameof(Assemble)));

        var toAI = GetNode<Button>("VBoxContainer/HBoxContainer/AIBtn");
        toAI.Connect("pressed", new Callable(this, nameof(GoToAI)));

        //Main Buttons
        #region Memory
        var memoryAndIO = GetNode<Button>("VBoxContainer/HBoxContainer/MemoryAndIO");
        memoryAndIO.Connect("pressed", new Callable(this, nameof(OpenMemory)));
        var view = GetNode<Button>("VBoxContainer/PanelContainer/MemoryAndIO/VBoxContainer/HBoxContainer/View");
        view.Connect("pressed", new Callable(this, nameof(ViewMemory)));
        var clear = GetNode<Button>("VBoxContainer/PanelContainer/MemoryAndIO/VBoxContainer/HBoxContainer/Clear");
        clear.Connect("pressed", new Callable(this, nameof(ClearMemory)));
        var hex = GetNode<Button>("VBoxContainer/PanelContainer/MemoryAndIO/VBoxContainer/HBoxContainer/VBoxContainer/Hex");
        hex.Connect("pressed", new Callable(this, nameof(ConvertToHex)));
        var binary = GetNode<Button>("VBoxContainer/PanelContainer/MemoryAndIO/VBoxContainer/HBoxContainer/VBoxContainer/Binary");
        binary.Connect("pressed", new Callable(this, nameof(ConvertToBinary)));
        #endregion

        #region BreakPoints
        var breakpoints = GetNode<Button>("VBoxContainer/HBoxContainer/BreakPoints");
        breakpoints.Connect("pressed", new Callable(this, nameof(OpenBreakPoints)));

        var addBreakpoint = GetNode<Button>("VBoxContainer/PanelContainer/Breakpoints/VBoxContainer/HBoxContainer/Add");
        addBreakpoint.Connect("pressed", new Callable(this, nameof(AddBreakPoint)));
        var deleteBreakpoint = GetNode<Button>("VBoxContainer/PanelContainer/Breakpoints/VBoxContainer/HBoxContainer/Delete");
        deleteBreakpoint.Connect("pressed", new Callable(this, nameof(DeleteBreakPoint)));
        #endregion

        #region TraceResults
        var traceResult = GetNode<Button>("VBoxContainer/HBoxContainer/TraceResults");
        traceResult.Connect("pressed", new Callable(this, nameof(OpenTraceResults)));

        var viewResult = GetNode<Button>("VBoxContainer/PanelContainer/TraceResult/VBoxContainer/HBoxContainer/ViewResults");
        viewResult.Connect("pressed", new Callable(this, nameof(ViewResults)));

        var clearResult = GetNode<Button>("VBoxContainer/PanelContainer/TraceResult/VBoxContainer/HBoxContainer/ClearResults");
        clearResult.Connect("pressed", new Callable(this, nameof(ClearResults)));
        #endregion

        #endregion

        #region Others
        //Memory
        memoryBox = GetNode<TextEdit>("VBoxContainer/PanelContainer/MemoryAndIO/VBoxContainer/ScrollContainer/MemoryText");

        //BreakPoints
        addressInput = GetNode<LineEdit>("VBoxContainer/PanelContainer/Breakpoints/VBoxContainer/HBoxContainer/AddressInput");
        breakPointAddressList = GetNode<VBoxContainer>("VBoxContainer/PanelContainer/Breakpoints/VBoxContainer/AddressList");

        //Trace Results
        traceResultBox = GetNode<TextEdit>("VBoxContainer/PanelContainer/TraceResult/VBoxContainer/ScrollContainer/TraceText");

        //Inputs
        instructionCodes = GetNode<TextEdit>("LeftContainer/ScrollContainer/InstructionCode");
        memoryLocation = GetNode<LineEdit>("LeftContainer/MemoryLocation");

        //Optional
        hardwired = GetNode<CheckButton>("LeftContainer/HBoxContainer/Hardwired");
        hardwired.Connect("toggled", new Callable(this, nameof(HardWired)));
        microprogrammed = GetNode<CheckButton>("LeftContainer/HBoxContainer/MicroProgrammed");
        microprogrammed.Connect("toggled", new Callable(this, nameof(MicroProgrammed)));

        microprogrammed.ButtonPressed = true;
        hardwired.ButtonPressed = false;
        #endregion
        #region Panels
        memoryPnl = GetNode<Panel>("VBoxContainer/PanelContainer/MemoryAndIO");
        breakPointsPnl = GetNode<Panel>("VBoxContainer/PanelContainer/Breakpoints");
        traceResultsPnl = GetNode<Panel>("VBoxContainer/PanelContainer/TraceResult");
        viewSystem = GetNode<SubViewportContainer>("VBoxContainer/PanelContainer/SystemViewContainer"); 
        
        memoryPnl.Show();
        breakPointsPnl.Hide();
        traceResultsPnl.Hide();
        viewSystem.Hide();
        #endregion

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
        
	}

    #region LeftButtons
    private void Assemble()
    {
        string text = instructionCodes.Text;

        if (int.TryParse(memoryLocation.Text, out int location))
        {
            AssemblyResults results = assembler.Assemble(text, location);
            AssemblyError[] errors = results.GetErrors();
            int i = 0;
            while (i < errors.Length)
            {
                Debug.Print(errors[i].GetString());
                //MessageBox.Show(errors[i].GetString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                i++;
            }
            if (errors.Length <= 0)
            {
                Debug.Print("Assembly Successful.");
                //MessageBox.Show("Assembly Successful.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        else
        {
            Debug.Print(memoryLocation.Text + " is not a valid memory location.");
            //MessageBox.Show(memLocation.Text + " is not a valid memory location.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void GoToAI()
    {
        Node simultaneousScene = ResourceLoader.Load<PackedScene>("res://Scenes/ai_system.tscn").Instantiate();
        GetTree().Root.AddChild(simultaneousScene);
        Hide();
    }
    private void HardWired()
    {
        microprogrammed.ButtonPressed = false;
    }
    private void MicroProgrammed()
    {
        hardwired.ButtonPressed = false;
    }
    #endregion

    #region Memory
    private void OpenMemory()
    {
        memoryPnl.Show();
        breakPointsPnl.Hide();
        traceResultsPnl.Hide();
        viewSystem.Hide();
    }
    private void ViewMemory()
    {
        memoryBox.Text = "";
        Memory.UpdateMemoryTextBox(memoryBox, isHex);
    }
    private void ClearMemory()
    {
        Memory.Clear();
        memoryBox.Text = "";
        Memory.UpdateMemoryTextBox(memoryBox, isHex);
    }
    private void ConvertToHex()
    {
        isHex = true;
        memoryBox.Text = "";
        Memory.UpdateMemoryTextBox(memoryBox, isHex);
    }
    private void ConvertToBinary()
    {
        isHex = false;
        memoryBox.Text = "";
        Memory.UpdateMemoryTextBox(memoryBox, isHex);
    }
    #endregion Memory

    #region BreakPoints
    private void OpenBreakPoints()
    {
        memoryPnl.Hide();
        breakPointsPnl.Show();
        traceResultsPnl.Hide();
        viewSystem.Hide();
    }

    private void AddBreakPoint()
    {
        int result = int.Parse(addressInput.Text);
        if (!string.IsNullOrEmpty(addressInput.Text) && IsDigitsRegex(addressInput.Text) && result < 65536 && result > 0 && !breakpointList.Contains(result))
        {
            breakpointList.Add(result);

            Label breakPoint = new Label();
            breakPoint.Text = " Address: " + result.ToString();
            breakPoint.Name = result.ToString();
            breakPoint.Set("theme_override_fonts/font", "res://Fonts/Inter-Regular.ttf");

            breakPointAddressList.AddChild(breakPoint);
            addressInput.Clear();
        }
        else
        {
            addressInput.Clear();
            Debug.Print("Please enter a valid breakpoint.");
            //MessageBox.Show("Please enter a valid breakpoint.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void DeleteBreakPoint()
    {
        int result = int.Parse(addressInput.Text);
        if (!string.IsNullOrEmpty(addressInput.Text) && IsDigitsRegex(addressInput.Text) && result < 65536 && result > 0)
        {
            for(int i = 0; i < breakpointList.Count; i++)
            {
                if (breakpointList[i] == result)
                {
                   Node breakLabel = breakPointAddressList.GetChild(i);
                   breakPointAddressList.RemoveChild(breakLabel);
                }
            }
            breakpointList.Remove(result);
            addressInput.Clear();
        }
        else
        {
            addressInput.Clear();
            Debug.Print("Please enter a valid breakpoint.");
            //MessageBox.Show("Please enter a valid breakpoint.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    public List<int> GetBreakPoints()
    {
        return breakpointList;
    }
    public bool IsDigitsRegex(string input)
    {
        Regex regex = new Regex(@"^\d+$");
        return regex.IsMatch(input);
    }
    #endregion

    #region TraceResults
    private void OpenTraceResults()
    {
        memoryPnl.Hide();
        breakPointsPnl.Hide();
        traceResultsPnl.Show();
        viewSystem.Hide();
    }
    private void ViewResults()
    {
        traceResultBox.Clear();
        TraceResults.UpdateTraceResults(traceResultBox);
        traceResultBox.Text += "\n";
    }
    private void ClearResults()
    {
        traceResultBox.Clear();
        TraceResults.RemoveAllStatements();
    }
    #endregion

    #region View System
    private void GoToSystem()
    {
        memoryPnl.Hide();
        breakPointsPnl.Hide();
        traceResultsPnl.Hide();
        viewSystem.Show();
    }
    #endregion
}
