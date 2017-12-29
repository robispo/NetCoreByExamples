# Webapi

* This is the "Webapi of user blacklistingToken" of the other example, but now we add eagerloading.
* We add the eagerloading concep base on the url with the key-word **with**, if you use this key-word you are rewrite to another controller action that make the eagerloading.
* We use the Microsoft rewrite middleware and implemented our own rule to do so.
* Inside of the action after the rewrite, I can find a way to set the model entity base on the url because Set method does not return a generic type, if someone knows a better way to do it, please, show me how.

## Example

* This url will return all user **http://localhost:8610/api/users** in you look the property call **roles** is null.
* This url will return a specific user **http://localhost:8610/api/users/robispo** in you look the property call **roles** is null.
* If you add a querystring like this **?with=roles.role.permissions.permission** to any of the above urls will fill with eagerloading the dependency.
* You can take out and only load one entity, **example:** I only want the roles ids i do not need the description you can do it like **?with=roles.**.
* But if you need the the dependency you can not take out the parent, **example:** **?with=roles.role.permission**

## Code

1. Create rule to rewrite, if the key-word **with** is in the querystring rewrite the url only on server-side.

```csharp
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;

namespace UserWebApi.Services
{
    public class RewriteRule : IRule
    {
        public void ApplyRule(RewriteContext context)
        {
            string[] blocks;
            QueryString queryString;

            if (context.HttpContext.Request.Query.ContainsKey("with"))
            {
                blocks = context.HttpContext.Request.Path.Value.Split("/");

                queryString = new QueryString()
                                    .Add("entity", blocks[2])
                                    .Add("id", blocks.Length > 3 ? blocks[3] : string.Empty)
                                    .Add("with", context.HttpContext.Request.Query["with"]);

                context.HttpContext.Request.QueryString = queryString;
                context.HttpContext.Request.Path = "/api/eagerloading/index";
            }
        }
    }
}

```

2. Implementing the rule at **Configure**. **Note:** the implementation must be before **app.UseMvc();** in order to work.

```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    RewriteOptions rewriteOptions;

    rewriteOptions = new RewriteOptions()
        .Add(new RewriteRule());
    app.UseRewriter(rewriteOptions);           
}   
```

3. This is the action that make the magic happen.

* If you look I make a switch to match the entitymodel with the string entity that you looking for, that means that for every entity that you want to use in this way you need to come to the switch statement and add it.
* Sorry! But I could not find another way.

```csharp
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserWebApi.Models;
using UserWebApi.Services;

namespace UserWebApi.Controllers
{
    public class EagerLoadingController : BaseController
    {
        public EagerLoadingController(DataBaseELContext dataBaseELContext, IJwtService jwtService) : base(dataBaseELContext, jwtService) { }

        [HttpGet("Index")]
        public IActionResult Index(string entity, string id, string with)
        {
            object result;
            string[] withParse;

            try
            {
                withParse = this.ParseWith(with);
                switch (entity)
                {
                    case "users":
                        IQueryable<UserEntity> dbSet;
                        dbSet = _dataBaseELContext.UserEntities;

                        for (int i = 0; i < withParse.Length; i++)
                            dbSet = dbSet.Include(withParse[i]);

                        if (!string.IsNullOrWhiteSpace(id))
                            result = dbSet.FirstOrDefault(u => u.UserLogin == id);
                        else
                            result = dbSet.Select(u => u).ToArray();
                        break;
                    default:
                        result = null;
                        break;
                }

                return
                   Ok(result);
            }
            catch (Exception ex)
            {
                return
                    BadRequest(new { ex.Message });
            }
        }

        private string[] ParseWith(string with)
        {
            string[] includes, thenincludes;
            CultureInfo cultureInfo;
            TextInfo textInfo;

            cultureInfo = Thread.CurrentThread.CurrentCulture;
            textInfo = cultureInfo.TextInfo;

            includes = with.Split(",");

            for (int i = 0; i < includes.Length; i++)
            {
                thenincludes = includes[i].Split(".");

                for (int j = 0; j < thenincludes.Length; j++)
                    thenincludes[j] = textInfo.ToTitleCase(thenincludes[j]);

                includes[i] = string.Join(".", thenincludes);
            }

            return
                includes;
        }
    }
}
```

## Support Links

1. [Unit testing frameworks](https://raygun.com/blog/unit-testing-frameworks-c/)
2. [Unit Testing ASP.NET Web API 2](https://www.asp.net/web-api/raw-content/tutorials/testing-and-debugging/unit-testing-aspnet-web-api)
3. [Create A Web API And Call It Using A Desktop Client Application](http://www.c-sharpcorner.com/article/create-a-web-api-and-call-it-using-a-desktop-client-application/)
4. [How can I dynamically add Include to ObjectSet<Entity> using C#?](https://stackoverflow.com/questions/16054284/how-can-i-dynamically-add-include-to-objectsetentity-using-c)
5. [ASP.NET CORE 2.0 URL REWRITING (Rule)](https://tahirnaushad.com/2017/08/18/url-rewriting-in-asp-net-core/)