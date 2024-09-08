using Godot;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class InfoSecure
{
    private static byte[] password = Encoding.UTF8.GetBytes("SMSSheeeshTrioMaster");
    public static byte[] EncryptData(string data)
    {
        using Aes aes = Aes.Create();
        using var key = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes("Salt"), 10000);

        aes.Key = key.GetBytes(32); // AES-256 key size
        aes.IV = key.GetBytes(16);  // AES block size

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using var writer = new StreamWriter(cs);

        writer.Write(data);
        writer.Flush();
        cs.FlushFinalBlock();
        return ms.ToArray();
    }

    public static string DecryptData(byte[] encryptedData)
    {
        using Aes aes = Aes.Create();
        using var key = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes("Salt"), 10000);

        aes.Key = key.GetBytes(32); // AES-256 key size
        aes.IV = key.GetBytes(16);  // AES block size

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(encryptedData);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var reader = new StreamReader(cs);

        // Read the decrypted data as a string
        return reader.ReadToEnd();
    }
}
