using Godot;
using System;
using System.Linq;
using Paymongo.Sharp;
using Paymongo.Sharp.Checkouts.Entities;
using Paymongo.Sharp.Core.Entities;
using Paymongo.Sharp.Core.Enums;
using Paymongo.Sharp.Links.Entities;
using Paymongo.Sharp.Sources.Entities;
using Paymongo.Sharp.Utilities;

public partial class PaymongoPayment : Node
{
    PaymongoClient client;

    [Export] private NotificationHandler _notificationHandler;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        client = new PaymongoClient(secretKey: "sk_test_Gf1MgmxD7g2c96jTG7vvByk1");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    private async void OnPayLink()
    {
        var secretKey = "sk_test_Gf1MgmxD7g2c96jTG7vvByk1";
        var client = new PaymongoClient(secretKey);

        Link link;

        var userAcc = AccountManager.GetUser();
        if(userAcc.Role == "Student")
        {
           link = new Link
            {
                Description = "Payment for Student Subscription",
                Amount = 100, //Set here the payment amount
                Currency = Currency.Php
            };
        }
        else if(userAcc.Role == "Teacher")
        {
            link = new Link
            {
                Description = "Payment for Teacher AI Subscription",
                Amount = 125,
                Currency = Currency.Php
            };
        }
        else
        {
            _notificationHandler.MessageBox("User unexistent", 1);
            
            return;
        }

        var linkResult = await client.Links.CreateLinkAsync(link);
        /*var paymentWindow = new PaymentWindow(linkResult.CheckoutUrl);

        paymentWindow.Show();*/

        while (true)
        {
            var paymentStatus = await client.Links.RetrieveLinkAsync(linkResult.Id);

            if (!paymentStatus.Payments!.Any())
            {
                continue;
            }

            var payment = paymentStatus.Payments!.First();

            if (payment.Status != PaymentStatus.Paid)
            {
                continue;
            }

            var status = $"Paid by {payment.Billing!.Name} on {payment.PaidAt} using {payment.Source!["type"]}";

            /*paymentWindow.Close();*/

            break;
        }

    }
/*
    private async void OnPayCheckout(object sender, RoutedEventArgs e)
    {
        var isDouble = decimal.TryParse(AmountTextBox.Text, out decimal doubleAmount);

        if (!isDouble)
        {
            return;
        }

        if (doubleAmount < 100)
        {
            return;
        }

        AmountTextBox.Text = string.Empty;
        StatusBlock.Text = string.Empty;

        var secretKey = Env.GetString("SECRET_KEY");
        var client = new PaymongoClient(secretKey);

        Checkout checkout = new Checkout()
        {
            Description = "Test Checkout",
            ReferenceNumber = "9282321A",
            LineItems = new[]
            {
                    new LineItem
                    {
                        Name = "Give You Up",
                        Images = new []
                        {
                            "https://i.insider.com/602ee9ced3ad27001837f2ac?width=750&format=jpeg"
                        },
                        Quantity = 1,
                        Currency = Currency.Php,
                        Amount = doubleAmount
                    }
                },
            PaymentMethodTypes = new[]
            {
                    PaymentMethod.GCash,
                    PaymentMethod.Card,
                    PaymentMethod.Paymaya,
                    PaymentMethod.BillEase,
                    PaymentMethod.Dob,
                    PaymentMethod.GrabPay,
                    PaymentMethod.DobUbp
                }
        };

        Checkout checkoutResult = await client.Checkouts.CreateCheckoutAsync(checkout);
        var paymentWindow = new PaymentWindow(checkoutResult.CheckoutUrl);

        paymentWindow.Show();

        while (true)
        {
            var paymentStatus = await client.Checkouts.RetrieveCheckoutAsync(checkoutResult.Id);

            if (!paymentStatus.Payments!.Any())
            {
                continue;
            }

            var payment = paymentStatus.Payments!.First();

            if (payment.Status != PaymentStatus.Paid)
            {
                continue;
            }

            StatusBlock.Text = $"Paid by {payment.Billing!.Name} on {payment.PaidAt} using {payment.Source!["type"]}";

            paymentWindow.Close();

            break;
        }

    }

    private async void OnPayGcash(object sender, RoutedEventArgs e)
    {
        var isDouble = decimal.TryParse(AmountTextBox.Text, out decimal doubleAmount);

        if (!isDouble)
        {
            return;
        }

        if (doubleAmount < 100)
        {
            return;
        }

        AmountTextBox.Text = string.Empty;
        StatusBlock.Text = string.Empty;

        var secretKey = Env.GetString("SECRET_KEY");
        var client = new PaymongoClient(secretKey);

        // Arrange
        Source source = new Source
        {
            Amount = doubleAmount,
            Description = "New Gcash Payment",
            Billing = new Billing
            {
                Name = "TestName",
                Email = "test@paymongo.com",
                Phone = "9063364572",
                Address = new Address
                {
                    Line1 = "TestAddress1",
                    Line2 = "TestAddress2",
                    PostalCode = "4506",
                    State = "TestState",
                    City = "TestCity",
                    Country = "PH"
                }
            },
            Redirect = new Redirect
            {
                Success = "http://127.0.0.1",
                Failed = "http://127.0.0.1"
            },
            Type = SourceType.GCash,
            Currency = Currency.Php
        };

        // Act
        var sourceResult = await client.Sources.CreateSourceAsync(source);
        var paymentWindow = new PaymentWindow(sourceResult.Redirect!.CheckoutUrl);

        paymentWindow.Show();

        while (true)
        {
            var paymentStatus = await client.Sources.RetrieveSourceAsync(sourceResult.Id);

            if (paymentStatus.Status != SourceStatus.Chargeable)
            {
                continue;
            }

            StatusBlock.Text = $"Chargeable on GCash by {paymentStatus.Billing.Name} on {paymentStatus.UpdatedAt}";

            paymentWindow.Close();

            break;
        }

    }

    private async void OnPayGrabPay(object sender, RoutedEventArgs e)
    {
        var isDouble = decimal.TryParse(AmountTextBox.Text, out decimal doubleAmount);

        if (!isDouble)
        {
            return;
        }

        if (doubleAmount < 100)
        {
            return;
        }

        AmountTextBox.Text = string.Empty;
        StatusBlock.Text = string.Empty;

        var secretKey = Env.GetString("SECRET_KEY");
        var client = new PaymongoClient(secretKey);

        // Arrange
        Source source = new Source
        {
            Amount = doubleAmount.ToIntAmount(),
            Description = "New GrabPay Payment",
            Billing = new Billing
            {
                Name = "TestName",
                Email = "test@paymongo.com",
                Phone = "9063364572",
                Address = new Address
                {
                    Line1 = "TestAddress1",
                    Line2 = "TestAddress2",
                    PostalCode = "4506",
                    State = "TestState",
                    City = "TestCity",
                    Country = "PH"
                }
            },
            Redirect = new Redirect
            {
                Success = "http://127.0.0.1",
                Failed = "http://127.0.0.1"
            },
            Type = SourceType.GrabPay,
            Currency = Currency.Php
        };

        // Act
        var sourceResult = await client.Sources.CreateSourceAsync(source);
        var paymentWindow = new PaymentWindow(sourceResult.Redirect!.CheckoutUrl);

        paymentWindow.Show();

        while (true)
        {
            var paymentStatus = await client.Sources.RetrieveSourceAsync(sourceResult.Id);

            if (paymentStatus.Status != SourceStatus.Chargeable)
            {
                continue;
            }

            StatusBlock.Text = $"Chargeable on GrabPay by {paymentStatus.Billing.Name} on {paymentStatus.UpdatedAt}";

            paymentWindow.Close();

            break;
        }

    }*/
}
