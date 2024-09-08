using Godot;
using System;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text;

public static class AccountFileSaver
{
    static string directoryLoc = "AccountData";
    static string fileName = "accountData.carpacc";
    public static void SaveAccount(UserData user)
    {
        // Create the account template string
        string accountTemplate =
            "Username: *********\n" +
            "Password: *********\n" +
            "Firstname: " + user.Firstname + "\n" +
            "Lastname: " + user.Lastname + "\n" +   // Fixed to show Lastname properly
            "Email: ************\n"+
            "Status: "+ user.Status +"\n" +
            "Subscription: " + user.Subscription + "\n" +
            "Role: " + user.Role + "\n" +
            "SubscriptionStart: " + user.SubscriptionStart + "\n" +
            "SubscriptionEnd: " + user.SubscriptionEnd + "\n" +
            "ProfileImage: " + user.ProfileImage + "\n";

        DirAccess dir = DirAccess.Open(directoryLoc);
        if (dir == null)
        {
            dir.MakeDir(directoryLoc);
            string filePath = Path.Combine(dir.GetCurrentDir(), fileName);

            using var file = Godot.FileAccess.Open(filePath, Godot.FileAccess.ModeFlags.Write);

            file.StoreString(accountTemplate);
            GD.PrintErr("Failed to open directory: " + directoryLoc);
            file.Close();
            return;
        }
        try
        {
            string filePath = Path.Combine(dir.GetCurrentDir(), fileName);

            using var file = Godot.FileAccess.Open(filePath, Godot.FileAccess.ModeFlags.Write);

            file.StoreString(accountTemplate);
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
                GD.Print($"Account file '{fileName}' successfully deleted.");
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

    }
}
