using Godot;
using System;

public class AssemblyInstructions
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

    public static short ToMnemonicCode(string mnemonic)
    {
        switch (mnemonic.ToUpper())
        {
            case "NOP":
                return opcodeNOP;
            case "LDAC":
                return opcodeLDAC;
            case "STAC":
                return opcodeSTAC;
            case "MVAC":
                return opcodeMVAC;
            case "MOVR":
                return opcodeMOVR;
            case "JUMP":
                return opcodeJUMP;
            case "JMPZ":
                return opcodeJMPZ;
            case "JPNZ":
                return opcodeJPNZ;
            case "ADD":
                return opcodeADD;
            case "SUB":
                return opcodeSUB;
            case "INAC":
                return opcodeINAC;
            case "CLAC":
                return opcodeCLAC;
            case "AND":
                return opcodeAND;
            case "OR":
                return opcodeOR;
            case "XOR":
                return opcodeXOR;
            case "NOT":
                return opcodeNOT;
            case "END":
                return opcodeEND;
            default:
                // Return -1 if failed to match assembly mnemonic
                return -1;
        }
    }

    public static string ToMnemonic(short code)
    {
        switch (code)
        {
            case opcodeNOP:
                return NOP;
            case opcodeLDAC:
                return LDAC;
            case opcodeSTAC:
                return STAC;
            case opcodeMVAC:
                return MVAC;
            case opcodeMOVR:
                return MOVR;
            case opcodeJUMP:
                return JUMP;
            case opcodeJMPZ:
                return JMPZ;
            case opcodeJPNZ:
                return JPNZ;
            case opcodeADD:
                return ADD;
            case opcodeSUB:
                return SUB;
            case opcodeINAC:
                return INAC;
            case opcodeCLAC:
                return CLAC;
            case opcodeAND:
                return AND;
            case opcodeOR:
                return OR;
            case opcodeXOR:
                return XOR;
            case opcodeNOT:
                return NOT;
            case opcodeEND:
                return END;
            default:
                return null;
        }
    }
    public static bool isMnemonic(string str)
    {
        str = str.ToUpper(); // Convert to uppercase for case-insensitive comparison

        if (str == NOP ||
            str == LDAC ||
            str == STAC ||
            str == MVAC ||
            str == MOVR ||
            str == JUMP ||
            str == JMPZ ||
            str == JPNZ ||
            str == ADD ||
            str == SUB ||
            str == INAC ||
            str == CLAC ||
            str == AND ||
            str == OR ||
            str == XOR ||
            str == NOT ||
            str == END)
        {
            return true;
        }

        return false;
    }
    public static short[] ToInstructionCode(string[] instruction)
    {
        if (instruction == null || instruction.Length == 0)
        {
            return null;
        }
        else
        {
            if (instruction[0] != null && (instruction[0].Equals(NOP, StringComparison.OrdinalIgnoreCase) ||
                instruction[0].Equals(MVAC, StringComparison.OrdinalIgnoreCase) ||
                instruction[0].Equals(MOVR, StringComparison.OrdinalIgnoreCase) ||
                instruction[0].Equals(ADD, StringComparison.OrdinalIgnoreCase) ||
                instruction[0].Equals(SUB, StringComparison.OrdinalIgnoreCase) ||
                instruction[0].Equals(INAC, StringComparison.OrdinalIgnoreCase) ||
                instruction[0].Equals(CLAC, StringComparison.OrdinalIgnoreCase) ||
                instruction[0].Equals(AND, StringComparison.OrdinalIgnoreCase) ||
                instruction[0].Equals(OR, StringComparison.OrdinalIgnoreCase) ||
                instruction[0].Equals(XOR, StringComparison.OrdinalIgnoreCase) ||
                instruction[0].Equals(NOT, StringComparison.OrdinalIgnoreCase) ||
                instruction[0].Equals(END, StringComparison.OrdinalIgnoreCase)) && (instruction.Length < 2))
            {
                short[] instructionCode = new short[1];
                instructionCode[0] = ToMnemonicCode(instruction[0]);
                return instructionCode;
            }
            else if (instruction[0] != null && (instruction[0].Equals(LDAC, StringComparison.OrdinalIgnoreCase) ||
                instruction[0].Equals(STAC, StringComparison.OrdinalIgnoreCase) ||
                instruction[0].Equals(JUMP, StringComparison.OrdinalIgnoreCase) ||
                instruction[0].Equals(JMPZ, StringComparison.OrdinalIgnoreCase) ||
                instruction[0].Equals(JPNZ, StringComparison.OrdinalIgnoreCase)) && instruction.Length < 3 && isAddress(instruction[1]))
            {
                short[] instructionCode = new short[3];
                short[] addressCode = toAddressCode(toAddressInteger(instruction[1]));

                instructionCode[0] = ToMnemonicCode(instruction[0]);
                instructionCode[1] = addressCode[0];
                instructionCode[2] = addressCode[1];

                return instructionCode;
            }
            return null;
        }
    }
    public static short[] toAddressCode(int address)
    {

        if (isAddress(address))
        {
            short[] codeByteArray = { (short)(address % 0x100), (short)((address / 0x100) % 0x100) };

            return (codeByteArray);
        }
        else
        {
            return (null);
        }

    }
    public static int toAddressInteger(string addressString)
    {
        int address;

        if (!string.IsNullOrEmpty(addressString))
        {
            char baseSpecifier = addressString[addressString.Length - 1];
            int addressRadix = ToRadix(baseSpecifier);

            if (addressRadix != -1)
            {
                addressString = addressString.Substring(0, addressString.Length - 1);
            }
            else
            {
                addressRadix = 10;
            }

            try
            {
                address = Convert.ToInt32(addressString, addressRadix);
            }
            catch (FormatException e)
            {
                return -1;
            }
        }
        else
        {
            return (-1);
        }

        if (isAddress(address))
        {
            return (address);
        }
        else
        {
            return (-1);
        }
    }

    public static bool IsValidOperands(string mnemonic, string[] operands)
    {
        mnemonic = mnemonic.ToUpper(); // Convert to uppercase for case-insensitive comparison

        if (mnemonic == NOP ||
            mnemonic == MVAC ||
            mnemonic == MOVR ||
            mnemonic == ADD ||
            mnemonic == SUB ||
            mnemonic == INAC ||
            mnemonic == CLAC ||
            mnemonic == AND ||
            mnemonic == OR ||
            mnemonic == XOR ||
            mnemonic == NOT ||
            mnemonic == END)
        {
            return (operands == null || operands.Length == 0 || (operands.Length == 1 && string.IsNullOrEmpty(operands[0])));
        }
        else if (mnemonic == LDAC || mnemonic == STAC || mnemonic == JUMP ||
            mnemonic == JMPZ || mnemonic == JPNZ)
        {
            return (operands != null && operands.Length == 1 && isAddress(operands[0]));
        }

        return false;
    }

    public static int ExpectsOperands(string mnemonic)
    {
        mnemonic = mnemonic.ToUpper(); // Convert to uppercase for case-insensitive comparison

        if (mnemonic == NOP ||
            mnemonic == MVAC ||
            mnemonic == MOVR ||
            mnemonic == ADD ||
            mnemonic == SUB ||
            mnemonic == INAC ||
            mnemonic == CLAC ||
            mnemonic == AND ||
            mnemonic == OR ||
            mnemonic == XOR ||
            mnemonic == NOT ||
            mnemonic == END)
        {
            return 0;
        }
        else if (mnemonic == LDAC ||
            mnemonic == STAC ||
            mnemonic == JUMP ||
            mnemonic == JMPZ ||
            mnemonic == JPNZ)
        {
            return 1;
        }

        return -1;
    }

    public static short ToByteShort(string byteString)
    {
        short byteShort;

        if (!string.IsNullOrEmpty(byteString))
        {
            char baseSpecifier = byteString[byteString.Length - 1];
            int byteRadix = ToRadix(baseSpecifier);

            if (byteRadix != -1)
            {
                byteString = byteString.Substring(0, byteString.Length - 1);
            }
            else
            {
                byteRadix = 10;
            }

            try
            {
                byteShort = Convert.ToInt16(byteString, byteRadix);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
        }
        else
        {
            return -1;
        }

        if (isByte(byteShort))
        {
            return (byteShort);
        }
        else
        {
            return (-1);
        }
    }

    public static int ToWordInteger(string wordString)
    {
        int wordInteger;

        if (!string.IsNullOrEmpty(wordString))
        {
            char baseSpecifier = wordString[wordString.Length - 1];
            int wordRadix = ToRadix(baseSpecifier);

            if (wordRadix != -1)
            {
                wordString = wordString.Substring(0, wordString.Length - 1);
            }
            else
            {
                wordRadix = 10;
            }

            try
            {
                wordInteger = Convert.ToInt32(wordString, wordRadix);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }
        else
        {
            return -1;
        }

        if (isWord(wordInteger))
        {
            return (wordInteger);
        }
        else
        {
            return (-1);
        }
    }


    public static bool isAddress(String str)
    {
        return (toAddressInteger(str) != -1);
    }
    public static bool isAddress(int num)
    {
        return num >= 0 && num <= 0xFFFF;
    }
    public static bool isByte(short num)
    {
        return ((num >= 0) && (num <= 0xFF));
    }

    public static bool isWord(int num)
    {
        return ((num >= 0) && (num <= 0xFFFF));
    }

    public static bool isConstantByte(short num)
    {
        return ((num >= 0) && (num <= 0xFF));
    }

    public static bool isConstantWord(int num)
    {
        return ((num >= 0) && (num <= 0xFFFF));
    }

    public static int ToRadix(char ch)
    {
        ch = char.ToLower(ch);

        switch (ch)
        {
            case 'b':
                return 2;
            case 'o':
            case 'q':
                return 8;
            case 'd':
                return 10;
            case 'h':
                return 16;
            default:
                return -1;
        }
    }

    public static short[] ToWordCode(int word)
    {
        if (isWord(word))
        {
            short[] codeByteArray = { (short)(word % 0x100), (short)((word / 0x100) % 0x100) };
            return codeByteArray;
        }
        else
        {
            return null;
        }
    }

    public static string ToNumberString(long num, int radix, int length)
    {
        string str = Convert.ToString(num, radix);

        while (str.Length < length)
        {
            str = "0" + str;
        }

        return str.Substring(str.Length - length);
    }
}
