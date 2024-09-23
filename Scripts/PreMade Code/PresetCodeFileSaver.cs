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
    static string fileName = "presetCodeData.data";

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
            string filePath = Path.Combine(dir.GetCurrentDir(), fileName);

            using var file = Godot.FileAccess.Open(filePath, Godot.FileAccess.ModeFlags.Write);

            //file.StoreString(accountTemplate);
            file.StoreString(presetCodeTemplate);
            file.Close();
            return;
        }
        try
        {
            string filePath = Path.Combine(dir.GetCurrentDir(), fileName);

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

        string filePath = Path.Combine(directoryLoc, fileName);

        if (Godot.FileAccess.FileExists(filePath))
        {
            try
            {
                using var file = Godot.FileAccess.Open(filePath, Godot.FileAccess.ModeFlags.Read);
                //string content = file.GetAsText();
                string text = file.GetAsText();
                file.Close();

                //DistributeAccountValues(content, user);
                DistributePresetCodeValue(text);
                //GD.Print(user);
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Failed to delete account file: {ex.Message}");
            }
        }
    }
    public static void DistributePresetCodeValue(string content)
    {
        List<List<Tokens>> tokens = new List<List<Tokens>>();
        string pattern = @"\b(Keyword:|Instructions:)\b|""[^""]*""|'[^']*'|\b[\w']+\b|\S";

        string[] lines = content.Split('\n');

        foreach (string line in lines)
        {
            if (!line.Trim().StartsWith("-"))
            {
                tokens.Add(TokenizeAccountLines(line.Replace(",", ""), pattern));
            }
        }
        for (int i = 0; i < tokens.Count; i+=2)
        {
            var keyword = SetPresetCodeValues(tokens[i]);
            var value = SetPresetCodeValues(tokens[i+1]);
            presetCodeList.AddSetOfCodes(keyword, value);
        }
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
    private static string SetPresetCodeValues(List<Tokens> tokens)
    {
        switch (tokens[0].value)
        {
            case "Keyword":
                if (2 < tokens.Count)
                {
                    string preset_keyword = "";
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        preset_keyword += tokens[i].value;
                    }
                    return preset_keyword;
                }
                break;
            case "Instructions":
                if (2 < tokens.Count)
                {
                    string preset_instructions = "";
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        preset_instructions += tokens[i].value;
                    }
                    return preset_instructions;
                }
                break;
            default:
                GD.PrintErr("Label " + tokens[0].value + " does not exist!");
                break;
        }
        return null;
    }

    public static void SetPresetCodeList(PremadeCodeList presetCodes)
    {
        presetCodeList = presetCodes;
    }
}
