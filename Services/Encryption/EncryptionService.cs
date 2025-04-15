using CaseAPI.Abstractions.Encryption;
using CaseAPI.Options.Encryption;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace CaseAPI.Services.Encryption;

public sealed class EncryptionService(IOptions<EncryptionOptions> options) : IEncryptionService
{
    private readonly byte[] _key = Encoding.UTF8.GetBytes(options.Value.SecretKey);

    public string Encrypt(string plainText)
    {
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

        byte[] encryptedBytes;

        using (Aes aes = Aes.Create())
        {
            aes.Key = _key;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;

            using ICryptoTransform encryptor = aes.CreateEncryptor();
            encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }

        return Convert.ToBase64String(encryptedBytes);
    }

    public string Decrypt(string cipherText)
    {
        byte[] cipherBytes = Convert.FromBase64String(cipherText);

        byte[] decryptedBytes;

        using (Aes aes = Aes.Create())
        {
            aes.Key = _key;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;

            using ICryptoTransform decryptor = aes.CreateDecryptor();
            decryptedBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
        }

        return Encoding.UTF8.GetString(decryptedBytes);
    }

}
