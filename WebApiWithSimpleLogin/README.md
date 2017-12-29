# Webapi

* This is the "Webapi of user" of the other example, but now we add a authentication login.
* We add to actions that need to be authenticated to access them, one of them need to had a policy the other only to be login.
* This mean that even if you are an authenticated user you need another access level to get in, (Role access level).
* This is make with JWT, that is very easy to implement.
* In this example we create a delegate class which is the one that has the functionality, also instead of send the token in the body of the message we send it on the header.

## Steps

1. Create a delegate class that generate the token and write it on the header. **Note:** IJwtService is a interface that I create.

```csharp
    public class JwtService : IJwtService
```

```csharp
public void GenerateToken(HttpContext context, IEnumerable<Claim> claims)
{
    JwtSecurityToken token;
    string tokenValue;

    token = new JwtSecurityToken
    (
        issuer : _domain,
        audience : _domain,
        claims : claims,
        expires : DateTime.Now.AddMinutes(30),
        signingCredentials : _creds
    );

    tokenValue = _tokenHandler.WriteToken(token);
    context.Response.Headers.Add(_tokenName, string.Concat(_baererph, tokenValue));
}
```

2. Create a method that set the rule of validation of the token.

```csharp
public void RulesTokenValidation(JwtBearerOptions options)
{
    options.TokenValidationParameters = _tokenValidationParameters;
}
```

3. Use the delegate class in the **ConfigureServices** and the **Configure**

```csharp
public void ConfigureServices(IServiceCollection services)
{
    JwtService jwtService;

    services.AddSingleton<IConfiguration>(Configuration);

    jwtService = new JwtService(Configuration);
    services.Add(new ServiceDescriptor(typeof(IJwtService), jwtService));

    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options => jwtService.RulesTokenValidation(options));

    services.AddDbContext<UserContext>(opt => opt.UseInMemoryDatabase("WebapiTest"));

    services.AddAuthorization(options =>
    {
        options.AddPolicy("AdminTest", policy => policy.RequireClaim("SuperTester", "true"));
    });

    services.AddMvc()
        .AddJsonOptions(opt =>
        {
            var resolver = opt.SerializerSettings.ContractResolver;
            if (resolver != null)
            {
                var res = resolver as DefaultContractResolver;
                res.NamingStrategy = new SnakeCaseNamingStrategy();
            }
        }); ;
}
```

```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseAuthentication();
    app.UseMvc();
}
```

4. Make the controller **authorize only**

```csharp
[Authorize]
public class AuthController : BaseController
```

5. Use the delegate class in the controller to autenticate users.

```csharp
[AllowAnonymous]
[HttpPost]
public IActionResult LogIn([FromBody]AuthModel authModel)
{
    // This is the code that validate the user, this is upto you, this is only example test.
    if (authModel != null && _userContext.UserEntities.Any(u => u.UserLogin == authModel.UserName && u.Password == authModel.Password))
    {
        IEnumerable<Claim> claims;
        claims = authModel.UserName == "robispo"
                    ? new[] { new Claim(ClaimTypes.Name, authModel.UserName), new Claim("SuperTester", "true") } //With this we give access to only a specific user to SuperTest.
                    : new[] { new Claim(ClaimTypes.Name, authModel.UserName) };

        _jwtService.GenerateToken(Request.HttpContext, claims);

        return
            Ok();
    }
    else
    {
        return
            BadRequest(new { Message = "Invalid user or password." });
    }
}
```


## Support Links

1. [JWT](https://jwt.io/)
2. [Authentication with JWT](https://jonhilton.net/security/apis/secure-your-asp.net-core-2.0-api-part-2---jwt-bearer-authentication/)
3. [Authentication with JWT](https://medium.com/@lugrugzo/asp-net-core-2-0-webapi-jwt-authentication-with-identity-mysql-3698eeba6ff8)
4. [Policy Authentication with JWT](http://hamidmosalla.com/2017/10/19/policy-based-authorization-using-asp-net-core-2-and-json-web-token-jwt/)