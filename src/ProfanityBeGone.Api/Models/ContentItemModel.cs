using Newtonsoft.Json;

namespace ProfanityBeGone.Api.Models
{
    public class ContentItemModel
    {
        [JsonProperty("Value")]
        public string Value { get; set; }
    }
}