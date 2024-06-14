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
        //GD.Print("File opened: " + filePath);
        //GD.Print("Content: " + content);
        DistributeValues(content);
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

    private static void DistributeValues(string content)
    {
        List<Tokens> tokens = new List<Tokens>();

        string pattern = @"\b(Status:|RtlStatement:|DataMovement:|CurrentMemoryLocation:|MemoryText:|BreakPointList:|TraceText:|AR:|PC:|DR:|TR:|IR:|R:|AC:|Z:)\b|""[^""]*""|'[^']*'|\b[\w']+\b|\S";

        string[] lines = content.Split('\n');

        foreach (string line in lines)
        {
            if (!line.Trim().StartsWith("-"))
            {
                tokens = TokenizeLines(line, pattern, tokens);
            }
        }
        /*foreach (Tokens token in tokens)
        {
            GD.Print("Type: "+ token.type+ " Value: "+token.value);
        }*/
        for (int i = 0; i < tokens.Count; i++)
        {
            i = SetValues(tokens, i);
        }
        GD.Print(AllData());
    }
    private static List<Tokens> TokenizeLines(string line, string pattern, List<Tokens> tokens)
    {
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
                else if (Regex.IsMatch(token, @"\b(Status|RtlStatement|DataMovement|CurrentMemoryLocation|MemoryText|BreakPointList|TraceText|AR|PC|DR|TR|IR|R|AC|Z)\b"))
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

    private static int SetValues(List<Tokens> tokens, int index)
    {
        switch (tokens[index].value)
        {
            case "Status":
                if(tokens[index+2].type!= TokenType.LABEL)
                {
                    index += 2;
                    status = tokens[index].value;
                }
                else
                {
                    index++;
                }
                break;
            case "RtlStatement":
                if (tokens[index + 2].type != TokenType.LABEL)
                {
                    index += 2;
                    rtlStatement = tokens[index].value;
                }
                else
                {
                    index++;
                }
                break;
            case "DataMovement":
                if (tokens[index + 2].type != TokenType.LABEL)
                {
                    index += 2;
                    dataMovement = tokens[index].value;
                }
                else
                {
                    index++;
                }
                break;
            case "CurrentMemoryLocation":
                if (tokens[index + 2].type != TokenType.LABEL)
                {
                    index += 2;
                    currentMemoryLocation = tokens[index].value;
                }
                else
                {
                    index++;
                }
                break;
            case "MemoryText":
                if (tokens[index + 2].type != TokenType.LABEL)
                {
                    index += 2;
                    memoryText = tokens[index].value;
                }
                else
                {
                    index++;
                }
                break;
            case "BreakPointList":
                if (tokens[index + 2].type != TokenType.LABEL)
                {
                    index += 2;
                    GD.Print(tokens[index].value);
                    while (tokens[index].type != TokenType.LABEL)
                    {
                        breakpointList.Add(Int32.Parse(tokens[index].value));
                        index++;
                    }
                }
                else
                {
                    index++;
                }
                break;
            case "TraceText":
                if (tokens[index + 2].type != TokenType.LABEL)
                {
                    index += 2;
                    traceText = tokens[index].value;
                }
                else
                {
                    index++;
                }
                break;
            case "AR":
                if (tokens[index + 2].type != TokenType.LABEL)
                {
                    index += 2;
                    ar = Int32.Parse(tokens[index].value);
                }
                else
                {
                    index++;
                }
                break;
            case "PC":
                if (tokens[index + 2].type != TokenType.LABEL)
                {
                    index += 2;
                    pc = Int32.Parse(tokens[index].value);
                }
                else
                {
                    index++;
                }
                break;
            case "DR":
                if (tokens[index + 2].type != TokenType.LABEL)
                {
                    index += 2;
                    dr = Int32.Parse(tokens[index].value);
                }
                else
                {
                    index++;
                }
                break;
            case "TR":
                if (tokens[index + 2].type != TokenType.LABEL)
                {
                    index += 2;
                    tr = Int32.Parse(tokens[index].value);
                }
                else
                {
                    index++;
                }
                break;
            case "IR":
                if (tokens[index + 2].type != TokenType.LABEL)
                {
                    index += 2;
                    ir = Int32.Parse(tokens[index].value);
                }
                else
                {
                    index++;
                }
                break;
            case "R":
                if (tokens[index + 2].type != TokenType.LABEL)
                {
                    index += 2;
                    r = Int32.Parse(tokens[index].value);
                }
                else
                {
                    index++;
                }
                break;
            case "AC":
                if (tokens[index + 2].type != TokenType.LABEL)
                {
                    index += 2;
                    ac = Int32.Parse(tokens[index].value);
                }
                else
                {
                    index++;
                }
                break;
            case "Z":
                if (tokens[index + 2].type != TokenType.LABEL)
                {
                    index += 2;
                    z = Int32.Parse(tokens[index].value);
                }
                else
                {
                    index++;
                }
                break;
            default:
                GD.PrintErr("Label " + tokens[index].value + " does not Exist!");
                break;
        }
        return index;
    }
}
