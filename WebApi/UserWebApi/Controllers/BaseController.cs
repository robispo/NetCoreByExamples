﻿using UserWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace UserWebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")] //All the response in this controller are set to json, not matter what.
    public class BaseController : Controller
    {
        protected readonly UserContext _userContext;

        public BaseController(UserContext userContext)
        {
            _userContext = userContext;           
            this.DefaultAdd();
        }

        private void DefaultAdd()
        {
            if (_userContext.UserEntities.Count() == 0)
            {
                _userContext.UserEntities.Add(new UserEntity
                {
                    Id = 1,
                    UserLogin = "robispo",
                    Email = "rabelobispo@hotmail.com",
                    Address = "Address",
                    FirstName = "Rabel",
                    LastName = "Obispo",
                    NickName = "robispo",
                    Password = "robispo"
                });

                _userContext.UserEntities.Add(new UserEntity
                {
                    Id = 2,
                    UserLogin = "jperez",
                    Email = "jperez@hotmail.com",
                    Address = "Address1",
                    FirstName = "Javis",
                    LastName = "Perez",
                    NickName = "jperez",
                    Password = "jperez"
                });

                _userContext.UserEntities.Add(new UserEntity
                {
                    Id = 3,
                    UserLogin = "jdeleon",
                    Email = "jdeleon@hotmail.com",
                    Address = "Address3",
                    FirstName = "Jose",
                    LastName = "De Leon",
                    NickName = "jdeleon",
                    Password = "jdeleon"
                });

                _userContext.SaveChanges();
            }
        }
    }
}