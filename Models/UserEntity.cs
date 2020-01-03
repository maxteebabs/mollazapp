using System;
using Microsoft.AspNetCore.Identity;

namespace WebApplication.Models
{
    public class UserEntity : IdentityUser<Guid>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int IsAdmin { get; set; }
        public DateTime DateCreated { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Password { get; set; }
    }
    
}
