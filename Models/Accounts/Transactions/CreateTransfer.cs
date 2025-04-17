namespace CaseAPI.Models.Accounts.Transactions;

public class CreateTransfer
{
    public string? FromAccountCode { get; set; }
    public string? ToAccountCode { get; set; }
    public string? Channel { get; set; }
    public decimal Amount { get; set; }
}
