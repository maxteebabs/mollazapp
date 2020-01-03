using WebApplication.Resources;

namespace WebApplication.Models
{
    public class Collection<T> : Resource
    {
        public T[] Value { get; set; }
    }
}