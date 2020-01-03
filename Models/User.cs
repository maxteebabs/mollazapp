using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using WebApplication.Infrastructure;
using WebApplication.Resources;

namespace WebApplication.Models
{
    public class User : Resource, IEtaggable
    {
        [Searchable]
        public string UserName { get; set; }
        
        [Searchable]
        public string FirstName { get; set; }
        [Searchable]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int IsAdmin { get; set; }
        [Sortable(Default = true)]
        public DateTime DateCreated { get; set; }
        public Address Location { get; set; }
        public string GetEtag()
        {
            var serialized = JsonConvert.SerializeObject(this);
            MD5 Md5Hash = MD5.Create();
            string hash = GetMd5Hash(Md5Hash, serialized);
            return hash;
        }
        
        public string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }

    public class Address
    {
        public string City { get; set; }
        public string Country { get; set; }
    }
    
}