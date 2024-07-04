using Godot;
using System.Text.RegularExpressions;
using System.Text;
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

    [Export] Button hexBtn;
    [Export] Button binaryBtn;
    [Export] CheckButton AnimationOn;

    public bool isHex = false;
    public bool animationOn = true;

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

        hexBtn.Connect("pressed", new Callable(this, nameof(IsHex)));
        binaryBtn.Connect("pressed", new Callable(this, nameof(IsBinary)));
    }
    public override void _Process(double delta)
    {
        animationOn = AnimationOn.ButtonPressed;
    }
    private void ConvertAR(string input) => UpdateRegisters(AR, input, 16);
    private void ConvertPC(string input) => UpdateRegisters(PC, input, 16);
    private void ConvertDR(string input) => UpdateRegisters(DR, input, 8);
    private void ConvertTR(string input) => UpdateRegisters(TR, input, 8);
    private void ConvertIR(string input) => UpdateRegisters(IR, input, 8);
    private void ConvertR(string input) => UpdateRegisters(R, input, 8);
    private void ConvertAC(string input) => UpdateRegisters(AC, input, 8);
    private void ConvertZ(string input) => UpdateRegisters(Z, input, 1);

    public void UpdateRegisters(LineEdit textBox, string value, int bits)
    {
        try
        {
            string result = isHex ? ToHex(value) : ToBinary(value, bits) ;
            textBox.Text = result;
        }
        catch
        {
            textBox.Text = isHex ? new string('0', bits) : "0";
        }
    }

    private string ToBinary(string hex, int bits)
    {
        hex = hex.Replace(" ", "");
        StringBuilder binary = new StringBuilder();
        foreach (char c in hex)
        {
            int value = Convert.ToInt32(c.ToString(), 16);
            binary.Append(Convert.ToString(value, 2).PadLeft(4, '0'));
        }
        string result = binary.ToString().PadLeft(bits, '0');
        return bits == 1 ? result : InsertSpaces(result, 4);
    }

    private string ToHex(string binary)
    {
        binary = binary.Replace(" ", "");
        StringBuilder hex = new StringBuilder();
        for (int i = 0; i < binary.Length; i += 4)
        {
            string chunk = binary.Substring(i, Math.Min(4, binary.Length - i)).PadLeft(4, '0');
            int value = Convert.ToInt32(chunk, 2);
            hex.Append(value.ToString("X"));
        }
        return hex.ToString();
    }

    private string InsertSpaces(string input, int groupSize)
    {
        StringBuilder result = new StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            if (i > 0 && i % groupSize == 0)
                result.Append(' ');
            result.Append(input[i]);
        }
        return result.ToString();
    }

    private void IsHex()
    {
        if (!isHex)
        {
            isHex = true;
            Conversion();
        }
    }

    private void IsBinary()
    {
        if (isHex)
        {
            isHex = false;
            Conversion();
        }
    }

    public void Conversion()
    {
        ConvertAR(AR.Text);
        ConvertPC(PC.Text);
        ConvertDR(DR.Text);
        ConvertTR(TR.Text);
        ConvertIR(IR.Text);
        ConvertR(R.Text);
        ConvertAC(AC.Text);
        ConvertZ(Z.Text);
    }
}