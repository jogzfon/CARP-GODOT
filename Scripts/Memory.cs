using Godot;
using System;
using System.Text;

public partial class Memory
{
    public const int MEMORY_SIZE = 65536;
    public short[] contents = new short[MEMORY_SIZE];
    private CPU CPUBox;

    public short Read(int address)
    {
        if ((address < 0) || (address >= MEMORY_SIZE))
        {
            return (-1);
        }
        else
        {
            return (contents[address]);
        }

    }

    public bool Write(int address, short data)
    {
        if (address < 0 || address >= MEMORY_SIZE || data < 0 || data > 0xFF)
        {
            return false;
        }

        contents[address] = data;
        if (CPUBox != null)
        {
            // Access the property or call the method on CPU.theBox
            CPUBox.IOint = contents[MEMORY_SIZE];
            CPUBox.IO = AssemblyInstructions.ToNumberString(CPUBox.IOint, 2, 8);
        }
        else
        {
            // Handle the case where CPU.theBox is null
            Console.WriteLine("The box is not present");
        }
        //CPUBox.CanvasRepaint();
        return true;
    }

    public void Clear()
    {
        for (int index = 0; index < MEMORY_SIZE; index++)
        {
            contents[index] = 0;
        }

        /*CPUBox.IO = "00000000";
        CPUBox.canvasRepaint();*/
    }

    public static string[] ToBinaryNybbleStringArray(short byteShort)
    {
        if (byteShort >= 0 && byteShort <= 0xFF)
        {
            string upperNybble = AssemblyInstructions.ToNumberString(byteShort / 0x10, 2, 4);
            string lowerNybble = AssemblyInstructions.ToNumberString(byteShort % 0x10, 2, 4);
            string[] binaryNybble = { upperNybble, lowerNybble };

            return binaryNybble;
        }
        else
        {
            return null;
        }
    }
    public static short FromBinaryNybbleStringArray(string[] nybbleString)
    {
        if (nybbleString != null && nybbleString.Length == 2)
        {
            try
            {
                int upperNybble = Convert.ToInt32(nybbleString[0], 2);
                int lowerNybble = Convert.ToInt32(nybbleString[1], 2);

                return (short)((upperNybble * 0x10) + lowerNybble);
            }
            catch (FormatException)
            {
                return -1;
            }
        }
        else
        {
            return -1;
        }
    }

    public string[] ReadBinaryNybbleStringArray(int address)
    {
        return ToBinaryNybbleStringArray(Read(address));
    }

    public bool WriteBinaryNybbleStringArray(int address, string[] dataStringArray)
    {
        short data = FromBinaryNybbleStringArray(dataStringArray);

        return Write(address, data);
    }

    public void UpdateMemoryTextBox(Label textBox, bool isHex)
    {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < MEMORY_SIZE; i++)
        {
            if (isHex)
            {
                sb.AppendFormat("{0} : {1}", i, Convert.ToString(contents[i], 16).PadLeft(2, '0'));
            }
            else
            {
                sb.AppendFormat("{0} : {1}", i, Convert.ToString(contents[i] & 0xFF, 2).PadLeft(8, '0'));
            }

            sb.AppendLine();

            // Optionally, add line breaks for better readability
            if ((i + 1) % 8 == 0)
            {
                sb.AppendLine();
            }
        }

        textBox.Text = sb.ToString();
    }
    public short[] GetInstructions()
    {
        //return contents.Select(s => (int)s).ToArray();
        return contents;
    }
}
