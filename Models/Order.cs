using System;
using WebApplication.Resources;

namespace WebApplication.Models
{
    public class Order : Resource
    {
        public float Amount { get; set; }
        public DateTime DateCreated { get; set; }
    }
}