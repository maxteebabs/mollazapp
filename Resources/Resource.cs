using System.Text.Json;
using System.Text.Json.Serialization;
using WebApplication.Models;

namespace WebApplication.Resources
{
    public abstract class Resource : Link
    {
//        [JsonIgnore]
        public Link Self { get; set; }
    }
}