using CaseAPI.Models.Accounts.Transactions;

namespace CaseAPI.Abstractions.Accounts;

public interface IAccountTransactionService
{
    Task CreateTransferAsync(CreateTransfer model);
    Task<List<ResultTransaction>> GetAllAsync(string accountId);
}