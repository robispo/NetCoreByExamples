using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using UserWebApi.Models;
using UserWebApi.Services;
using Microsoft.EntityFrameworkCore;

namespace UserWebApi.Controllers
{
    [Route("api/users")] //Url http://{domain}/api/users
    public class UserController : BaseController
    {
        public UserController(UserContext userContext, IJwtService jwtService) : base(userContext, jwtService) { }


        [HttpGet()]
        public IEnumerable<UserEntity> GetUserAll()
        {
            IEnumerable<UserEntity> result;
            result = _userContext.UserEntities.ToArray();

            return
                result;
        }

        [HttpGet("{userLogin}")]
        public UserEntity GetUser(string userLogin)
        {
            var z = Request.Path.Value;

            return
                _userContext.UserEntities
                    //.Include("Roles.Role.Permissions.Permission")
                    .FirstOrDefault(u => u.UserLogin == userLogin);
        }

        [HttpPost]
        public IActionResult InsertUser([FromBody]UserEntity user)
        {
            UserEntity result;

            try
            {
                user.Id = _userContext.UserEntities.Count() + 1;
                _userContext.UserEntities.Add(user);
                _userContext.SaveChanges();
                result = _userContext.UserEntities.FirstOrDefault(u => u.Id == user.Id);

                return
                    Ok(result);
            }
            catch (Exception ex)
            {
                return
                    BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult UpdateUser([FromBody]UserEntity user)
        {
            UserEntity result;

            try
            {
                if (_userContext.UserEntities.Any(u => u.Id == user.Id))
                {
                    _userContext.UserEntities.Update(user);
                    _userContext.SaveChanges();
                    result = _userContext.UserEntities.FirstOrDefault(u => u.Id == user.Id);

                    return
                        Ok(result);
                }
                else
                {
                    return
                        BadRequest(new { Message = "user don not exists." });
                }
            }
            catch (Exception ex)
            {
                return
                    BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{userLogin}")]
        public IActionResult DeleteUser(string userLogin)
        {
            UserEntity result;

            try
            {
                if (_userContext.UserEntities.Any(u => u.UserLogin == userLogin))
                {
                    result = _userContext.UserEntities.FirstOrDefault(u => u.UserLogin == userLogin);
                    _userContext.UserEntities.Remove(result);
                    _userContext.SaveChanges();

                    return
                        Ok(result);
                }
                else
                {
                    return
                        BadRequest(new { Message = "user don not exists." });
                }
            }
            catch (Exception ex)
            {
                return
                    BadRequest(new { Message = ex.Message });
            }
        }
    }
}