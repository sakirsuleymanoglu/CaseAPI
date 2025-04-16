namespace CaseAPI.Models.Accounts.Transactions;

public class CreateWithdrawal
{
    public string? AccountCode { get; set; }
    public decimal Amount { get; set; }
}