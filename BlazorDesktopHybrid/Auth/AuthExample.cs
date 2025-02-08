using System.Security.Claims;
using BlazorDesktopHybrid.Storage;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorDesktopHybrid.Auth;

public class UserCredentials
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    
    public ClaimsPrincipal ToClaimsPrincipal() => new(new ClaimsIdentity(new Claim[]
    {
        new (ClaimTypes.Name, Username),
        new (ClaimTypes.Hash, Password),
    }, "BlazorApp"));
    
    public static UserCredentials FromClaimsPrincipal(ClaimsPrincipal principal) => new()
    {
        Username = principal.FindFirst(ClaimTypes.Name)?.Value ?? "",
        Password = principal.FindFirst(ClaimTypes.Hash)?.Value ?? ""
    };
}

public class ExampleUserService(ILocalStorage localStorage)
{
    public UserCredentials? LookupUserInDatabase(string username, string password)
    {
        var usersFromDatabase = new List<UserCredentials>()
        {
            new() { Username = "username", Password = "password"}
        };

        return usersFromDatabase.SingleOrDefault(u => u.Username == username && u.Password == password);
    }
}

public class ExampleAuthenticationStateProvider(ILocalStorage localStorage, ExampleUserService userService)
    : AuthenticationStateProvider
{
    public async Task LoginAsync(string username, string password)
    {
        var principal = new ClaimsPrincipal();
        
        var user = userService.LookupUserInDatabase(username, password);

        if (user is not null)
        {
            // Don't do this, saves password plaintext on client :/ 
            await localStorage.SetAsync(StorageKey, user);
            principal = user.ToClaimsPrincipal();
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }
    
    public async Task LogoutAsync()
    {
        await localStorage.DeleteAsync(StorageKey);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new())));
    }
    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var principal = new ClaimsPrincipal();
        var user = await localStorage.GetAsync<UserCredentials>(StorageKey);

        if (user.Success && user.Value is not null)
        {
            var userInDatabase = userService.LookupUserInDatabase(user.Value.Username, user.Value.Password);
            if (userInDatabase is not null)
            {
                principal = userInDatabase.ToClaimsPrincipal();
            }
        }

        return new(principal);
    }

    private const string StorageKey = "ExampleAuth";
}
