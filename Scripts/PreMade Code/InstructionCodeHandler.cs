using Godot;
using System;

public partial class InstructionCodeHandler : TextEdit
{
    [Export] private TextEdit instructions;

    private PremadeCodeList precodes = new PremadeCodeList();

    public override void _Ready()
    {
        precodes.InitializePremadeCodes();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        foreach (PremadeCode code in precodes.premadeCodes)
        {
            // Check if the Enter key is pressed and if the KeyWord exists in the instructions
            if (Input.IsActionJustPressed("ui_accept") && instructions.Text.Contains(code.KeyWord))
            {
                // Replace the KeyWord in instructions with the corresponding PreCodes
                instructions.Text = instructions.Text.Replace(code.KeyWord, code.PreCodes);
            }
        }
    }

    public PremadeCodeList GetPrecodes()
    {
        return precodes;
    }
}
