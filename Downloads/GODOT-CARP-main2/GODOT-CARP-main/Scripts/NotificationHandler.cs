using Godot;
using System;

public partial class NotificationHandler : Control
{
    //NotificationWindow
    [Export] private Window notificationWindow;
    [Export] private Button closeNotification;
    [Export] private Label message;
    
	// Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        notificationWindow.Visible = false;
        closeNotification.Connect("pressed", new Callable(this, nameof(CloseNotification)));
    }

    public void CloseNotification()
    {
        notificationWindow.Visible = false;
    }

    public void MessageBox(string value, int color)
    {
        Color red = Colors.Red;
        Color white = Colors.White;
        notificationWindow.Visible = true;
        notificationWindow.Popup();
        if (color == 0)
        {
            message.Text = value;
            message.AddThemeColorOverride("font_color", white);
        }
        else
        {
            message.Text = value;
            message.AddThemeColorOverride("font_color", red);
        }
    }
}
