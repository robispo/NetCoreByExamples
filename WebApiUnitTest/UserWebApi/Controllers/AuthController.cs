using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserWebApi.Models;
using UserWebApi.Services;

namespace UserWebApi.Controllers
{
    [Authorize]
    [Route("api/auth")]
    public class AuthController : BaseController
    {
        public AuthController(DataBaseELContext dataBaseELContext, IJwtService jwtService) : base(dataBaseELContext, jwtService) { }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult LogIn([FromBody]AuthModel authModel)
        {
            // This is the code that validate the user, this is upto you, this is only example test.
            if (authModel != null && _dataBaseELContext.UserEntities.Any(u => u.UserLogin == authModel.UserName && u.Password == authModel.Password))
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

        [HttpGet("Test")]
        public IActionResult Test()
        {
            return
                Ok(new { Message = "You are authorized!!!" });
        }

        [HttpGet("SuperTest")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdminTest")]
        public IActionResult SuperTest()
        {
            return
                Ok(new { Message = "You are SupperTester authorized!!!" });
        }
    }
}