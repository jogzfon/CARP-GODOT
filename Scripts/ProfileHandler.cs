using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Godot;
using System;

public partial class ProfileHandler : Node
{
    [Export]
    Button schoolBtn;
    [Export]
    MarginContainer centerMargin;
    [Export]
    TextureButton accountProfile;
    [Export]
    Panel profilePanel;
    [Export]
    Button exitProfile;
    [Export]
    FileDialog profileDialog;
    [Export]
    TextureButton changeProfileBtn;
    [Export]
    Texture2D defaultProfileTexture;
    [Export]
    Button saveProfileImage;
    [Export]
    Label userName;

    [Export]
    ColorRect subscriptionPanel;

    [Export]
    private NotificationHandler notification;


    IFirebaseConfig config = new FirebaseConfig
    {
        AuthSecret = "sl5J6RLP0fMsh6OJNNj978xIelPyaSCuwr6hOf8R",
        BasePath = "https://carp-70436-default-rtdb.asia-southeast1.firebasedatabase.app/",
    };

    IFirebaseClient client;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        client = new FireSharp.FirebaseClient(config);

        if (AccountManager.GetUser() != null)
        {
            if (AccountManager.schoolOn)
            {
                schoolBtn.Visible = true;
                centerMargin.Visible = true;
                schoolBtn.Connect("pressed", new Callable(this, nameof(SchoolPage)));
            }
            accountProfile.TextureNormal = Base64ToTexture(AccountManager.GetUser().ProfileImage);
            changeProfileBtn.TextureNormal = Base64ToTexture(AccountManager.GetUser().ProfileImage);
        }
        else
        {
            DisableProfileAccess();
        }

        client = new FireSharp.FirebaseClient(config);
        saveProfileImage.Connect("pressed", new Callable(this, nameof(SaveProfile)));
        accountProfile.Connect("pressed", new Callable(this, nameof(OpenProfilePage)));
        changeProfileBtn.Connect("pressed", new Callable(this, nameof(OpenChangeProfile)));
        profileDialog.Connect("file_selected", new Callable(this, nameof(ChangeProfile)));
        exitProfile.Connect("pressed", new Callable(this, nameof(CloseProfilePage)));
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (AccountManager.GetUser() != null)
        {
            schoolBtn.Disabled = false;
            accountProfile.Disabled = false;
            if(AccountManager.GetUser().ProfileImage != "NONE")
            {
                accountProfile.TextureNormal = Base64ToTexture(AccountManager.GetUser().ProfileImage);
            }
            userName.Text = AccountManager.GetUser().Firstname +" "+ AccountManager.GetUser().Lastname;
        }
        else
        {
            accountProfile.TextureNormal = defaultProfileTexture;
            changeProfileBtn.TextureNormal = defaultProfileTexture;
        }
	}
    private void SchoolPage()
    {

    }
    private void DisableProfileAccess()
    {
        accountProfile.Disabled = true;
        profileDialog.Visible = false;
        profilePanel.Visible = false;
        schoolBtn.Visible = false;
        centerMargin.Visible = false;
        accountProfile.TextureNormal = defaultProfileTexture;
        changeProfileBtn.TextureNormal = defaultProfileTexture;
        changeProfileBtn.Disabled = true;
    }
    private void OpenProfilePage()
    {
        if (AccountManager.GetUser() != null)
        {
            profilePanel.Visible = true;
            subscriptionPanel.Visible = false;
            if (AccountManager.GetUser().ProfileImage != "NONE")
            {
                changeProfileBtn.TextureNormal = Base64ToTexture(AccountManager.GetUser().ProfileImage);
            }
        }
        else
        {
            GD.Print("No user logged in.");
        }
    }
    private void CloseProfilePage()
    {
        profilePanel.Visible = false;
    }
    private void OpenChangeProfile()
    {
        profileDialog.Visible = true;
    }
    private void ChangeProfile(string path)
    {
        Texture2D texture = (Texture2D)GD.Load(path);

        if (texture != null)
        {
            notification.MessageBox("Texture loaded successfully", 0);
        }
        else
        {
            notification.MessageBox("Failed to load texture", 1);
        }
        changeProfileBtn.TextureNormal = texture;
    }
    private async void SaveProfile()
    {
        Texture2D textureToSave = changeProfileBtn.TextureNormal as Texture2D;
        string profileImageBase64 = TextureToBase64(textureToSave);

        var currentUser = AccountManager.GetUser();
        currentUser.ProfileImage = profileImageBase64;

        SetResponse response = await client.SetAsync("Users/" + currentUser.Username, currentUser);
        UserData result = response.ResultAs<UserData>();

        notification.MessageBox(result.Username+" Profile Updated", 0);

        AccountManager.SetUser(result);
    }
    public string TextureToBase64(Texture2D texture)
    {
        Image img = texture.GetImage();
        byte[] byteArray = img.SavePngToBuffer();
        return Convert.ToBase64String(byteArray);
    }

    // Convert Base64 string back to Texture2D
    public Texture2D Base64ToTexture(string base64String)
    {
        byte[] byteArray = Convert.FromBase64String(base64String);
        Image img = new Image();
        img.LoadPngFromBuffer(byteArray);

        ImageTexture texture = ImageTexture.CreateFromImage(img);
        return texture;
    }
}
