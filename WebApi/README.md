# Webapi

* This is a "Webapi of user" from scratch in ASP.Net Core MVC
* In this example we change the default json response format to SnakeCase.
* I tried to follow all REST standards that we think of, if there are others that i not following, please, tell me.

## Steps

1. Create simple get of users.
2. Change to routing.
3. All the response are set to json format.
3. Change JSON response format (namingstrategy) into snakecase, REST standards.
4. Add the whole CRUD to the users.

## Support Links

1. [.Net Core](http://www.tutorialsteacher.com/core)
2. [Change JSON format](https://weblog.west-wind.com/posts/2016/Jun/27/Upgrading-to-ASPNET-Core-RTM-from-RC2#ToCamelCaseorNot)
3. [SnakeCaseNamingStrategy](https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_Serialization_SnakeCaseNamingStrategy.htm)

## Code

```csharp
        public void ConfigureServices(IServiceCollection services)
        {
            //This the implementation of EntityFramework using memory database.
            services.AddDbContext<UserContext>(opt => opt.UseInMemoryDatabase("WebapiTest"));

            services.AddMvc()
                // This change the format of the json response to a snakecase.
                .AddJsonOptions(opt =>
                {
                    var resolver = opt.SerializerSettings.ContractResolver;
                    if (resolver != null)
                    {
                        var res = resolver as DefaultContractResolver;
                        res.NamingStrategy = new SnakeCaseNamingStrategy();
                        //res.NamingStrategy = new CamelCaseNamingStrategy();
                        //res.NamingStrategy = null; //This use the format of you already had.
                    }
                }); ;
        }
```

```csharp
    [Produces("application/json")] //All the response in this controller are set to json, not matter what.
    public class BaseController : Controller
```

```csharp
    [Route("api/users")] //Url http://{domain}/api/users
    public class UserController : BaseController
```