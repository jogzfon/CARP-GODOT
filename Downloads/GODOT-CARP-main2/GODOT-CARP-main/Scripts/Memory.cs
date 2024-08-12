using Godot;
using System;
using System.Text;

public static class Memory
{
    public static Animations animation;
    public const int MEMORY_SIZE = 65536;
    public static short[] contents = new short[MEMORY_SIZE];
    public static short Read(int address)
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

    public static bool Write(int address, short data)
    {
        if (address < 0 || address >= MEMORY_SIZE || data < 0 || data > 0xFF)
        {
            return false;
        }

        contents[address] = data;
        if (animation != null)
        {
            // Access the property or call the method on CPU.theBox
            animation.IOint = contents[MEMORY_SIZE];
            animation.IO = AssemblyInstructions.ToNumberString(animation.IOint, 2, 8);
        }
        else
        {
            // Handle the case where CPU.theBox is null
            Console.WriteLine("The box is not present");
        }
        //CPUBox.CanvasRepaint();
        return true;
    }

    public static void Clear()
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

    public static string[] ReadBinaryNybbleStringArray(int address)
    {
        return ToBinaryNybbleStringArray(Read(address));
    }

    public static bool WriteBinaryNybbleStringArray(int address, string[] dataStringArray)
    {
        short data = FromBinaryNybbleStringArray(dataStringArray);

        return Write(address, data);
    }

    public static void UpdateMemoryTextBox(TextEdit textBox, bool isHex)
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
    public static short[] GetInstructions()
    {
        //return contents.Select(s => (int)s).ToArray();
        return contents;
    }
}
