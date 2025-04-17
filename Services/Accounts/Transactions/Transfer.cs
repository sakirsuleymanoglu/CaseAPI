using CaseAPI.Abstractions.Accounts.Transactions;
using CaseAPI.Abstractions.Hubs;
using CaseAPI.Data;
using CaseAPI.Entities;
using CaseAPI.Entities.Enums;
using CaseAPI.Models.Accounts.Transactions;
using Microsoft.EntityFrameworkCore;

namespace CaseAPI.Services.Accounts.Transactions;

public sealed class Transfer(CreateTransfer model, ApplicationDbContext context,
     ITransferHubService transferHubService) : TransactionBase(model.Channel), ITransaction
{
    public TransactionType Type { get; } = TransactionType.Transfer;

    public async Task OperationAsync()
    {
        Account? fromAccount = await context.Accounts
            .Include(x => x.AppUser)
            .SingleOrDefaultAsync(x => x.Code == model.FromAccountCode);

        if (fromAccount == null)
            throw new Exception();

        Account? toAccount = await context.Accounts.SingleOrDefaultAsync(x => x.Code == model.ToAccountCode);

        if (toAccount == null)
            throw new Exception();

        if (fromAccount.Balance < model.Amount)
            throw new Exception("Insufficient balance");

        decimal toAccountBalance = toAccount.Balance;
        decimal fromAccountBalance = fromAccount.Balance;

        toAccount.Balance += model.Amount;
        fromAccount.Balance -= model.Amount;

        DateTime createdDate = DateTime.Now;

        AccountTransaction fromAccountTransaction = new()
        {
            Id = Guid.NewGuid(),
            Account = fromAccount,
            Amount = model.Amount,
            Channel = Channel,
            Type = Type,
            Description = Type.ToString(),
            Balance = fromAccountBalance,
            CreatedDate = createdDate,
            UpdatedBalance = fromAccount.Balance
        };

        AccountTransaction toAccountTransaction = new()
        {
            Id = Guid.NewGuid(),
            Account = toAccount,
            Amount = model.Amount,
            Channel = Channel,
            Type = Type,
            Balance = toAccountBalance,
            Description = Type.ToString(),
            CreatedDate = createdDate,
            UpdatedBalance = toAccount.Balance
        };

        await context.AccountTransactions.AddAsync(fromAccountTransaction);
        await context.AccountTransactions.AddAsync(toAccountTransaction);

        await context.SaveChangesAsync();

        await transferHubService.SendNewTransferMessageAsync(toAccount.AppUserId, new()
        {
            Amount = model.Amount,
            SenderAccountCode = model.FromAccountCode,
            SenderUserName = fromAccount.AppUser?.UserName,
        });
    }
}
