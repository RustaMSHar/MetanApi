using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using MetanApi.Models;
using MetanApi.Registration.Models;

public class UserService
{
    private readonly UserManager<AdminUser> _userManager;
    private readonly IMongoCollection<AdminUser> _userCollection;

    public UserService(UserManager<AdminUser> userManager, IMongoDatabase database)
    {
        _userManager = userManager;
        _userCollection = database.GetCollection<AdminUser>("Admin");
    }

    public async Task<IdentityResult> RegisterAsync(RegisterModel model)
    {
        var user = new AdminUser { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);
        return result;
    }
}
