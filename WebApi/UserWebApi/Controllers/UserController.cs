using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using UserWebApi.Models;

namespace UserWebApi.Controllers
{
    [Route("api/users")]
    public class UserController : BaseController
    {
        public UserController(UserContext _userContext) : base(_userContext) { }

        [HttpGet()]
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

        [HttpPost]
        public ResponseMessage InsertUser([FromBody]UserEntity user)
        {
            ResponseMessage result;

            try
            {
                _userContext.UserEntities.Add(user);
                _userContext.SaveChanges();
                result = new ResponseMessage
                {
                    Code = 1,
                    Message = "Success!!!"
                };
            }
            catch (Exception ex)
            {
                result = new ResponseMessage
                {
                    Code = 2,
                    Message = ex.Message
                };
            }

            return
                result;
        }

        [HttpPut]
        public ResponseMessage UpdateUser([FromBody]UserEntity user)
        {
            ResponseMessage result;

            try
            {
                _userContext.UserEntities.Update(user);
                _userContext.SaveChanges();
                result = new ResponseMessage
                {
                    Code = 1,
                    Message = "Success!!!"
                };
            }
            catch (Exception ex)
            {
                result = new ResponseMessage
                {
                    Code = 2,
                    Message = ex.Message
                };
            }

            return
                result;
        }

        [HttpDelete("{userLogin}")]
        public ResponseMessage DeleteUser(string userLogin)
        {
            ResponseMessage result;

            try
            {
                _userContext.UserEntities.Remove(_userContext.UserEntities.FirstOrDefault(u => u.UserLogin == userLogin));
                _userContext.SaveChanges();
                result = new ResponseMessage
                {
                    Code = 1,
                    Message = "Success!!!"
                };
            }
            catch (Exception ex)
            {
                result = new ResponseMessage
                {
                    Code = 2,
                    Message = ex.Message
                };
            }

            return
                result;
        }
    }
}