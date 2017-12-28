using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserWebApi.Models;
using UserWebApi.Services;

namespace UserWebApi.Controllers
{
    public class EagerLoadingController : BaseController
    {
        public EagerLoadingController(UserContext userContext, IJwtService jwtService) : base(userContext, jwtService) { }

        [HttpGet("Index")]
        public IActionResult Index(string entity, string id, string with)
        {
            object result;

            switch (entity)
            {
                case "users":
                    if (!string.IsNullOrWhiteSpace(id))
                        result = _userContext.Set<UserEntity>().Include(with).FirstOrDefault(u => u.UserLogin == id);
                    else
                        result = _userContext.Set<UserEntity>().Include(with).Select(u=>u);
                    break;
                default:
                    result = null;
                    break;
            }

            return
               Ok(result);
        }


    }
}