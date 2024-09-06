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

    [Export]
    Panel profilePanel;

    [Export] PackedScene logInPage;

    [Export] private Control documentationAdder;

    [Export]
	Texture2D freeTier;
    [Export]
    Texture2D guestTier;
    [Export]
    Texture2D studentTier;
    [Export]
    Texture2D teacherTier;

    private bool isMouseOverPanel = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		subscriptionBtn.TextureNormal = freeTier;
        subscriptionPanel.Visible = false;

        subscriptionBtn.Connect("pressed", new Callable(this, nameof(OpenSubscription)));

        // Connect the mouse_exited signal to close the subscription panel when the mouse leaves it
        subscriptionPanel.Connect("mouse_exited", new Callable(this, nameof(OnMouseExitedPanel)));
        subscriptionPanel.Connect("mouse_entered", new Callable(this, nameof(OnMouseEnteredPanel)));

        // Connect the mouse_exited and mouse_entered signals for the buttons inside the panel
        //guestTierBtn.Connect("mouse_exited", new Callable(this, nameof(OnMouseExitedPanel)));
        guestTierBtn.Connect("mouse_entered", new Callable(this, nameof(OnMouseEnteredPanel)));
        // studentTierBtn.Connect("mouse_exited", new Callable(this, nameof(OnMouseExitedPanel)));
        studentTierBtn.Connect("mouse_entered", new Callable(this, nameof(OnMouseEnteredPanel)));
        // teacherTierBtn.Connect("mouse_exited", new Callable(this, nameof(OnMouseExitedPanel)));
        teacherTierBtn.Connect("mouse_entered", new Callable(this, nameof(OnMouseEnteredPanel)));
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
        if (documentationAdder != null)
        {
            documentationAdder.Visible = false;
        }

        if (AccountManager.GetUser() != null)
        {
            profilePanel.Visible = false;
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
    private void OnMouseEnteredPanel()
    {
        isMouseOverPanel = true;
    }

    private void OnMouseExitedPanel()
    {
        isMouseOverPanel = false;
        // Defer closing to the next frame to ensure other signals don't conflict
        CallDeferred(nameof(CheckMouseOver));
    }

    private void CheckMouseOver()
    {
        if (!isMouseOverPanel && subscriptionPanel.Visible)
        {
            subscriptionPanel.Visible = false;
        }
    }
}
