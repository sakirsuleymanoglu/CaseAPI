namespace CaseAPI.Hubs.Models;

public sealed class NewTransferMessage
{
    public string? SenderUserName { get; set; }
    public decimal Amount { get; set; }
    public string? SenderAccountCode { get; set; }
}
