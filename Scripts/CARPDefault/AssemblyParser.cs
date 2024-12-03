using System;
using System.Collections.Generic;
using System.Diagnostics;
public partial class AssemblyParser
{
    int i = 0;

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

    // Called when the node enters the scene tree for the first time.
    public AssemblyParser()
    {
        memorycode = Memory.contents;
    }

    #region Animation Controls
    public void StartAnimation(int memoryStartLocation)
    {
        for (i = memoryStartLocation; i < memorycode.Length; i++)
        {
            FETCH1();
            FETCH2();
            FETCH3();
            switch (memorycode[i])
            {
                case opcodeNOP:
                    NOP();
                    break;
                case opcodeLDAC:
                    i++;
                    LDAC1();
                    LDAC2();
                    LDAC3();
                    LDAC4();
                    LDAC5();
                    break;
                case opcodeSTAC:
                    i++;
                    STAC1();
                    STAC2();
                    STAC3();
                    STAC4();
                    STAC5();
                    break;
                case opcodeMVAC:
                    MVAC();
                    break;
                case opcodeMOVR:
                    MOVR();
                    break;
                case opcodeJUMP:
                    i++;
                    JUMP();
                    break;
                case opcodeJMPZ:
                    i++;
                    JMPZ();
                    break;
                case opcodeJPNZ:
                    i++;
                    JPNZ();
                    break;
                case opcodeADD:
                    ADD();
                    break;
                case opcodeSUB:
                    SUB();
                    break;
                case opcodeINAC:
                    INAC();
                    break;
                case opcodeCLAC:
                    CLAC();
                    break;
                case opcodeAND:
                    AND();
                    break;
                case opcodeOR:
                    OR();
                    break;
                case opcodeXOR:
                    XOR();
                    break;
                case opcodeNOT:
                    NOT();
                    break;
                case opcodeEND:
                    END();
                    return;
                default:
                    // Handle unknown instructions or implement additional instructions
                    Debug.Print("Instruction Does not Exist");
                    break;
            }
        }
        
    }
    
    public void ResetRegisters()
    {
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
    public void FETCH1()
    {
        ar_bit = pc_bit;

        TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public void FETCH2()
    {
        pc_bit = pc_bit + 1;
        dr_bit = memorycode[i];
    
        TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public void FETCH3()
    {
        ar_bit = pc_bit;
        ir_bit = dr_bit;

        TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    #endregion
    #region DataMovement
    #region LDAC
    public void LDAC1()
    {
        dr_bit = memorycode[i];
        ar_bit = ar_bit + 1;
        pc_bit = pc_bit + 1;

        TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public void LDAC2()
    {
        tr_bit = dr_bit;
        dr_bit = memorycode[i];
        pc_bit = pc_bit + 1;

        TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public void LDAC3()
    {
        ar_bit = dr_bit | tr_bit;

        TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public void LDAC4()
    {
        dr_bit = memorycode[i];

        TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public void LDAC5()
    {
        ac_bit = dr_bit;

        TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    #endregion

    #region STAC
    public void STAC1()
    {
        dr_bit = memorycode[i];
        ar_bit = ar_bit + 1;
        pc_bit = pc_bit + 1;

        TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public void STAC2()
    {
        tr_bit = dr_bit;
        dr_bit = memorycode[i];
        pc_bit = pc_bit + 1;

        TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public void STAC3()
    {
        ar_bit = dr_bit | tr_bit;

        TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public void STAC4()
    {
        dr_bit = ac_bit;

        TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public void STAC5()
    {
        memorycode[i] = (short)dr_bit;

        TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    #endregion

    public void MVAC()
    {
        r_bit = ac_bit;

        TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public void MOVR()
    {
        ac_bit = r_bit;
        
        TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public void JUMP()
    {
        int position = memorycode[i] - 1;
        if (position >= 0 && position < memorycode.Length)
        {
            TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
            i = position;
        }
        else
        {
            // Handle invalid jump, e.g., by throwing an exception or setting position to a default value
            Console.WriteLine("Invalid memory line JUMP");
        }
    }
    public void JMPZ()
    {
        if (z_bit == 1)
        {
            int position = memorycode[i] - 1;
            TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
            i = position;
        }
        else
        {
            TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
        }
    }
    public void JPNZ()
    {
        if (z_bit == 0)
        {
            int position = memorycode[i] - 1;
            TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
            i = position;
        }
        else
        {
            TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
        }
    }

    public void NOP()
    {
        TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }

    public void END()
    {
        TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    #endregion
    #region Arithmetic
    public void ADD()
    {
        ac_bit = ac_bit + r_bit;

        // Set z based on the result
        z_bit = (ac_bit == 0) ? 1 : 0;

        TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public void SUB()
    {
        ac_bit = ac_bit - r_bit;

        // Set z based on the result
        z_bit = (ac_bit == 0) ? 1 : 0;

        TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public void INAC()
    {
        ac_bit = ac_bit + 1;

        // Set z based on the result
        z_bit = (ac_bit == 0) ? 1 : 0;

        TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public void CLAC()
    {
        ac_bit = 0;
        z_bit = 1;
        TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    #endregion
    #region Logical
    public void AND()
    {
        // Perform bitwise AND on ac and r
        ac_bit = ac_bit & r_bit;

        // Set z based on the result
        z_bit = (ac_bit == 0) ? 1 : 0;

        TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public void OR()
    {
        ac_bit = ac_bit | 1;
        z_bit = (ac_bit == 0) ? 1 : 0;

        TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public void XOR()
    {
        // Perform bitwise XOR on ac and 1
        ac_bit = ac_bit ^ 1;

        // Set z based on the result
        z_bit = (ac_bit == 0) ? 1 : 0;

        TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    public void NOT()
    {

        // Perform bitwise NOT on ac
        ac_bit = ~ac_bit;

        // Set z based on the result
        z_bit = (ac_bit == 0) ? 1 : 0;

        TraceResults.AddResult("rtlStatement", "Data Movement", ar_bit, pc_bit, dr_bit, tr_bit, ir_bit, r_bit, ac_bit, z_bit);
    }
    #endregion
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
