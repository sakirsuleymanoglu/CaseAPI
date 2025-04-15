namespace CaseAPI.Models.Jwt;

public class CreatedJwt
{
    public string? Token { get; set; }
    public DateTime Expiration { get; set; }
}
