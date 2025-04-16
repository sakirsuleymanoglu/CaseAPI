using CaseAPI.Entities.Enums;

namespace CaseAPI.Abstractions.Accounts.Transactions;

public abstract class TransactionBase
{
    public TransactionChannel Channel { get; }
    public TransactionBase(string channel)
    {
        if (!Enum.TryParse(channel, out TransactionChannel transactionChannel))
            Channel = TransactionChannel.Other;

        Channel = transactionChannel;
    }
}
