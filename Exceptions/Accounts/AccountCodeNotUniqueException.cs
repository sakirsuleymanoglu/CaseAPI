using CaseAPI.Exceptions.Common;

namespace CaseAPI.Exceptions.Accounts;

public sealed class AccountCodeNotUniqueException : BadRequestException
{
    public AccountCodeNotUniqueException() : base("Hesap kodu benzersiz değil")
    {

    }
}
