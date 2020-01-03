using System;
using Microsoft.AspNetCore.Identity;

namespace WebApplication.Models
{
    public class UserRoleEntity : IdentityRole<Guid>
    {
        public UserRoleEntity() : base()
        {
            
        }
        public UserRoleEntity(string roleName) : base(roleName)
        {
            
        }
    }
}