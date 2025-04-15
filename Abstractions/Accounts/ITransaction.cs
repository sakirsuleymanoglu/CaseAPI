using CaseAPI.Entities.Enums;

namespace CaseAPI.Abstractions.Accounts;

public interface ITransaction
{
    public TransactionType Type { get; }
    public TransactionChannel Channel { get; }
    Task OperationAsync();
}

