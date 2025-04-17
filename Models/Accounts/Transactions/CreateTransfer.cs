namespace CaseAPI.Models.Accounts.Transactions;

public sealed record CreateTransfer
{
    public string? FromAccountCode { get; init; }
    public string? ToAccountCode { get; init; }
    public string? Channel { get; init; }
    public decimal Amount { get; init; }
}
