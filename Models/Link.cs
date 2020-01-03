using System.ComponentModel;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebApplication.Models
{
    public class Link
    {
        public const string GetMethod = "GET";
        public static Link To (string routeName, object routeValues= null)
            => new Link()
            {
                RouteName = routeName,
                RouteValues = routeValues,
                Method = GetMethod,
                Relations = null
            };
        public static Link ToCollection(string routeName, object routeValues=null)
            => new Link()
            {
                RouteName = routeName,
                RouteValues = routeValues,
                Method = GetMethod,
                Relations = new [] {"collection"}   
            };
        [JsonProperty(Order = -4)]
        public string Href { get; set; }
        [JsonProperty(Order = -3, PropertyName = "Rel"
            , NullValueHandling = NullValueHandling.Ignore)]
        public string[] Relations { get; set; }
        [JsonProperty(Order = -2
            , DefaultValueHandling = DefaultValueHandling.Ignore
            , NullValueHandling = NullValueHandling.Ignore)]
//        [System.Text.Json.Serialization.JsonIgnore]
        [DefaultValue(GetMethod)]
        public string Method { get; set; }
//        [System.Text.Json.Serialization.JsonIgnore]
        public string RouteName { get; set; }
//        [System.Text.Json.Serialization.JsonIgnore]
        public object RouteValues { get; set; }
    }
}