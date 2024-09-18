using Godot;
using System;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection.Metadata;

public static class AccountFileSaver
{
    static string directoryLoc = "AccountData";
    static string fileName = "accountData.carpacc";

    public static void SaveAccount(UserData user)
    {
        // Create the account template string
        string accountTemplate =
            "Username: " +user.Username+ "\n" +
            "Password: " +user.Password+ "\n" +
            "Firstname: " + user.Firstname + "\n" +
            "Lastname: " + user.Lastname + "\n" +   // Fixed to show Lastname properly
            "Email: " + user.Email + "\n"+
            "Status: "+ user.Status +"\n" +
            "Subscription: " + user.Subscription + "\n" +
            "Role: " + user.Role + "\n" +
            "SubscriptionStart: " + user.SubscriptionStart + "\n" +
            "SubscriptionEnd: " + user.SubscriptionEnd + "\n" +
            "ProfileImage: " + user.ProfileImage + "\n";

        byte[] encryptedData = InfoSecure.EncryptData(accountTemplate);

        DirAccess dir = DirAccess.Open(directoryLoc);
        if (dir == null)
        {
            dir.MakeDir(directoryLoc);
            dir = DirAccess.Open(directoryLoc);

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

    public static void DeleteAccountFile() {
        string filePath = Path.Combine(directoryLoc, fileName);

        if (Godot.FileAccess.FileExists(filePath))
        {
            try
            {
                DirAccess.RemoveAbsolute(filePath);
                //GD.Print($"Account file '{fileName}' successfully deleted.");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Failed to delete account file: {ex.Message}");
            }
        }
        else
        {
            GD.PrintErr($"Account file '{fileName}' not found.");
        }
    }

    public static void GetAccountFile()
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

                UserData user = new UserData();
                //DistributeAccountValues(content, user);
                DistributeAccountValues(decryptedContent, user);
                //GD.Print(user);
                AccountManager.SetUser(user);
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Failed to delete account file: {ex.Message}");
            }
        }
    }
    private static void DistributeAccountValues(string content, UserData user)
    {
        List<List<Tokens>> tokens = new List<List<Tokens>>();

        string pattern = @"\b(Username:|Password:|Firstname:|Lastname:|Email:|Status:|Subscription:|Role:|SubscriptionStart:|SubscriptionEnd:|ProfileImage:)\b|""[^""]*""|'[^']*'|\b[\w']+\b|\S";

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
                SetAccountValues(tokens[i], user);
            }
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
                else if (Regex.IsMatch(token, @"\b(Username|Password|Firstname|Lastname|Email|Status|Subscription|Role|SubscriptionStart|SubscriptionEnd|ProfileImage)\b"))
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

    private static void SetAccountValues(List<Tokens> tokens, UserData user)
    {
        switch (tokens[0].value)
        {
            case "Username":
                if (2 < tokens.Count)
                {
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        user.Username = tokens[i].value;
                    }
                }
                break;
            case "Password":
                if (2 < tokens.Count)
                {
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        user.Password += tokens[i].value;
                    }
                }
                break;
            case "Firstname":
                if (2 < tokens.Count)
                {
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        user.Firstname += tokens[i].value + " ";
                    }
                }
                break;
            case "Lastname":
                if (2 < tokens.Count)
                {
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        user.Lastname += tokens[i].value + " ";
                    }
                }
                break;
            case "Email":
                if (2 < tokens.Count)
                {
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        user.Email += tokens[i].value;
                    }
                }
                break;
            case "Status":
                if (2 < tokens.Count)
                {
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        user.Status += tokens[i].value + " ";
                    }
                }
                break;
            case "Subscription":
                if (2 < tokens.Count)
                {
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        user.Subscription += tokens[i].value + " ";
                    }
                }
                break;
            case "Role":
                if (2 < tokens.Count)
                {
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        user.Role += tokens[i].value;
                    }
                }
                break;
            case "SubscriptionStart":
                if (2 < tokens.Count)
                {
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        user.SubscriptionStart += tokens[i].value;
                    }
                }
                break;
            case "SubscriptionEnd":
                if (2 < tokens.Count)
                {
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        user.SubscriptionEnd += tokens[i].value;
                    }
                }
                break;
            case "ProfileImage":
                if (2 < tokens.Count)
                {
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        user.ProfileImage += tokens[i].value;
                    }
                }
                break;
            default:
                GD.PrintErr("Label " + tokens[0].value + " does not exist!");
                break;
        }

    }
}
