using Godot;
using System;
using Paymongo.Sharp;

public partial class PaymongoPayment : Node
{
    PaymongoClient client;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        client = new PaymongoClient(secretKey: "sk_test_Gf1MgmxD7g2c96jTG7vvByk1");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
