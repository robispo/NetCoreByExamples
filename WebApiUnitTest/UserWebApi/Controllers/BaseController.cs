﻿using UserWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using UserWebApi.Services;
using System.Collections.Generic;

namespace UserWebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class BaseController : Controller
    {
        protected readonly DataBaseELContext _dataBaseELContext;
        protected readonly IJwtService _jwtService;

        public BaseController(DataBaseELContext dataBaseELContext, IJwtService jwtService)
        {
            _dataBaseELContext = dataBaseELContext;
            _jwtService = jwtService;
            this.DefaultAdd();
        }

        private void DefaultAdd()
        {
            if (_dataBaseELContext.UserEntities.Count() == 0)
            {
                #region Role
                _dataBaseELContext.RoleEntities.Add(new RoleEntity
                {
                    Id = 1,
                    Role = "Adminstrator"
                });

                _dataBaseELContext.RoleEntities.Add(new RoleEntity
                {
                    Id = 2,
                    Role = "User"
                });

                _dataBaseELContext.RoleEntities.Add(new RoleEntity
                {
                    Id = 3,
                    Role = "Supervisor"
                });
                #endregion

                #region Permission
                _dataBaseELContext.PermissionEntities.Add(new PermissionEntity
                {
                    Id = 1,
                    Permission = "CreateUser"
                });

                _dataBaseELContext.PermissionEntities.Add(new PermissionEntity
                {
                    Id = 2,
                    Permission = "ReadUser"
                });

                _dataBaseELContext.PermissionEntities.Add(new PermissionEntity
                {
                    Id = 3,
                    Permission = "UpdateUser"
                });
                _dataBaseELContext.PermissionEntities.Add(new PermissionEntity
                {
                    Id = 4,
                    Permission = "DeleteUser"
                });
                _dataBaseELContext.PermissionEntities.Add(new PermissionEntity
                {
                    Id = 5,
                    Permission = "ApproveUser"
                });
                #endregion

                #region RolePermission
                _dataBaseELContext.RolePermissionEntities.Add(new RolePermissionEntity
                {
                    RoleId = 1,
                    PermissionId = 1,
                    // Permission = _userContext.PermissionEntities.FirstOrDefault(p => p.Id == 1)
                });
                _dataBaseELContext.RolePermissionEntities.Add(new RolePermissionEntity
                {
                    RoleId = 1,
                    PermissionId = 2,
                    // Permission = _userContext.PermissionEntities.FirstOrDefault(p => p.Id == 2)
                });
                _dataBaseELContext.RolePermissionEntities.Add(new RolePermissionEntity
                {
                    RoleId = 1,
                    PermissionId = 3,
                    //  Permission = _userContext.PermissionEntities.FirstOrDefault(p => p.Id == 3)
                });
                _dataBaseELContext.RolePermissionEntities.Add(new RolePermissionEntity
                {
                    RoleId = 1,
                    PermissionId = 4,
                    // Permission = _userContext.PermissionEntities.FirstOrDefault(p => p.Id == 4)
                });
                _dataBaseELContext.RolePermissionEntities.Add(new RolePermissionEntity
                {
                    RoleId = 1,
                    PermissionId = 5,
                    //  Permission = _userContext.PermissionEntities.FirstOrDefault(p => p.Id == 5)
                });
                _dataBaseELContext.RolePermissionEntities.Add(new RolePermissionEntity
                {
                    RoleId = 2,
                    PermissionId = 2,
                    //Permission = _userContext.PermissionEntities.FirstOrDefault(p => p.Id == 2)
                });
                _dataBaseELContext.RolePermissionEntities.Add(new RolePermissionEntity
                {
                    RoleId = 3,
                    PermissionId = 2,
                    //  Permission = _userContext.PermissionEntities.FirstOrDefault(p => p.Id == 2)
                });
                _dataBaseELContext.RolePermissionEntities.Add(new RolePermissionEntity
                {
                    RoleId = 3,
                    PermissionId = 5,
                    //  Permission = _userContext.PermissionEntities.FirstOrDefault(p => p.Id == 5)
                });
                #endregion

                #region UserRole
                _dataBaseELContext.UserRoleEntities.Add(new UserRoleEntity
                {
                    UserId = 1,
                    RoleId = 1,
                });
                _dataBaseELContext.UserRoleEntities.Add(new UserRoleEntity
                {
                    UserId = 2,
                    RoleId = 2,
                });
                _dataBaseELContext.UserRoleEntities.Add(new UserRoleEntity
                {
                    UserId = 2,
                    RoleId = 3,
                });
                _dataBaseELContext.UserRoleEntities.Add(new UserRoleEntity
                {
                    UserId = 3,
                    RoleId = 2,
                });
                #endregion

                #region User
                _dataBaseELContext.UserEntities.Add(new UserEntity
                {
                    Id = 1,
                    UserLogin = "robispo",
                    Email = "rabelobispo@hotmail.com",
                    Address = "Address1",
                    FirstName = "Rabel",
                    LastName = "Obispo",
                    NickName = "robispo",
                    Password = "robispo",
                    //Roles = new RoleEntity[] { new RoleEntity { Id = 1, Role = "Adminstrator", Permissions = new PermissionEntity[] { new PermissionEntity { Id = 1, Permission = "CreateUser" } } } }
                });

                _dataBaseELContext.UserEntities.Add(new UserEntity
                {
                    Id = 2,
                    UserLogin = "jperez",
                    Email = "jperez@hotmail.com",
                    Address = "Address1",
                    FirstName = "Javis",
                    LastName = "Perez",
                    NickName = "jperez",
                    Password = "jperez",
                    //Roles = new UserRoleEntity[] { new UserRoleEntity { UserId = 2, RoleId = 3 }, new UserRoleEntity { UserId = 2, RoleId = 2 } }
                });

                _dataBaseELContext.UserEntities.Add(new UserEntity
                {
                    Id = 3,
                    UserLogin = "jdeleon",
                    Email = "jdeleon@hotmail.com",
                    Address = "Address3",
                    FirstName = "Jose",
                    LastName = "De Leon",
                    NickName = "jdeleon",
                    Password = "jdeleon",
                    //Roles = new UserRoleEntity[] { new UserRoleEntity { UserId = 3, RoleId = 2 } }
                });
                #endregion

                #region UserOld
                /*_userContext.UserEntities.Add(new UserEntity
                {
                    Id = 1,
                    UserLogin = "robispo",
                    Email = "rabelobispo@hotmail.com",
                    Address = "Address",
                    FirstName = "Rabel",
                    LastName = "Obispo",
                    NickName = "robispo",
                    Password = "robispo",
                    Roles = new UserRoleEntity[] { new UserRoleEntity { UserId = 1, RoleId = 1 } }
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
                    Password = "jperez",
                    Roles = new UserRoleEntity[] { new UserRoleEntity { UserId = 2, RoleId = 3 }, new UserRoleEntity { UserId = 2, RoleId = 2 } }
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
                    Password = "jdeleon",
                    Roles = new UserRoleEntity[] { new UserRoleEntity { UserId = 3, RoleId = 2 } }
                });*/
                #endregion

                _dataBaseELContext.SaveChanges();
            }
        }
    }
}