# Webapi

* This is the "Webapi of user SimpleLogin" of the other example, but now we add blacklistingToken.
* This is something of our own because we did not find any example a the moment of this kind of implementation.
* In every request that is make (after you are authenticated) the jwt token is renew and send back into the header, after you use a token is blacklisted and you can not use it again.
* We implemented a grace-period too, why? In case that you are making requests to fast you get the change of change, we let a window of ten seconds in this example. 

## Steps

1. Create a method that renew the token (Only if you are autenticated and it is a valid token).

```csharp
public void ValidateAndRenewToken(HttpContext context)
{
    string auth;
    SecurityToken validatedToken;
    ClaimsPrincipal claimsPrincipal;
    IEnumerable<Claim> claims;

    auth = context.Request.Headers[_tokenName];

    if (!string.IsNullOrWhiteSpace(auth))
    {
        auth = auth.Replace(_baererph, string.Empty);

        if (_tokenHandler.CanReadToken(auth))
        {
            try
            {
                claimsPrincipal = _tokenHandler.ValidateToken(auth, _tokenValidationParameters, out validatedToken);
                claims = claimsPrincipal.Claims.Where(c => !_registeredClaimUse.Contains(c.Type));
                this.GenerateToken(context, claims);
            }
            catch (Exception)
            {
            }
        }
    }
}
```

2. Create a middleware to renew the token on all request and implemente at **Configure**.

```csharp
public class JwtMiddleware
{
    readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext context, IJwtService jwtService)
    {
        jwtService.ValidateAndRenewToken(context);
        
        return 
            this._next(context);
    }
}

public static class JwtMiddlewareExtensions
{
    public static IApplicationBuilder UseValidateAndRenewToken(this IApplicationBuilder builder)
    {
        return 
        	builder.UseMiddleware<JwtMiddleware>();
    }
}
```

```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseValidateAndRenewToken();
    app.UseAuthentication();
    app.UseMvc();
}
```

3. Create a **IAuthorizationRequirement** that allow you to manipulate the request before to get pass to the controller action.

```csharp
public class BlacklistingJwtMiddleware : IAuthorizationRequirement
```

```csharp
public class BlacklistingJwtMiddlewareHandler : AuthorizationHandler<BlacklistingJwtMiddleware>
```

4. In the **ConfigureServices** in the **AddAuthorization** block add a **Requirements**, this requirement will use EntityFrameword to save the token in memory and do not allow to use it again (Use the configuration to find static values this is use to know the grace period).

```csharp
services.AddAuthorization(options =>
{
    options.AddPolicy("AdminTest", policy => policy.RequireClaim("SuperTester", "true"));
    options.AddPolicy("BlacklistingJwt", policy => policy.Requirements.Add(new BlacklistingJwtMiddleware(new UserContext(dbOptions), Configuration)));
});
```

5. Force **Authorize** to ask for you requirement.

```csharp
[Authorize(Policy = "BlacklistingJwt")]
```


https://docs.microsoft.com/en-us/ef/core/querying/related-data
https://www.billbogaiv.com/posts/using-aspnet-cores-middleware-to-modify-response-body
https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware?tabs=aspnetcore2x