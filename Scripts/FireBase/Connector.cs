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
}
