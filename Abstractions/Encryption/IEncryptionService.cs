namespace CaseAPI.Abstractions.Encryption;

public interface IEncryptionService
{
    string? Encrypt(string plainText);
    string? Decrypt(string cipherText);
}
