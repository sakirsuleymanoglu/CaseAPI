using CaseAPI.Exceptions.Common;

namespace CaseAPI.Exceptions.Transactions;

public sealed class AccountNotFoundException : NotFoundException
{
    public AccountNotFoundException() : base("Hesap bulunamadı")
    {

    }
}
