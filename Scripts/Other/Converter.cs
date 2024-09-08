using Godot;
using System;

public static class Converter
{
    public static string TextureToBase64(Texture2D texture)
    {
        Image img = texture.GetImage();
        byte[] byteArray = img.SavePngToBuffer();
        return Convert.ToBase64String(byteArray);
    }

    // Convert Base64 string back to Texture2D
    public static Texture2D Base64ToTexture(string base64String)
    {
        byte[] byteArray = Convert.FromBase64String(base64String);
        Image img = new Image();
        img.LoadPngFromBuffer(byteArray);

        ImageTexture texture = ImageTexture.CreateFromImage(img);
        return texture;
    }

    public static int BinaryStringToInt(string binaryString)
    {
        // Remove spaces
        string cleanedBinaryString = binaryString.Replace(" ", "");

        // Convert binary string to integer
        int result = Convert.ToInt32(cleanedBinaryString, 2);

        return result;
    }
}
