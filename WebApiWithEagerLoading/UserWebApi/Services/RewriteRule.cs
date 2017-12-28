using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
