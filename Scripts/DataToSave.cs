using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public static class DataToSave
{
    public static string filePath = "";

    public static string status = "Idle";
    public static string rtlStatement = "";
    public static string dataMovement = "";
    public static string currentMemoryLocation = "";

    //Memory and IO
    public static string memoryText = "";

    //Break Points
    public static List<int> breakpointList = new List<int>();

    //Trace Results
    public static string traceText = "";

    //View System
    private static int ar = 0x00000000;
    private static int pc = 0x00000000;
    private static int dr = 0x00000000;
    private static int tr = 0x00000000;
    private static int ir = 0x00000000;
    private static int r = 0x00000000;
    private static int ac = 0x00000000;
    private static int z = 0;

    public static void SaveFile()
    {
        using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Write);
        file.StoreString(AllData());
        file.Close();
    }
    public static void OpenFile()
    {
        if (!FileAccess.FileExists(filePath))
        {
            GD.PrintErr("File does not exist: " + filePath);
            return;
        }

        using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
        string content = file.GetAsText();
        file.Close();
        GD.Print("File opened: " + filePath);
        GD.Print("Content: " + content);

        //Make this part so the content will update all the values in here
    }

    private static string AllData()
    {
        string allData =
            "---------- Detail Info   ----------\n" +
            "Status: " + status + "\n" +
            "RtlStatement: " + rtlStatement + "\n" +
            "DataMovement: " + dataMovement + "\n" +
            "CurrentMemoryLocation: " + currentMemoryLocation + "\n\n\n" +
            "---------- Memory And IO ----------\n" +
            "MemoryText: " + memoryText + "\n\n\n" +
            "---------- Break Points  ----------\n" +
            "BreakPointList: " + AllBreakPoints() + "\n\n\n" +
            "---------- Trace Results ----------\n" +
            "TraceText: " + traceText + "\n\n\n" +
            "---------- Registers     ----------\n" +
            "AR: " + ar + "\n" +
            "PC: " + pc + "\n" +
            "DR: " + dr + "\n" +
            "TR: " + tr + "\n" +
            "IR: " + ir + "\n" +
            "R: " + r + "\n" +
            "AC: " + ac + "\n" +
            "Z: " + z + "\n";
        return allData;
    }
    private static string AllBreakPoints()
    {
        string allnum ="";
        foreach(int s in breakpointList)
        {
            allnum += s + " ";
        }
        return allnum;
    }
}
