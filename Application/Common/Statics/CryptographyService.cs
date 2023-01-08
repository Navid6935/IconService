using System.Security.Cryptography;
using System.Text;

namespace Application.Common.Statics;
public static class CryptographyService
{
    static readonly string dbkey = "SgVkYp3s6v9y$B&E)H@McQfThWmZq4t7";
    public static string? EncryptString(string data)
    {

        try
        {
            using var rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC; //remember this parameter
            rijndaelCipher.Padding = PaddingMode.PKCS7; //remember this parameter

            rijndaelCipher.KeySize = 0x80;
            rijndaelCipher.BlockSize = 0x80;
            var pwdBytes = Encoding.UTF8.GetBytes(dbkey);
            var keyBytes = new byte[0x10];
            var len = pwdBytes.Length;
            if (len > keyBytes.Length)
            {
                len = keyBytes.Length;
            }
            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;
            using var transform = rijndaelCipher.CreateEncryptor();
            var plainText = Encoding.UTF8.GetBytes(data);
            return Convert.ToBase64String
            (transform.TransformFinalBlock(plainText, 0, plainText.Length));
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static string? DecryptString(string data)
    {
        try
        {
            using var rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;

            rijndaelCipher.KeySize = 0x80;
            rijndaelCipher.BlockSize = 0x80;
            var encryptedData = Convert.FromBase64String(data);
            var pwdBytes = Encoding.UTF8.GetBytes(dbkey);
            var keyBytes = new byte[0x10];
            var len = pwdBytes.Length;

            if (len > keyBytes.Length)
            {
                len = keyBytes.Length;
            }

            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;
            var plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock
                        (encryptedData, 0, encryptedData.Length);

            return Encoding.UTF8.GetString(plainText);
        }
        catch (Exception)
        {
            return null;
        }
    }
}
