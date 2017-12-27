using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserWebApi.Models;
using UserWebApi.Services;

namespace UserWebApi.Controllers
{
    public class EagerLoadingController : BaseController
    {
        public EagerLoadingController(UserContext userContext, IJwtService jwtService) : base(userContext, jwtService) { }

        [HttpGet("Index")]
        public IActionResult Index(string entity, string id, string querystring, string a, string b)
        {
            var z = Request.Path.Value;
            return
               Ok(new { entity, id, querystring ,a,b});
        }
    }
}