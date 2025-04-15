using CaseAPI.Entities.Enums;

namespace CaseAPI.Entities;

public sealed class AccountTransaction
{
    public Guid Id { get; set; }
    public Account? Account { get; set; }
    public Guid AccountId { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public TransactionChannel Channel { get; set; }
    public decimal UpdatedBalance { get; set; }
    public decimal Balance { get; set; }
}
