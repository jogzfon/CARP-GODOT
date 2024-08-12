using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

public partial class project_page : Control
{
    CheckButton hardwired;
    CheckButton microprogrammed;

    [Export] private Panel memoryPnl;
    [Export] private Panel breakPointsPnl;
    [Export] private Panel traceResultsPnl;
    [Export] private Control aiPnl;
    [Export] private SubViewportContainer viewSystem;

    TextEdit instructionCodes;
    LineEdit memoryLocation;

    private Assembler assembler = new Assembler();

    //Memory
    private bool isHex = false;
    private TextEdit memoryBox;

    //Break Points
    public List<int> breakpointList = new List<int>();
    private LineEdit addressInput;
    private VBoxContainer breakPointAddressList;

    private bool firstOpenBreakPoint = true;

    //Trace Results
    private TextEdit traceResultBox;

    //NotificationHandler
    [Export] private NotificationHandler notification;

    #region Buttons
    [Export] private Button toSystem;
    [Export] private Button assemble;
    [Export] private Button toAI;

    [Export] private TextureButton back;
    #endregion
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        #region Buttons
        //Side buttons
        toSystem.Connect("pressed", new Callable(this, nameof(GoToSystem)));

        if (AccountManager.GetUser() != null)
        {
            if (AccountManager.GetRole().Equals("Teacher"))
            {
                toAI.Visible = true;
                toAI.Disabled = false;
                toAI.Connect("pressed", new Callable(this, nameof(GoToAI)));
            }
            else
            {
                toAI.Visible = false;
                toAI.Disabled = true;
            }
            
            aiPnl.Visible = false;
            Memory.contents = DataToSave.memoryContents;
            breakpointList = DataToSave.breakpointList;
        }
        else
        {
            toAI.Visible = false;
            toAI.Disabled = true;
        }

        assemble.Connect("pressed", new Callable(this, nameof(Assemble)));

        back.Connect("pressed", new Callable(this, nameof(BackToProject)));

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
        memoryPnl.Show();
        breakPointsPnl.Hide();
        traceResultsPnl.Hide();
        viewSystem.Hide();
        #endregion

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
        if (AccountManager.GetUser() != null)
        {
            DataToSave.memoryContents = Memory.contents;
            DataToSave.breakpointList = breakpointList;
            DataToSave.traceText = traceResultBox.Text;

            if (Input.IsActionPressed("Ctrl") && Input.IsActionPressed("S"))
            {
                if (AccountManager.GetUser() != null && (AccountManager.GetUser().Subscription == "Student" || AccountManager.GetUser().Subscription == "Teacher"))
                {
                    GD.Print("Printed");
                    DataToSave.SaveFile();
                }
            }
        }
	}
 
    public void BackToProject()
    {
        if (AccountManager.GetUser() != null)
        {
            if (AccountManager.GetUser().Subscription == "Student" || AccountManager.GetUser().Subscription == "Teacher")
            {
                DataToSave.SaveFile();
            }
            DataToSave.ResetDatas();
        }
        /*if(mainPage != null)
        {
            GetTree().ChangeSceneToPacked(mainPage);
        }*/
        this.QueueFree();
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

                notification.MessageBox(errors[i].GetString(), 1);

                //MessageBox.Show(errors[i].GetString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                i++;
            }
            if (errors.Length <= 0)
            {
                Debug.Print("Assembly Successful.");

                notification.MessageBox("Assembly Successful.", 0);
                //MessageBox.Show("Assembly Successful.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        else
        {
            Debug.Print(memoryLocation.Text + " is not a valid memory location.");
            notification.MessageBox(memoryLocation.Text + " is not a valid memory location.", 1);
            //MessageBox.Show(memLocation.Text + " is not a valid memory location.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void GoToAI()
    {
        aiPnl.Visible = true;
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

        if (firstOpenBreakPoint && AccountManager.GetUser()!=null)
        {
            firstOpenBreakPoint = false;
            foreach (int breakpoint in breakpointList)
            {
                Label breakPoint = new Label();
                breakPoint.Text = " Address: " + breakpoint.ToString();
                breakPoint.Name = breakpoint.ToString();
                breakPoint.Set("theme_override_fonts/font", "res://Fonts/Inter-Regular.ttf");

                breakPointAddressList.AddChild(breakPoint);
            }
        }
    }

    private void AddBreakPoint()
    {
        int result = int.Parse(addressInput.Text);
        if (!string.IsNullOrEmpty(addressInput.Text) && IsDigitsRegex(addressInput.Text) && result < 65536 && result >= 0 && !breakpointList.Contains(result))
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
            notification.MessageBox("Please enter a valid breakpoint.", 1);
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
            notification.MessageBox("Please enter a valid breakpoint.", 1);
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

        if(AccountManager.GetUser() != null)
        {
            traceResultBox.Text = DataToSave.traceText;
        }
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
