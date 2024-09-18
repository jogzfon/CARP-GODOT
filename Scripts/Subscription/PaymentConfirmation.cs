using Godot;
using System;

public partial class PaymentConfirmation : Node
{
    [Export] private Button _payment_receiptBtn;

    [Export] private FileDialog _receipt_dialog;

    [Export] private NotificationHandler _notification;
    
    Texture2D _receipt = null;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _payment_receiptBtn.Connect("pressed", new Callable(this, nameof(OpenReceiptDialogue)));

        _receipt_dialog.Connect("file_selected", new Callable(this, nameof(Receipt)));
    }

	private void OpenReceiptDialogue()
    {
        _receipt_dialog.Visible = true;
    }
    private void Receipt(string path)
    {
        _receipt = (Texture2D)GD.Load(path);

        if (_receipt != null)
        {
            _notification.MessageBox("Receipt loaded successfully", 0);
        }
        else
        {
            _notification.MessageBox("Failed to load receipt. Try again", 1);
        }
        _payment_receiptBtn.Icon = _receipt;
    }
    public Texture2D GetReceipt()
    {
        return _receipt;
    }
    public void ResetReceipt()
    {
        _payment_receiptBtn.Icon = null;
        _receipt = null;
    }
}
