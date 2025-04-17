namespace CaseAPI.Models.Accounts.Transactions;

public sealed record ResultTransaction
{
    public Guid Id { get; init; }
    public string? Channel { get; init; }
    public string? Type { get; init; }
    public decimal Amount { get; init; }
    public decimal Balance { get; init; }
    public decimal UpdatedBalance { get; init; }
    public string? Description { get; init; }
    public DateTime CreatedDate { get; init; }
}
