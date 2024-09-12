using Godot;
using System;
using System.Collections.Generic;

public partial class Assembler
{
    public const string NOP = "NOP";
    public const string LDAC = "LDAC";
    public const string STAC = "STAC";
    public const string MVAC = "MVAC";
    public const string MOVR = "MOVR";
    public const string JUMP = "JUMP";
    public const string JMPZ = "JMPZ";
    public const string JPNZ = "JPNZ";
    public const string ADD = "ADD";
    public const string SUB = "SUB";
    public const string INAC = "INAC";
    public const string CLAC = "CLAC";
    public const string AND = "AND";
    public const string OR = "OR";
    public const string XOR = "XOR";
    public const string NOT = "NOT";
    public const string END = "END";

    public const string ORG = "ORG";
    public const string DB = "DB";
    public const string DW = "DW";

    public AssemblyResults Assemble(string source, int location)
    {
        AssemblySourceProgram code = new AssemblySourceProgram(source);
        int memoryCounter = location;
        List<AssemblyError> assemblyErrorList = new List<AssemblyError>();
        int errorCount = 0;

        /*// Pre assembly in order to get the jumps to work correctly
        for (int lineIndex = 1; lineIndex <= code.SourceLineLength(); lineIndex++)
        {
            AssemblySourceLine line = code.GetSourceLineByLineNumber(lineIndex);
        }*/
        for (int lineIndex = 1; lineIndex <= code.SourceLineLength(); lineIndex++)
        {
            //Console.WriteLine("Assembling . . . Checking line " + lineIndex);
            //MessageBox.Show("Assembling . . . Checking line " + lineIndex, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //box.ShowStatus("Assembling . . . Checking line " + lineIndex);
            AssemblySourceLine line = code.GetSourceLineByLineNumber(lineIndex);
            Console.WriteLine(line);

            if (line != null && line.SourceTokenLength() > 0)
            {
                string[] tokens = line.GetSourceTokenStringArray();
                int offset = 0;
                if (IsLabel(tokens[0]))
                {
                    offset = 1;
                }
                if (string.Equals(tokens[0 + offset], ORG, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("ORG");
                    if (tokens.Length - offset < 2)
                    {
                        errorCount++;
                        assemblyErrorList.Add(new AssemblyError(
                            lineIndex, "LINE " + lineIndex + "::  " + tokens[0 + offset] + ": ORG expects one operand"));
                    }
                    else if (tokens.Length - offset > 2)
                    {
                        errorCount++;
                        assemblyErrorList.Add(new AssemblyError(
                            lineIndex, "LINE " + lineIndex + "::  " + tokens[0 + offset] + ": ORG does not accept more than one " +
                            "operand"));
                    }
                    else if (!AssemblyInstructions.isAddress(tokens[1 + offset]))
                    {
                        errorCount++;
                        assemblyErrorList.Add(new AssemblyError(
                            lineIndex, "LINE " + lineIndex + "::  " + tokens[1 + offset] + ": Specified address is invalid"));
                    }

                }
                else if (string.Equals(tokens[0 + offset], DB, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("DB");
                    if (tokens.Length - offset < 2)
                    {
                        errorCount++;
                        assemblyErrorList.Add(new AssemblyError(
                            lineIndex, "LINE " + lineIndex + "::  " + tokens[0 + offset] + ": DB expects one operand"));
                    }
                    else if (tokens.Length - offset > 2)
                    {
                        errorCount++;
                        assemblyErrorList.Add(new AssemblyError(
                            lineIndex, "LINE " + lineIndex + "::  " + tokens[0 + offset] + ": DB does not accept more than one " +
                            "operand"));
                    }
                    else if (AssemblyInstructions.ToByteShort(tokens[1 + offset]) ==
                        -1)
                    {
                        errorCount++;
                        assemblyErrorList.Add(new AssemblyError(
                            lineIndex, "LINE " + lineIndex + "::  " + tokens[1 + offset] + ": Specified byte constant is invalid"));
                    }

                }
                else if (string.Equals(tokens[0 + offset], DW, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("DW");
                    if (tokens.Length - offset < 2)
                    {
                        errorCount++;
                        assemblyErrorList.Add(new AssemblyError(
                            lineIndex, "LINE " + lineIndex + "::  " + tokens[0 + offset] + ": DW expects one operand"));
                    }
                    else if (tokens.Length - offset > 2)
                    {
                        errorCount++;
                        assemblyErrorList.Add(new AssemblyError(
                            lineIndex, "LINE " + lineIndex + "::  " + tokens[0 + offset] + ": DW does not accept more than one " +
                            "operand"));
                    }
                    else if (AssemblyInstructions.ToWordInteger(tokens[1 + offset])
                        == -1)
                    {
                        errorCount++;
                        assemblyErrorList.Add(new AssemblyError(
                            lineIndex, "LINE " + lineIndex + "::  " + tokens[1 + offset] + ": Specified word constant is invalid"));
                    }

                }
                else if (AssemblyInstructions.isMnemonic(tokens[0 + offset]))
                {
                    Console.WriteLine("Mnemonic");
                    if (AssemblyInstructions.ExpectsOperands(tokens[0 + offset]) == 1)
                    {

                        if (tokens.Length - offset < 2)
                        {
                            errorCount++;
                            assemblyErrorList.Add(new AssemblyError(
                                lineIndex, "LINE " + lineIndex + "::  " + tokens[0 + offset].ToUpper() + ": expects "
                                + "one operand"));
                        }
                        else if (tokens.Length - offset > 2)
                        {
                            errorCount++;
                            assemblyErrorList.Add(new AssemblyError(
                                lineIndex, "LINE " + lineIndex + "::  " + tokens[0 + offset].ToUpper() + ": does not "
                                + "accept more than one operand"));
                        }
                        else if (!AssemblyInstructions.isAddress(tokens[
                            1 + offset]) && !LabelExists(tokens[1 + offset], code))
                        {
                            errorCount++;
                            assemblyErrorList.Add(new AssemblyError(
                                lineIndex, "LINE " + lineIndex + "::  " + tokens[1 + offset].ToUpper() + ": Specified address is invalid"));
                        }

                    }
                    else
                    {

                        if (tokens.Length - offset > 1)
                        {
                            errorCount++;
                            assemblyErrorList.Add(new AssemblyError(
                                lineIndex, "LINE " + lineIndex + "::  " + tokens[0 + offset].ToUpper() + ": does not "
                                + "accept any operands"));
                        }

                    }

                }
                else
                {
                    errorCount++;
                    assemblyErrorList.Add(new AssemblyError(
                        lineIndex, "LINE " + lineIndex + "::  " + tokens[0 + offset] + ": Specified mnemonic is invalid"));
                }
            }
        }

        if (errorCount != 0)
        {
            //MessageBox.Show("Assembling . . . " + errorCount + " error(s) " + "encountered", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //box.ShowStatus("Assembling . . . " + errorCount + " error(s) " + "encountered");
        }
        else
        {

            for (int lineIndex = 1; lineIndex <= code.SourceLineLength(); lineIndex++)
            {
                //Console.WriteLine("Assembling . . . Assembling line " + lineIndex);
                //MessageBox.Show("Assembling . . . Assembling line " + lineIndex, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //box.ShowStatus("Assembling . . . Assembling line " + lineIndex);

                AssemblySourceLine line = code.GetSourceLineByLineNumber(lineIndex);

                if ((line != null) && (line.ToString() != "") && (line.SourceTokenLength() > 0))
                {
                    string[] tokens = line.GetSourceTokenStringArray();
                    int offset = 0;
                    if (IsLabel(tokens[0]))
                    {
                        offset = 1;
                    }

                    if (string.Equals(tokens[0 + offset], ORG, StringComparison.OrdinalIgnoreCase))
                    {
                        memoryCounter = AssemblyInstructions.toAddressInteger(
                            tokens[1 + offset]);
                    }
                    else if (string.Equals(tokens[0 + offset], DB, StringComparison.OrdinalIgnoreCase))
                    {
                        short byteShort = AssemblyInstructions.ToByteShort(
                            tokens[1 + offset]);

                        Memory.Write(memoryCounter, byteShort);

                        //                  String[] binaryNybble = toBinaryNybble( byteShort );
                        //                  memory[ memoryCounter ] = binaryNybble;

                        memoryCounter++;
                    }
                    else if (string.Equals(tokens[0 + offset], DW, StringComparison.OrdinalIgnoreCase))
                    {
                        short[] wordCode = AssemblyInstructions.ToWordCode(
                            AssemblyInstructions.ToWordInteger(tokens[1 + offset]));

                        Memory.Write(memoryCounter, wordCode[0]);
                        Memory.Write(memoryCounter + 1, wordCode[1]);

                        //                  String[] binaryNybble = toBinaryNybble( wordCode[ 0 ] );
                        //                  memory[ memoryCounter ] = binaryNybble;

                        //                  binaryNybble = toBinaryNybble( wordCode[ 1 ] );
                        //                  memory[ memoryCounter + 1 ] = binaryNybble;

                        memoryCounter += 2;
                    }
                    else
                    {
                        code.SetSourceLineAddressByLineNumber(memoryCounter,
                            lineIndex);
                        //                  opcode[ memoryCounter ] = tokens[ 0 ].toUpperCase();

                        //                  String[] binaryNybble = toBinaryNybble(
                        //                     AssemblyInstruction.toMnemonicCode( tokens[ 0 ] ) );

                        Memory.Write(memoryCounter, AssemblyInstructions.
                            ToMnemonicCode(tokens[0 + offset]));

                        //                  memory[ memoryCounter ] = binaryNybble;

                        if (AssemblyInstructions.ExpectsOperands(tokens[
                            0 + offset]) == 1)
                        {
                            short[] addressCode;
                            if (LabelExists(tokens[1 + offset], code))
                            {
                                addressCode = AssemblyInstructions.
                                toAddressCode(GetLabelAddress(tokens[1 + offset], code));
                            }
                            else
                            {
                                addressCode = AssemblyInstructions.
                                    toAddressCode(AssemblyInstructions.
                                    toAddressInteger(tokens[1 + offset]));
                            }

                            //                     String[] binaryNybbleAddress = toBinaryNybble(
                            //                        addressCode[ 0 ] );

                            Memory.Write(memoryCounter + 1, addressCode[0]);

                            //                     memory[ memoryCounter + 1 ] = binaryNybbleAddress;

                            //                     binaryNybbleAddress = toBinaryNybble( addressCode[
                            //                        1 ] );

                            Memory.Write(memoryCounter + 2, addressCode[1]);

                            //                     memory[ memoryCounter + 2 ] = binaryNybbleAddress;

                            memoryCounter += 2;
                        }

                        memoryCounter++;
                    }

                }
            }
        }

        //CPUBox.GetMemory().Invalidate();
        // CPUBox.GetMemory().Update();

        AssemblyError[] assemblyErrors = assemblyErrorList.ToArray();

        //MessageBox.Show("Assembly Complete", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //box.ShowStatus("Assembly Complete");
        return new AssemblyResults(assemblyErrors, code);
    }

    private static bool IsLabel(string token)
    {
        if (token.IndexOf(':') != -1 && token.IndexOf(':') == token.Length - 1 && token.Length > 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private static bool LabelExists(string label, AssemblySourceProgram code)
    {
        for (int lineIndex = 1; lineIndex <= code.SourceLineLength(); lineIndex++)
        {
            AssemblySourceLine line = code.GetSourceLineByLineNumber(lineIndex);
            if (line != null && line.GetLabel().ToLower().Equals(label.ToLower() + ":"))
            {
                return true;
            }
        }
        return false;
    }

    private static int GetLabelAddress(string label, AssemblySourceProgram code)
    {
        if (LabelExists(label, code))
        {
            int memoryCounter = 0;
            for (int lineIndex = 1; lineIndex <= code.SourceLineLength(); lineIndex++)
            {
                AssemblySourceLine line = code.GetSourceLineByLineNumber(lineIndex);
                if (line != null && line.GetLabel().ToLower().Equals(label.ToLower() + ":"))
                {
                    return memoryCounter;//line.getAddress();
                }

                if ((line != null) && (line.SourceTokenLength() > 0))
                {
                    String[] tokens = line.GetSourceTokenStringArray();
                    int offset = 0;
                    if (IsLabel(tokens[0]))
                    {
                        offset = 1;
                    }

                    if (string.Equals(tokens[0 + offset], ORG, StringComparison.OrdinalIgnoreCase))
                    {
                        memoryCounter = AssemblyInstructions.toAddressInteger(
                            tokens[1 + offset]);
                    }
                    else if (string.Equals(tokens[0 + offset], DB, StringComparison.OrdinalIgnoreCase))
                    {

                        memoryCounter++;
                    }
                    else if (string.Equals(tokens[0 + offset], DW, StringComparison.OrdinalIgnoreCase))
                    {
                        memoryCounter += 2;
                    }
                    else
                    {
                        code.SetSourceLineAddressByLineNumber(memoryCounter,
                            lineIndex);

                        if (AssemblyInstructions.ExpectsOperands(tokens[
                            0 + offset]) == 1)
                        {

                            memoryCounter += 2;
                        }

                        memoryCounter++;
                    }

                }
            }
        }
        return -1;
    }
}
