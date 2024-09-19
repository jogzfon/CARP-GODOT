using Godot;
using System;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

public static class SubscriptionRequestHandler
{
	/*private static IFirebaseConfig config = new FirebaseConfig
    {
        AuthSecret = "W5nGKOER85yiOo7BZqptL2nKmWNRcng0Mclftbg5",
        BasePath = "https://carp-70436-default-rtdb.asia-southeast1.firebasedatabase.app/",
    };*/

    private static IFirebaseClient client;
    private static bool TryConnectToDatabase()
    {
        try
        {
            //client = new FireSharp.FirebaseClient(config);
            client = Connector.ConnectToClient();
        }
        catch (Exception ex)
        {
            GD.Print($"Connection failed: {ex.Message}");
            return false;
        }
        return true;
    }
    public static bool RequestSubscription(Texture2D receipt)
    {
        TryConnectToDatabase();
        if (client == null)
        {
            GD.Print("Database connection is not established.");
            return false;
        }
        Send_Request(receipt);
        return true;
    }
    private static async void Send_Request(Texture2D receipt)
    {
        try
        {
            FirebaseResponse updateResponse = await client.UpdateAsync("Users/" + AccountManager.GetUser().Username, new { Subscription = "Endorsed", ProofOfPayment = Converter.TextureToBase64(receipt) });

            if (updateResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                GD.Print("Request sent successfully.");
                AccountManager.GetUser().Subscription = "Endorsed";
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
