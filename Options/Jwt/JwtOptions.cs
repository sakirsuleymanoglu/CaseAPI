namespace CaseAPI.Options.Jwt;

public sealed record JwtOptions
{
    public string? SecurityKey { get; init; }
    public string? Issuer { get; init; }
    public string? Audience { get; init; }
    public int ExpireInMinutes { get; set; }
}
