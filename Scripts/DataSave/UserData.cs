using Godot;
using System;

public class UserData
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public string Status { get; set; }
    public string Subscription { get; set; }
    public string Role { get; set; }
    public string SubscriptionStart { get; set; }
    public string SubscriptionEnd { get; set; }
    public string ProfileImage { get; set; }

    // ToString method to display user data
    public override string ToString()
    {
        return $"Username: {Username}\n" +
               $"Password: {new string('*', Password.Length)}\n" +  // Mask the password
               $"Firstname: {Firstname}\n" +
               $"Lastname: {Lastname}\n" +
               $"Email: {Email}\n" +
               $"Status: {Status}\n" +
               $"Subscription: {Subscription}\n" +
               $"Role: {Role}\n" +
               $"Subscription Start: {SubscriptionStart}\n" +
               $"Subscription End: {SubscriptionEnd}";
    }
}
