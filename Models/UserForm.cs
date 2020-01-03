using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.DataProtection;

namespace WebApplication.Models
{
    public class UserForm
    {
        [Required]
        [Display(Name = "firstName", Description = "Firstname")]
        public string? FirstName { get; set; }
        [Required]
        [Display(Name = "lastName", Description = "Lastname")]
        public string? LastName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email", Description = "Email Field")]
        public string Email { get; set; }
        
        [MinLength(8)]
        [MaxLength(100)]
        [Required]
        [Display(Name="Password", Description = "Password")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Phone", Description = "Phone")]
        public string Phone { get; set; }

        [Display(Name = "City", Description = "City")]
        public string? City { get; set; }
        [Display(Name = "Country", Description = "Country")]
        public string? Country { get; set; }
//        [Required]
//        [Display(Name = "startAt", Description = "Booking Start Time")]
//        public DateTimeOffset? StartAt { get; set; }
//        
//        [Required]
//        [Display(Name = "endAt", Description = "Booking Start Time")]
//        public DateTimeOffset? EndAt { get; set; }
    }
}