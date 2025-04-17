namespace CaseAPI.Options.Encryption;

public sealed record EncryptionOptions
{
    public string? SecretKey { get; init; }
}
