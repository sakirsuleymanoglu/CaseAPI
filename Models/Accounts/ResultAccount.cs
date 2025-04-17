namespace CaseAPI.Models.Accounts;

public sealed record ResultAccount
{
    public Guid Id { get; init; }
    public string? Title { get; init; }
    public string? Code { get; init; }
    public decimal Balance { get; init; }
}
