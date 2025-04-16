namespace CaseAPI.Entities;

public sealed class Account
{
    public Account()
    {
        Transactions = new HashSet<AccountTransaction>();

    }

    public Guid Id { get; set; }
    public string? Code { get; set; }
    public ICollection<AccountTransaction> Transactions { get; set; }
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    public decimal Balance { get; set; }

    public byte[] RowVersion { get; set; } = [];
}
