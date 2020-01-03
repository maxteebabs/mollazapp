using Newtonsoft.Json;
using NJsonSchema;

namespace WebApplication.Models
{
    public class FormField
    {
        public string Name { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Label { get; set; }
//        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
//        public FormFieldOption[] options { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Pattern { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Required { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Secret { get; set; }

//        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
//        [DefaultType(DefaultType)]
//        public string Type { get; set; } = DefaultTypeNameGenerator;
    }
}