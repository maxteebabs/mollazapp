using WebApplication.Resources;

namespace WebApplication.Models
{
    public class RootResponse : Resource
    {
        public Link Users { get; set; } 
        public string Info { get; set; } 
    }
}