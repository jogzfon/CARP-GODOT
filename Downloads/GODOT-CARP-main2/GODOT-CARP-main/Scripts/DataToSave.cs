using Godot;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
public static class DataToSave
{
    public static string filePath = "";

    public static string status = "Idle";
    public static string rtlStatement = "";
    public static string dataMovement = "";
    public static string currentMemoryLocation = "";
    public static string intructionCodes = "";

    //Memory and IO
    public static short[] memoryContents = new short[Memory.MEMORY_SIZE];

    //Break Points
    public static List<int> breakpointList = new List<int>();

    //Trace Results
    public static string traceText = "";

    //View System
    public static int ar = 0x00000000;
    public static int pc = 0x00000000;
    public static int dr = 0x00000000;
    public static int tr = 0x00000000;
    public static int ir = 0x00000000;
    public static int r = 0x00000000;
    public static int ac = 0x00000000;
    public static int z = 0;

    private static bool isInstructionCode = false;

    public static void SaveFile()
    {
        using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Write);

        if (file != null)
        {
            file.StoreString(AllData());
            file.Close();
        }
        ResetDatas();
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
        //GD.Print("File opened: " + filePath);
        //GD.Print("Content: " + content);
        DistributeValues(content);
        //Make this part so the content will update all the values in here
    }

    #region Get All Datas
    private static string AllData()
    {
        string allData =
            "---------- Detail Info   ----------\n" +
            "Status: " + status + "\n" +
            "RtlStatement: " + rtlStatement + "\n" +
            "DataMovement: " + dataMovement + "\n" +
            "CurrentMemoryLocation: " + currentMemoryLocation + "\n" +
            "InstructionCodes: "+ intructionCodes + "\n\n\n" +
            "---------- Memory And IO ----------\n" +
            "MemoryContents: " + AllMemoryContent() + "\n\n\n" +
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
    private static string AllMemoryContent()
    {
        string allnum = "";
        foreach (short s in memoryContents)
        {
            allnum += s + " ";
        }
        return allnum;
    }
    #endregion

    #region Distribute Values
    private static void DistributeValues(string content)
    {
        List<List<Tokens>> tokens = new List<List<Tokens>>();

        string pattern = @"\b(Status:|RtlStatement:|DataMovement:|CurrentMemoryLocation:|InstructionCodes:|MemoryContents:|BreakPointList:|TraceText:|AR:|PC:|DR:|TR:|IR:|R:|AC:|Z:)\b|""[^""]*""|'[^']*'|\b[\w']+\b|\S";

        string[] lines = content.Split('\n');

        foreach (string line in lines)
        {
            if (!line.Trim().StartsWith("-"))
            {
                tokens.Add(TokenizeLines(line, pattern));
            }
        }
       /* foreach (List<Tokens> token in tokens)
        {
            foreach(Tokens tok in token)
            {
                GD.Print("Type: "+ tok.type+ " Value: "+ tok.value);
            }
            GD.Print(token.Count+" ENDLINE----------------\n\n");
        }*/
        foreach (List<Tokens> token in tokens)
        {
            if (token.Count > 0)
            {
                SetValues(token);
            }
        }
        //GD.Print(AllData());
    }
    private static List<Tokens> TokenizeLines(string line, string pattern)
    {
        List<Tokens> tokens = new List<Tokens>();
        MatchCollection matches = Regex.Matches(line, pattern);

        foreach (Match match in matches)
        {
            string token = match.Value;

            if (!string.IsNullOrEmpty(token))
            {
                if (Regex.IsMatch(token, ":"))
                {
                    tokens.Add(new Tokens(TokenType.COLON, token));
                }
                else if (Regex.IsMatch(token, @"\b(Status|RtlStatement|DataMovement|CurrentMemoryLocation|InstructionCodes|MemoryContents|BreakPointList|TraceText|AR|PC|DR|TR|IR|R|AC|Z)\b"))
                {
                    tokens.Add(new Tokens(TokenType.LABEL, token));
                }
                else
                {
                    tokens.Add(new Tokens(TokenType.VALUE, token));
                }
            }
        }
        return tokens;
    }
    private static void SetValues(List<Tokens> tokens)
    {
        switch (tokens[0].value)
        {
            case "Status":
                if (2 < tokens.Count)
                {
                    for (int i = 2; i<tokens.Count; i++)
                    {
                        status = tokens[2].value+" ";
                    } 
                }
                break;
            case "RtlStatement":
                if (2 < tokens.Count)
                {
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        rtlStatement += tokens[i].value + " ";
                    }
                }
                break;
            case "DataMovement":
                if (2 < tokens.Count)
                {
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        dataMovement += tokens[i].value + " ";
                    }
                }
                break;
            case "CurrentMemoryLocation":
                if (2 < tokens.Count)
                {
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        currentMemoryLocation += tokens[i].value + " ";
                    }
                }
                break;
            case "InstructionCodes":
                if (2 < tokens.Count)
                {
                    isInstructionCode = true;
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        intructionCodes += tokens[i].value + " ";
                    }
                    intructionCodes += "\n";
                }
                break;
            case "MemoryContents":
                if (2 < tokens.Count)
                {
                    isInstructionCode = false;
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        memoryContents[i-2] = Int16.Parse(tokens[i].value);
                    }
                }
                break;
            case "BreakPointList":
                if (2 < tokens.Count)
                {
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        breakpointList.Add(Int32.Parse(tokens[i].value));
                    }
                }
                break;
            case "TraceText":
                if (2 < tokens.Count)
                {
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        traceText += tokens[i].value + " ";
                    }
                }
                break;
            case "AR":
                if (2 < tokens.Count)
                {
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        ar = Int32.Parse(tokens[i].value);
                    }
                }
                break;
            case "PC":
                if (2 < tokens.Count)
                {
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        pc = Int32.Parse(tokens[i].value);
                    }
                }
                break;
            case "DR":
                if (2 < tokens.Count)
                {
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        dr = Int32.Parse(tokens[i].value);
                    }
                }
                break;
            case "TR":
                if (2 < tokens.Count)
                {
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        tr = Int32.Parse(tokens[i].value);
                    }
                }
                break;
            case "IR":
                if (2 < tokens.Count)
                {
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        ir = Int32.Parse(tokens[i].value);
                    }
                }
                break;
            case "R":
                if (2 < tokens.Count)
                {
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        r = Int32.Parse(tokens[i].value);
                    }
                }
                break;
            case "AC":
                if (2 < tokens.Count)
                {
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        ac = Int32.Parse(tokens[i].value);
                    }
                }
                break;
            case "Z":
                if (2 < tokens.Count)
                {
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        z = Int32.Parse(tokens[i].value);
                    }
                }
                break;

            default:
                if (isInstructionCode)
                {
                    for (int i = 0; i < tokens.Count; i++)
                    {
                        intructionCodes += tokens[i].value + " ";
                    }
                    intructionCodes += "\n";
                }
                else
                {
                    GD.PrintErr("Label " + tokens[0].value + " does not exist!");
                }
                break;
        }
       
    }
    #endregion

    public static void ResetDatas()
    {
        filePath = "";

        status = "Idle";
        rtlStatement = "";
        dataMovement = "";
        currentMemoryLocation = "";
        intructionCodes = "";

        //Memory and IO
        memoryContents = new short[Memory.MEMORY_SIZE];

        //Break Points
        breakpointList = new List<int>();

        //Trace Results
        traceText = "";

        //View System
        ar = 0x00000000;
        pc = 0x00000000;
        dr = 0x00000000;
        tr = 0x00000000;
        ir = 0x00000000;
        r = 0x00000000;
        ac = 0x00000000;
        z = 0;
    }
}
