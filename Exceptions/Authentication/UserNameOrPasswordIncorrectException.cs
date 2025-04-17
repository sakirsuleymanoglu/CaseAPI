using CaseAPI.Exceptions.Common;

namespace CaseAPI.Exceptions.Authentication;

public sealed class UserNameOrPasswordIncorrectException : BadRequestException
{
    public UserNameOrPasswordIncorrectException() : base("Kullanıcı adı ya da parola hatalı")
    {

    }
}
