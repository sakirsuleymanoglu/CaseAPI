using CaseAPI.Abstractions.Accounts;
using CaseAPI.Abstractions.Hubs;
using CaseAPI.Abstractions.Users;
using CaseAPI.Data;
using CaseAPI.Models.Accounts.Transactions;
using CaseAPI.Services.Accounts.Transactions;
using Microsoft.EntityFrameworkCore;

namespace CaseAPI.Services.Accounts;

public sealed class AccountTransactionService(
    ApplicationDbContext context,
    ITransferHubService transferHubService,
    IUserService userService) : IAccountTransactionService
{
    public async Task CreateTransferAsync(CreateTransfer model) => await new Transfer(model, context, transferHubService, userService).OperationAsync();

    public async Task<List<ResultTransaction>> GetAllAsync(string accountId) => await context.AccountTransactions
            .AsNoTracking()
            .Where(x => x.AccountId == Guid.Parse(accountId))
            .Select(x => new ResultTransaction
            {
                Id = x.Id,
                Channel = x.Channel.ToString(),
                Type = x.Type.ToString(),
                Balance = x.Balance,
                UpdatedBalance = x.UpdatedBalance,
                Amount = x.Amount,
                Description = x.Description,
                CreatedDate = x.CreatedDate,
            }).OrderByDescending(x => x.CreatedDate).ToListAsync();
}
