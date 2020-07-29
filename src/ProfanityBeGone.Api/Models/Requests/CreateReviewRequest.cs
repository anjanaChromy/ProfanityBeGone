using Newtonsoft.Json;

namespace ProfanityBeGone.Api.Requests
{
    public class CreateReviewRequest
    {
        [JsonProperty("ContentValue")]
        public string ContentValue { get; set; }
    }
}