using System;

namespace WebApplication.Models
{
    public class OrderEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public float Amount { get; set; }
        public DateTime DateCreated { get; set; }
    }
}