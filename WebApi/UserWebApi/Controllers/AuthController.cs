using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UserWebApi.Models;

namespace UserWebApi.Controllers
{
    [Authorize]
    [Route("api/auth")]
    public class AuthController : BaseController
    {
        public AuthController(UserContext _userContext) : base(_userContext) { }

        [AllowAnonymous]
        [HttpPost]
        public object LogIn([FromBody]AuthModel authModel)
        {
            if (authModel != null && _userContext.UserEntities.Any(u => u.UserLogin == authModel.UserName && u.Password == authModel.Password))
            {
                IEnumerable<Claim> claims;
                if (authModel.UserName == "robispo")
                {
                    claims = new[] { new Claim(ClaimTypes.Name, authModel.UserName), new Claim("SuperTester", "true") };
                }
                else
                {
                    claims = new[] { new Claim(ClaimTypes.Name, authModel.UserName) };
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SecurityKey")));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: Environment.GetEnvironmentVariable("Domain"),
                    audience: Environment.GetEnvironmentVariable("Domain"),
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return
                    new { token = new JwtSecurityTokenHandler().WriteToken(token) };
            }
            else
            {
                return
                    BadRequest("Invalid user or password.");
            }
        }

        [HttpGet("Test")]
        public object Test()
        {
            return
                new { Message = "You are authorized!!!" };
        }

        [HttpGet("SuperTest")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdminTest")]
        public object SuperTest()
        {
            return
                new { Message = "You are SupperTester authorized!!!" };
        }
    }
}