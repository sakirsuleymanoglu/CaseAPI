using CaseAPI.Entities.Enums;

namespace CaseAPI.Abstractions.Accounts.Transactions;

public interface ITransaction
{
    public TransactionType Type { get; }
    public TransactionChannel Channel { get; }
    Task OperationAsync();
}

