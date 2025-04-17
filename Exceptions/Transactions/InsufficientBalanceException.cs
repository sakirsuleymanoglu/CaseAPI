using CaseAPI.Exceptions.Common;

namespace CaseAPI.Exceptions.Transactions;

public sealed class InsufficientBalanceException : BadRequestException
{
    public InsufficientBalanceException() : base("Yetersiz bakiye")
    {
    }
}
