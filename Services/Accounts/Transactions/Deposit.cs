using CaseAPI.Abstractions.Accounts;
using CaseAPI.Data;
using CaseAPI.Entities;
using CaseAPI.Entities.Enums;
using CaseAPI.Models.Accounts.Transactions;
using Microsoft.EntityFrameworkCore;

namespace CaseAPI.Services.Accounts.Transactions;

public sealed class Deposit : TransactionBase, ITransaction
{

    readonly ApplicationDbContext _context;
    readonly CreateDeposit _model;
    public Deposit(CreateDeposit model, ApplicationDbContext context) : base(TransactionChannel.ATM.ToString())
    {
        _model = model;
        _context = context;
    }

    public TransactionType Type { get; } = TransactionType.Deposit;

    public async Task OperationAsync()
    {
        Account? account = await _context.Accounts.SingleOrDefaultAsync(x => x.Code == _model.AccountCode);

        if (account == null)
            throw new Exception();

        decimal balance = account.Balance;

        account.Balance += _model.Amount;

        AccountTransaction transaction = new()
        {
            Id = Guid.NewGuid(),
            Amount = _model.Amount,
            Channel = Channel,
            Account = account,
            Type = Type,
            Description = Type.ToString(),
            CreatedDate = DateTime.Now,
            Balance = balance,
            UpdatedBalance = account.Balance
        };

        await _context.AccountTransactions.AddAsync(transaction);

        await _context.SaveChangesAsync();
    }
}
