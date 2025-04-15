using Microsoft.AspNetCore.Identity;

namespace CaseAPI.Entities;

public sealed class AppUser : IdentityUser<Guid>
{
    public AppUser()
    {
        Accounts = new HashSet<Account>();
    }

    public ICollection<Account> Accounts { get; set; }
}
