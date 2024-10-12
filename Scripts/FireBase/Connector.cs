using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Godot;
using System;

public static class Connector
{
    private static IFirebaseConfig config = new FirebaseConfig
    {
        AuthSecret = "sl5J6RLP0fMsh6OJNNj978xIelPyaSCuwr6hOf8R",
        BasePath = "https://carp-70436-default-rtdb.asia-southeast1.firebasedatabase.app/",
    };

    private static IFirebaseClient client;
    
    public static IFirebaseClient ConnectToClient()
    {
        if(client == null)
        {
            client = new FireSharp.FirebaseClient(config);
        }
        return client;
    }

    public async static void UpdateSubscription()
    {
        try
        {
            FirebaseResponse updateResponse = await client.UpdateAsync("Users/" + AccountManager.GetUser().Username, new { Subscription = "Subscribed", SubscriptionStart = DateTime.Now.ToString(), SubscriptionEnd = DateTime.Now.AddDays(30).ToString() });

            if (updateResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                GD.Print("Request accepted successfully.");
                // Update the local user object to reflect the new status
            }
            else
            {
                GD.Print("Failed to accept request.");
            }
        }
        catch (Exception ex)
        {
            GD.Print($"Error updating Firebase: {ex.Message}");
        }
    }
}
