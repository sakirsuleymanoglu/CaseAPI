namespace CaseAPI.Models.Accounts.Transactions;

public class CreateEachOtherDeposit
{
    public string? FromAccountCode { get; set; }
    public string? ToAccountCode { get; set; }
    public decimal Amount { get; set; }
}
