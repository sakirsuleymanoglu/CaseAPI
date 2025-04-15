using CaseAPI.Abstractions.Accounts;
using CaseAPI.Data;
using CaseAPI.Entities;
using CaseAPI.Entities.Enums;
using CaseAPI.Models.Accounts.Transactions;
using Microsoft.EntityFrameworkCore;

namespace CaseAPI.Services.Accounts.Transactions;

public sealed class EachOtherDeposit : TransactionBase, ITransaction
{

    readonly ApplicationDbContext _context;
    readonly CreateEachOtherDeposit _model;
    public EachOtherDeposit(CreateEachOtherDeposit model, ApplicationDbContext context) : base(TransactionChannel.ATM.ToString())
    {
        _model = model;
        _context = context;
    }

    public TransactionType Type { get; } = TransactionType.Deposit;

    public async Task OperationAsync()
    {
        Account? fromAccount = await _context.Accounts.SingleOrDefaultAsync(x => x.Code == _model.FromAccountCode);

        if (fromAccount == null)
            throw new Exception();

        Account? toAccount = await _context.Accounts.SingleOrDefaultAsync(x => x.Code == _model.ToAccountCode);

        if (toAccount == null)
            throw new Exception();

        if (fromAccount.Balance < _model.Amount)
            throw new Exception("Insufficient balance");

        decimal toAccountBalance = toAccount.Balance;
        decimal fromAccountBalance = fromAccount.Balance;

        toAccount.Balance += _model.Amount;
        fromAccount.Balance -= _model.Amount;

        DateTime createdDate = DateTime.Now;

        AccountTransaction fromAccountTransaction = new()
        {
            Id = Guid.NewGuid(),
            Account = fromAccount,
            Amount = _model.Amount,
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
            Amount = _model.Amount,
            Channel = Channel,
            Type = Type,
            Balance = toAccountBalance,
            Description = Type.ToString(),
            CreatedDate = createdDate,
            UpdatedBalance = toAccount.Balance
        };

        await _context.AccountTransactions.AddAsync(fromAccountTransaction);
        await _context.AccountTransactions.AddAsync(toAccountTransaction);

        await _context.SaveChangesAsync();
    }
}
