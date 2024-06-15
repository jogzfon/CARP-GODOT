using Godot;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System;

public partial class system_menu : Control
{
	[Export] LineEdit AR;
	[Export] LineEdit PC;
	[Export] LineEdit DR;
	[Export] LineEdit TR;
	[Export] LineEdit IR;
	[Export] LineEdit R;
	[Export] LineEdit AC;
	[Export] LineEdit Z;

    public bool isHex = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        AR.Connect("text_submitted", new Callable(this, nameof(ConvertAR)));
        PC.Connect("text_submitted", new Callable(this, nameof(ConvertPC)));
        DR.Connect("text_submitted", new Callable(this, nameof(ConvertDR)));
        TR.Connect("text_submitted", new Callable(this, nameof(ConvertTR)));
        IR.Connect("text_submitted", new Callable(this, nameof(ConvertIR)));
        R.Connect("text_submitted", new Callable(this, nameof(ConvertR)));
        AC.Connect("text_submitted", new Callable(this, nameof(ConvertAC)));
        Z.Connect("text_submitted", new Callable(this, nameof(ConvertZ)));
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void ConvertAR(String input)
	{
        UpdateRegisters(AR, input, "ar");
    }
    private void ConvertPC(String input)
    {
        UpdateRegisters(PC, input, "pc");
    }
    private void ConvertDR(String input)
    {
        UpdateRegisters(DR, input, "dr");
    }
    private void ConvertTR(String input)
    {
        UpdateRegisters(TR, input, "tr");
    }
    private void ConvertIR(String input)
    {
        UpdateRegisters(IR, input, "ir");
    }
    private void ConvertR(String input)
    {
        UpdateRegisters(R, input, "r");
    }
    private void ConvertAC(String input)
    {
        UpdateRegisters(AC, input, "ac");
    }
    private void ConvertZ(String input)
    {
        UpdateRegisters(Z, input,"z");
    }
    public void UpdateRegisters(LineEdit textBox, string value, string regname)
    {
        try
        {
            int num;

            if (IsHexadecimal(value))
            {
                // Convert the hex string to an integer
                num = Convert.ToInt32(value, 16);
            }
            else
            {
                // Convert the decimal string to an integer
                num = Convert.ToInt32(value);
            }

            // Determine the binary representation with padding
            string binaryString;
            if (regname == "ar" || regname == "pc")
            {
                binaryString = Convert.ToString(num & 0xFFFF, 2).PadLeft(16, '0');
            }
            else
            {
                binaryString = Convert.ToString(num & 0xFF, 2).PadLeft(8, '0');
            }

            // Insert spaces every 4 digits in the binary string
            binaryString = InsertSpaces(binaryString, 4);

            // Set the textBox text
            textBox.Text = binaryString;
        }
        catch (FormatException)
        {
            // Handle format exception if the input string cannot be converted to an integer
            textBox.Text = "Invalid Input";
        }
        catch (OverflowException)
        {
            // Handle overflow exception if the input string represents a number out of the integer range
            textBox.Text = "Overflow";
        }
    }

    private string InsertSpaces(string binaryString, int groupSize)
    {
        for (int i = groupSize; i < binaryString.Length; i += (groupSize + 1))
        {
            binaryString = binaryString.Insert(i, " ");
        }
        return binaryString;
    }
    private bool IsHexadecimal(string value)
    {
        // A simple regex to check if a string is a valid hexadecimal number
        return Regex.IsMatch(value, @"\A\b[0-9a-fA-F]+\b\Z");
    }

}
