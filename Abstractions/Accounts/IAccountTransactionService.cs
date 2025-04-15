﻿using CaseAPI.Models.Accounts.Transactions;

namespace CaseAPI.Abstractions.Accounts;

public interface IAccountTransactionService
{
    Task CreateEachOtherDepositAsync(
       CreateEachOtherDeposit model
        );
    Task CreateDepositAsync(
       CreateDeposit model
        );
}