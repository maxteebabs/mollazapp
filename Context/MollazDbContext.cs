using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication.Models;

namespace WebApplication.Context
{
//    public class MollazDbContext : DbContext
    public class MollazDbContext : IdentityDbContext<UserEntity, UserRoleEntity, Guid>
    {
        public MollazDbContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<UserEntity> Users { get; set; }
    }
}