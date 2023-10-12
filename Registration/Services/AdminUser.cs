using Microsoft.AspNetCore.Identity;

public class AdminUser : IdentityUser
{
    
    public string FullName { get; set; }
    
}
