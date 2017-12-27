using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
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
            //var response = context.HttpContext.Response;
            //response.StatusCode = StatusCodes.Status302Found;
            context.HttpContext.Request.Path = "/api/eagerloading/index";
            //context.Result = RuleResult.EndResponse;
        }
    }
}
