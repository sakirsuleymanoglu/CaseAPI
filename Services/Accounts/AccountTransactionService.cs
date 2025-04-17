using CaseAPI.Abstractions.Accounts;
using CaseAPI.Abstractions.Hubs;
using CaseAPI.Data;
using CaseAPI.Models.Accounts.Transactions;
using CaseAPI.Services.Accounts.Transactions;

namespace CaseAPI.Services.Accounts;

public sealed class AccountTransactionService(ApplicationDbContext context,
    ITransferHubService transferHubService) : IAccountTransactionService
{
    public async Task CreateEachOtherDepositAsync(CreateEachOtherDeposit model) => await new EachOtherDeposit(model, context).OperationAsync();
    public async Task CreateDepositAsync(CreateDeposit model) => await new Deposit(model, context).OperationAsync();
    public async Task CreateWithdrawalAsync(CreateWithdrawal model) => await new Withdrawal(model, context).OperationAsync();
    public async Task CreateTransferAsync(CreateTransfer model) => await new Transfer(model, context, transferHubService).OperationAsync();
}
