using Godot;
using System;
using System.Collections.Generic;

public partial class SubscriptionHandler : Node
{
	[Export]
	TextureButton subscriptionBtn;
    [Export]
    TextureButton guestTierBtn;
    [Export]
    TextureButton studentTierBtn;
    [Export]
    TextureButton teacherTierBtn;

    [Export]
    ColorRect subscriptionPanel;

    [Export] PackedScene logInPage;

    [Export]
	Texture2D freeTier;
    [Export]
    Texture2D guestTier;
    [Export]
    Texture2D studentTier;
    [Export]
    Texture2D teacherTier;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		subscriptionBtn.TextureNormal = freeTier;
        subscriptionPanel.Visible = false;

        subscriptionBtn.Connect("pressed", new Callable(this, nameof(OpenSubscription)));
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (AccountManager.GetUser() != null)
        {
            if(AccountManager.GetUser().Subscription == "Student")
            {
                subscriptionBtn.TextureNormal = studentTier;
                studentTierBtn.Modulate = new Color("5d5d5d");
                teacherTierBtn.Modulate = new Color("ffffff");
                guestTierBtn.Modulate = new Color("ffffff");

                studentTierBtn.Disabled = true;
                teacherTierBtn.Disabled = false;
                guestTierBtn.Disabled = false;
            }
            else if(AccountManager.GetUser().Subscription == "Teacher")
            {
                subscriptionBtn.TextureNormal = teacherTier;
                teacherTierBtn.Modulate = new Color("5d5d5d");
                studentTierBtn.Modulate = new Color("ffffff");
                guestTierBtn.Modulate = new Color("ffffff");

                studentTierBtn.Disabled = false;
                teacherTierBtn.Disabled = true;
                guestTierBtn.Disabled = false;
            }
            else
            {
                subscriptionBtn.TextureNormal = guestTier;
                guestTierBtn.Modulate = new Color("5d5d5d");
                studentTierBtn.Modulate = new Color("ffffff");
                teacherTierBtn.Modulate = new Color("ffffff");

                studentTierBtn.Disabled = false;
                teacherTierBtn.Disabled = false;
                guestTierBtn.Disabled = true;
            }
        }
        else
        {
            subscriptionBtn.TextureNormal = freeTier;
        }
    }

    private void OpenSubscription()
    {
        if (AccountManager.GetUser() != null)
        {
            if (subscriptionPanel.Visible)
                subscriptionPanel.Visible = false;
            else
                subscriptionPanel.Visible = true;
        }
        else
        {
            Node simultaneous = logInPage.Instantiate();
            GetTree().Root.AddChild(simultaneous);
        }
    }
}
