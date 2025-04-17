namespace CaseAPI.Models.Authentication;

public sealed record Login
{
    public string? UserName { get; init; }
    public string? Password { get; init; }
}
