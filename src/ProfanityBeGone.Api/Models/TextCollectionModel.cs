using Newtonsoft.Json;

namespace ProfanityBeGone.Api.Models
{
    public class TextCollectionModel
    {
        [JsonProperty("Owner")]
        public string Owner { get; set; }

        [JsonProperty("Discriminator")]
        public string Discriminator { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("LanguageCode")]
        public string LanguageCode { get; set; }
    }
}