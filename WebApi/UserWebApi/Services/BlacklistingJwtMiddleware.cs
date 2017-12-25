using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UserWebApi.Models;

namespace UserWebApi.Services
{
    public class BlacklistingJwtMiddleware : IAuthorizationRequirement
    {
        UserContext _userContext;
        IConfiguration _configuration;
        readonly string _tokenName, _applyGracePeriod;
        readonly int _gracePeriod;

        public BlacklistingJwtMiddleware(UserContext userContext, IConfiguration configuration)
        {
            _userContext = userContext;
            _configuration = configuration;
            _tokenName = "Authorization";
            _gracePeriod = int.Parse(_configuration["gracePeriod"]);
            _applyGracePeriod = _configuration["applyGracePeriod"].Trim().ToLower();
        }

        public bool ValidateToken(HttpContext context)
        {
            bool result;
            string tokenValue;
            JwtTokenEntity lastToken;
            TimeSpan timeSpan;
            int totalSeconds;

            result = true;

            if (_applyGracePeriod == "true")
            {
                tokenValue = context.Request.Headers[_tokenName];

                if (!string.IsNullOrWhiteSpace(tokenValue))
                {
                    lastToken = _userContext.JwtTokenEntities.Where(t=>t.Token == tokenValue).OrderBy(t => t.CreateDate).FirstOrDefault();

                    if (lastToken != null)
                    {
                        timeSpan = DateTime.Now - lastToken.CreateDate;
                        totalSeconds = (int)timeSpan.TotalSeconds;

                        if (totalSeconds > _gracePeriod)
                            result = false;
                    }

                    _userContext.JwtTokenEntities.Add(new JwtTokenEntity { Id = _userContext.JwtTokenEntities.Count() + 1, Token = tokenValue, CreateDate = DateTime.Now });
                    _userContext.SaveChanges();
                }
            }

            return
                result;
        }
    }

    public class BlacklistingJwtMiddlewareHandler : AuthorizationHandler<BlacklistingJwtMiddleware>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BlacklistingJwtMiddleware requirement)
        {
            if (requirement.ValidateToken(((ActionContext)context.Resource).HttpContext))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return
                Task.CompletedTask;
        }
    }
}
