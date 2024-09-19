using Godot;
using System;

public partial class AccountUpdater : Node
{
    // Timer to handle account updates
    private Timer updateTimer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Initialize the timer
        updateTimer = new Timer();
        updateTimer.WaitTime = 5.0f; // Set to 5 seconds, change to 10.0f for 10 seconds
        updateTimer.Autostart = true; // Start automatically when added to the scene
        updateTimer.OneShot = false; // Loop the timer
        updateTimer.Connect("timeout", new Callable(this, nameof(OnUpdateTimeout))); // Connect the timer to a method

        // Add the timer as a child node to the current node
        AddChild(updateTimer);
    }

    // This method will be called every time the timer times out
    private void OnUpdateTimeout()
    {
        // Call your account update logic here
        GD.Print("Account update triggered at: " + DateTime.Now);
        // Add your account update code here
    }
}
