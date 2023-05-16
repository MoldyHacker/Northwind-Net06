using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser
{
    public static implicit operator IdentityResult(AppUser v)
    {
        throw new NotImplementedException();
    }
}