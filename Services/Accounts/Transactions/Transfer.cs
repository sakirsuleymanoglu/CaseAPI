using CaseAPI.Abstractions.Accounts.Transactions;
using CaseAPI.Abstractions.Hubs;
using CaseAPI.Abstractions.Users;
using CaseAPI.Data;
using CaseAPI.Entities;
using CaseAPI.Entities.Enums;
using CaseAPI.Exceptions.Transactions;
using CaseAPI.Models.Accounts.Transactions;
using Microsoft.EntityFrameworkCore;

namespace CaseAPI.Services.Accounts.Transactions;

public sealed class Transfer(
    CreateTransfer model,
    ApplicationDbContext context,
    ITransferHubService transferHubService,
    IUserService userService) : TransactionBase(model.Channel), ITransaction
{
    public TransactionType Type { get; } = TransactionType.Transfer;

    public async Task OperationAsync()
    {
        Guid userId = userService.GetAuthenticatedUserId();

        bool userHasFromAccount = await context.Accounts.AnyAsync(x => x.Code == model.FromAccountCode && x.AppUserId == userId);

        if (!userHasFromAccount)
            throw new UserHasFromAccountException();

        Account? fromAccount = await context.Accounts
            .Include(x => x.AppUser)
            .SingleOrDefaultAsync(x => x.Code == model.FromAccountCode) ?? throw new AccountNotFoundException();

        Account? toAccount = await context.Accounts.SingleOrDefaultAsync(x => x.Code == model.ToAccountCode) ?? throw new AccountNotFoundException();

        if (fromAccount.Balance < model.Amount)
            throw new InsufficientBalanceException();

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

        if (toAccount.AppUserId != userId)
            await transferHubService.SendNewTransferMessageAsync(toAccount.AppUserId, new()
            {
                Amount = model.Amount,
                ToAccountCode = model.ToAccountCode,
                SenderUserName = fromAccount.AppUser?.UserName,
            });
    }
}
