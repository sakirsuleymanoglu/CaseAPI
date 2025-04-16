using CaseAPI.Abstractions.Accounts;
using CaseAPI.Data;
using CaseAPI.Models.Accounts.Transactions;
using CaseAPI.Services.Accounts.Transactions;

namespace CaseAPI.Services.Accounts;

public sealed class AccountTransactionService(ApplicationDbContext context) : IAccountTransactionService
{
    public async Task CreateEachOtherDepositAsync(
       CreateEachOtherDeposit model
        ) => await new EachOtherDeposit(model, context).OperationAsync();

    public async Task CreateDepositAsync(
       CreateDeposit model
        ) => await new Deposit(model, context).OperationAsync();

    public async Task CreateWithdrawalAsync(CreateWithdrawal model)
       => await new Withdrawal(model, context).OperationAsync();
}
