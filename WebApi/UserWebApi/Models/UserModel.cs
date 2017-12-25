using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
    

namespace UserWebApi.Models
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string UserLogin { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string Password { get; set; }        
    }

    public class JwtTokenEntity
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime CreateDate { get; set; }

    }

    public class ResponseMessage
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }

    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        public DbSet<UserEntity> UserEntities { get; set; }
        public DbSet<JwtTokenEntity> JwtTokenEntities { get; set; }

    }
}
