namespace CaseAPI.Models.Accounts.Transactions;

public class CreateDeposit
{
    public string? AccountCode { get; set; }
    public decimal Amount { get; set; }
}