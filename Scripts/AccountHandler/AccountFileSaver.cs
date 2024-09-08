using Godot;
using System;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text;

public static class AccountFileSaver
{
    public static void SaveAccount(UserData user)
    {
        // Create the account template string
        string accountTemplate =
            "Firstname: " + user.Firstname + "\n" +
            "Lastname: " + user.Lastname + "\n" +   // Fixed to show Lastname properly
            "Username: *********\n" +
            "Password: *********\n" +
            "Subscription: " + user.Subscription + "\n" +
            "Role: " + user.Role + "\n" +
            "ProfileImage: " + user.ProfileImage + "\n";

        string directoryLoc = "AccountData";
        string fileName = "accountData.carpacc";
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
}
