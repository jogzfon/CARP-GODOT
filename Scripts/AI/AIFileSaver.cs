using Godot;
using System;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection.Metadata;

public static class AIFileSaver
{
    static string directoryLoc = "AccountData";
    static string fileName = "aiData.carpacc";
    public static void SaveAIAPI(string apiKey)
    {
        // Create the account template string
        string keyAPITemplate = "API_Key: " + apiKey + "\n";

        byte[] encryptedData = InfoSecure.EncryptData(keyAPITemplate);

        DirAccess dir = DirAccess.Open(directoryLoc);
        if (dir == null)
        {
            dir.MakeDir(directoryLoc);
            string filePath = Path.Combine(dir.GetCurrentDir(), fileName);

            using var file = Godot.FileAccess.Open(filePath, Godot.FileAccess.ModeFlags.Write);

            //file.StoreString(accountTemplate);
            file.StoreBuffer(encryptedData);
            file.Close();
            return;
        }
        try
        {
            string filePath = Path.Combine(dir.GetCurrentDir(), fileName);

            using var file = Godot.FileAccess.Open(filePath, Godot.FileAccess.ModeFlags.Write);

            //file.StoreString(accountTemplate);
            file.StoreBuffer(encryptedData);
            file.Close();
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Failed to save account data: {ex.Message}");
        }
    }
    public static string GetAIAPIFile()
    {

        string filePath = Path.Combine(directoryLoc, fileName);

        if (Godot.FileAccess.FileExists(filePath))
        {
            try
            {
                using var file = Godot.FileAccess.Open(filePath, Godot.FileAccess.ModeFlags.Read);
                //string content = file.GetAsText();
                byte[] encryptedData = file.GetBuffer((int)file.GetLength());
                file.Close();

                // Decrypt the data using the password
                string decryptedContent = InfoSecure.DecryptData(encryptedData);

                //DistributeAccountValues(content, user);
                decryptedContent = DistributeAPIValue(decryptedContent);
                //GD.Print(user);
                return decryptedContent;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Failed to delete account file: {ex.Message}");
            }
        }
        return null;
    }
    public static string DistributeAPIValue(string content)
    {
        List<List<Tokens>> tokens = new List<List<Tokens>>();
        string pattern = @"\b(API_Key:)\b|""[^""]*""|'[^']*'|\b[\w']+\b|\S";

        string[] lines = content.Split('\n');

        foreach (string line in lines)
        {
            if (!line.Trim().StartsWith("-"))
            {
                tokens.Add(TokenizeAccountLines(line.Replace(",", ""), pattern));
            }
        }
        for (int i = 0; i < tokens.Count; i++)
        {
            if (tokens[i].Count > 0)
            {
                content += SetAPIValues(tokens[i]);
            }
        }

        return content;
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
                else if (Regex.IsMatch(token, @"\b(API_Key)\b"))
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
    private static string SetAPIValues(List<Tokens> tokens)
    {
        switch (tokens[0].value)
        {
            case "API_Key":
                if (2 < tokens.Count)
                {
                    string api_key = "";
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        api_key += tokens[i].value;
                    }
                    return api_key;
                }
                break;
            default:
                GD.PrintErr("Label " + tokens[0].value + " does not exist!");
                break;
        }
        return null;
    }
}
