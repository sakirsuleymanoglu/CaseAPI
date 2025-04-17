using CaseAPI.Exceptions.Common;

namespace CaseAPI.Exceptions.Transactions;

public sealed class UserHasFromAccountException : BadRequestException
{
    public UserHasFromAccountException() : base("Kullanıcı gönderen hesabına sahip değil")
    {
    }
}
