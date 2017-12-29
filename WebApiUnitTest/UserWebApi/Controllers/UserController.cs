using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using UserWebApi.Models;
using UserWebApi.Services;

namespace UserWebApi.Controllers
{
    [Route("api/users")] //Url http://{domain}/api/users
    public class UserController : BaseController
    {
        public UserController(DataBaseELContext dataBaseELContext, IJwtService jwtService) : base(dataBaseELContext, jwtService) { }


        [HttpGet()]
        public IActionResult GetUserAll()
        {
            return
                Ok(_dataBaseELContext.UserEntities.ToArray());
        }

        [HttpGet("{userLogin}")]
        public IActionResult GetUser(string userLogin)
        {
            UserEntity userEntity;

            userEntity = _dataBaseELContext.UserEntities.FirstOrDefault(u => u.UserLogin == userLogin);

            if (userEntity!=null)
            {
                return
                    Ok(userEntity);
            }
            else
            {
                return
                    BadRequest(new { Message = "User do not exists." });
            }
        }

        [HttpPost]
        public IActionResult InsertUser([FromBody]UserEntity user)
        {
            UserEntity result;

            try
            {
                user.Id = _dataBaseELContext.UserEntities.Count() + 1;
                _dataBaseELContext.UserEntities.Add(user);
                _dataBaseELContext.SaveChanges();
                result = _dataBaseELContext.UserEntities.FirstOrDefault(u => u.Id == user.Id);

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
                if (_dataBaseELContext.UserEntities.Any(u => u.Id == user.Id))
                {
                    _dataBaseELContext.UserEntities.Update(user);
                    _dataBaseELContext.SaveChanges();
                    result = _dataBaseELContext.UserEntities.FirstOrDefault(u => u.Id == user.Id);

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
                if (_dataBaseELContext.UserEntities.Any(u => u.UserLogin == userLogin))
                {
                    result = _dataBaseELContext.UserEntities.FirstOrDefault(u => u.UserLogin == userLogin);
                    _dataBaseELContext.UserEntities.Remove(result);
                    _dataBaseELContext.SaveChanges();

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