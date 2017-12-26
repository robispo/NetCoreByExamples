using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        public List<UserRoleEntity> Roles { get; set; }
    }

    public class UserRoleEntity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public UserEntity User { get; set; }
        public RoleEntity Role { get; set; }
        public List<RolePermissionEntity> Permissions { get; set; }
    }

    public class RolePermissionEntity
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
    }

    public class RoleEntity
    {
        public int Id { get; set; }
        public string Role { get; set; }
    }

    public class PermissionEntity
    {
        public int Id { get; set; }
        public string Permission { get; set; }
    }

    public class JwtTokenEntity
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime CreateDate { get; set; }

    }

    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RolePermissionEntity>().HasKey(t => new { t.RoleId, t.PermissionId });
            modelBuilder.Entity<UserRoleEntity>().HasKey(t => new { t.UserId, t.RoleId });

            modelBuilder.Entity<UserRoleEntity>().HasOne(u => u.User).WithMany(u => u.Roles).HasForeignKey(u => u.UserId);
           // modelBuilder.Entity<RolePermissionEntity>().HasOne(u => u.UserRole).WithMany(u => u.Permissions).HasForeignKey(u => u.RoleId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<UserEntity> UserEntities { get; set; }
        public DbSet<JwtTokenEntity> JwtTokenEntities { get; set; }
        public DbSet<UserRoleEntity> UserRoleEntities { get; set; }
        public DbSet<RolePermissionEntity> RolePermissionEntities { get; set; }
        public DbSet<RoleEntity> RoleEntities { get; set; }
        public DbSet<PermissionEntity> PermissionEntities { get; set; }

    }
}
