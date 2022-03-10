using System.Security.Claims;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;


namespace API.Extensions;

public static class UserManagementExtensions
{
    // since we cant use an Include() method with the built in userManager, we'd have to query the contexxt directly.
    // we could either inject that into a controller, or set up an extension like this
    // there's no wrong way of doing it really, this just keeps things cleaner (minus my comments)
    public static  AppUser FindUserByClaimsPrincipleWithAddressAsync(this UserManager<AppUser> input,
        ClaimsPrincipal user)
    {
        // we dont have access to HTTP context when instantiating our controller
        // but we can use the ClaimsPrinciple that makes up the User within HttpContext, we just need to provide it from the controller
        // and pass it here
        // string? email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        string? email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        return input.Users.Include(x => x.Address).SingleOrDefault(x => x.Email == email);
        // this allows us to use our user manager to get the user with their address instead of eeding to inject the context within the account controller
    }

    public static AppUser FindByEmailFromClaimsPrinciple(this UserManager<AppUser> input,
        ClaimsPrincipal user)
    {
        string? email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        return input.Users.SingleOrDefault(x => x.Email == email);
    }
}