using Godot;
using System;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection.Metadata;
public static class PresetCodeFileSaver
{
    static string directoryLoc = "PresetCodeData";

    static PremadeCodeList presetCodeList;

    public static void SavePresetCode(string keyword, string instructions)
    {
        // Create the account template string
        string presetCodeTemplate = "Keyword: " + keyword + "\n" + 
            "Instructions: " + instructions;

        DirAccess dir = DirAccess.Open(directoryLoc);
        if (dir == null)
        {
            dir.MakeDir(directoryLoc);
            string filePath = Path.Combine(dir.GetCurrentDir(), keyword + ".data");

            using var file = Godot.FileAccess.Open(filePath, Godot.FileAccess.ModeFlags.Write);

            //file.StoreString(accountTemplate);
            file.StoreString(presetCodeTemplate);
            file.Close();
            return;
        }
        try
        {
            string filePath = Path.Combine(dir.GetCurrentDir(), keyword + ".data");

            using var file = Godot.FileAccess.Open(filePath, Godot.FileAccess.ModeFlags.Write);

            //file.StoreString(accountTemplate);
            file.StoreString(presetCodeTemplate);
            file.Close();
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Failed to save account data: {ex.Message}");
        }
    }
    public static void GetPresetCodeFile()
    {
        // Open the directory
        DirAccess dir = DirAccess.Open(directoryLoc);
        if (dir == null)
        {
            GD.PrintErr("Failed to open directory: " + directoryLoc);
            DirAccess.MakeDirAbsolute(directoryLoc);
            return;
        }

        // Iterate through the directory contents
        dir.ListDirBegin(); // Start listing the directory contents

        string fileName = dir.GetNext();
        while (fileName != "")
        {
            // Skip special entries "." and ".."
            if (!dir.CurrentIsDir() && fileName.EndsWith(".data"))
            {
                string filePath = Path.Combine(dir.GetCurrentDir(), fileName);
                using var file = Godot.FileAccess.Open(filePath, Godot.FileAccess.ModeFlags.Read);
                string content = file.GetAsText();

                DistributePresetCodeValue(content);
            }

            fileName = dir.GetNext(); // Get the next file
        }

        dir.ListDirEnd(); // Finish listing the directory contents
    }
    public static void DistributePresetCodeValue(string content)
    {
        List<List<Tokens>> tokens = new List<List<Tokens>>();
        string pattern = @"\b(Keyword:|Instructions:)\b|""[^""]*""|'[^']*'|\b[\w']+\b|\S|\s+|\n|\r\n";

        string[] lines = content.Split('\n');

        foreach (string line in lines)
        {
            tokens.Add(TokenizeAccountLines(line, pattern));
        }
        string keyword = String.Empty;
        string value = String.Empty;

        for (int i = 0; i < tokens.Count; i++)
        {
            var temp = SetPresetCodeValues(tokens[i]);

            if (temp.type.Equals("Keyword"))
            {
                keyword += temp.content;
            }
            else if(temp.type.Equals("Instructions"))
            {
                value += temp.content;
            }
            else
            {
                value += "\n";
            }
        }
        presetCodeList.AddSetOfCodes(keyword, value);
        //GD.Print("Keyword: " + keyword + "\nInstructions: " + value);
    }
    private static List<Tokens> TokenizeAccountLines(string line, string pattern)
    {
        List<Tokens> tokens = new List<Tokens>();
        MatchCollection matches = Regex.Matches(line, pattern);

        foreach (Match match in matches)
        {
            string token = match.Value;

            if (!string.IsNullOrEmpty(token) || token != ",")
            {
                if (Regex.IsMatch(token, ":"))
                {
                    tokens.Add(new Tokens(TokenType.COLON, token));
                }
                else if (Regex.IsMatch(token, @"\b(Keyword|Instructions)\b"))
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
    private static (string type, string content) SetPresetCodeValues(List<Tokens> tokens)
    {
        string content = "";
        if (tokens.Count == 0)
        {
            return ("Instructions", "\n");
        }

        switch (tokens[0].value)
        {
            case "Keyword":
                if (3 < tokens.Count)
                {
                    for (int i = 3; i < tokens.Count; i++)
                    {
                        content += tokens[i].value;
                    }
                    return ("Keyword", content);
                }
                break;
            case "Instructions":
                if (3 < tokens.Count)
                {
                    for (int i = 3; i < tokens.Count; i++)
                    {
                        content += tokens[i].value;
                    }
                    return ("Instructions", content + "\n");
                }
                break;
            default:
                for (int i = 0; i < tokens.Count; i++)
                {
                    content += tokens[i].value;
                }
                return ("Instructions", content + "\n");
        }
        return ("Instructions", "\n");
    }

    public static void SetPresetCodeList(PremadeCodeList presetCodes)
    {
        presetCodeList = presetCodes;
    }
}
