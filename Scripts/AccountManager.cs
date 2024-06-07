using Godot;
using System;

public static class AccountManager
{
    private static UserData user = null;

    public static void SetUser(UserData userIn)
    {
        user = userIn;
    }
    public static UserData GetUser()
    {
        return user;
    }
    public static String GetRole()
    {
        return user.Role;
    }
}
