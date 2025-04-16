using CaseAPI.Abstractions.Accounts.Transactions;
using CaseAPI.Data;
using CaseAPI.Entities;
using CaseAPI.Entities.Enums;
using CaseAPI.Models.Accounts.Transactions;
using Microsoft.EntityFrameworkCore;

namespace CaseAPI.Services.Accounts.Transactions;

public sealed class Withdrawal(CreateWithdrawal model, ApplicationDbContext context) : TransactionBase(TransactionChannel.ATM.ToString()), ITransaction
{
    public TransactionType Type { get; } = TransactionType.Withdrawal;

    public async Task OperationAsync()
    {
        Account? account = await context.Accounts.SingleOrDefaultAsync(x => x.Code == model.AccountCode);

        if (account == null)
            throw new Exception();

        decimal balance = account.Balance;

        if (model.Amount > account.Balance)
            throw new Exception("Insufficient funds");

        account.Balance -= model.Amount;

        AccountTransaction transaction = new()
        {
            Id = Guid.NewGuid(),
            Amount = model.Amount,
            Channel = Channel,
            Account = account,
            Type = Type,
            Description = Type.ToString(),
            CreatedDate = DateTime.Now,
            Balance = balance,
            UpdatedBalance = account.Balance
        };

        await context.AccountTransactions.AddAsync(transaction);

        await context.SaveChangesAsync();
    }
}
