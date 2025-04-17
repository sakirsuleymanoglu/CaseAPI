using CaseAPI.Abstractions.Accounts.Transactions;
using CaseAPI.Data;
using CaseAPI.Entities.Enums;
using CaseAPI.Models.Accounts.Transactions;

namespace CaseAPI.Services.Accounts.Transactions;

public sealed class EachOtherDeposit(CreateEachOtherDeposit model, ApplicationDbContext context) : TransactionBase(TransactionChannel.ATM.ToString()), ITransaction
{
    public TransactionType Type { get; } = TransactionType.Deposit;

    public async Task OperationAsync()
    {
        //EachOtherDeposit operations...
    }
}
