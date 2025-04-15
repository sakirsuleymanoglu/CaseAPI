namespace CaseAPI.Exceptions.Authentication;

public class UserNameOrPasswordIncorrectException : Exception
{
    public UserNameOrPasswordIncorrectException() : base("Kullanıcı adı ya da parola hatalı")
    {
    }
}
