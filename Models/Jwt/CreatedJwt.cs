namespace CaseAPI.Models.Jwt;

public sealed record CreatedJwt
{
    public string? Token { get; init; }
    public DateTime Expiration { get; init; }
}
