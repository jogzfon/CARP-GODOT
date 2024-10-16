using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        // Check if the string contains any characters outside of '0' and '1'
        if (cleanedBinaryString.Any(c => !"01".Contains(c)))
        {
            // Treat the string as hexadecimal
            int result = Convert.ToInt32(cleanedBinaryString, 16);

            // Convert the integer back to a binary string
            cleanedBinaryString = Convert.ToString(result, 2);
        }

        // Convert binary string to integer
        int binaryResult = Convert.ToInt32(cleanedBinaryString, 2);

        return binaryResult;
    }

    public static Task<string> TextureToBase64Async(Texture2D texture)
    {
        return Task.Run(() => TextureToBase64(texture));
    }

    public static Task<Texture2D> Base64ToTextureAsync(string base64String)
    {
        return Task.Run(() => Base64ToTexture(base64String));
    }

}
