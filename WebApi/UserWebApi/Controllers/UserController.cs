using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using UserWebApi.Models;

namespace UserWebApi.Controllers
{
    public class UserController : BaseController
    {
        public UserController(UserContext _userContext) : base(_userContext) { }

        [HttpGet]
        public IEnumerable<UserEntity> GetUserAll()
        {
            return
                _userContext.UserEntities.ToArray();
        }

        [HttpGet("{userLogin}")]
        public UserEntity GetUser(string userLogin)
        {
            return
                _userContext.UserEntities.FirstOrDefault(u => u.UserLogin == userLogin);
        }
    }
}