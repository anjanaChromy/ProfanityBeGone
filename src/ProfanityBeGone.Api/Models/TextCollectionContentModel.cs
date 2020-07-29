using System.Collections.Generic;
using Newtonsoft.Json;

namespace ProfanityBeGone.Api.Models
{
    public class TextCollectionContentModel : TextCollectionModel
    {
        [JsonProperty("Content")]
        public IEnumerable<ContentItemModel> Content { get; set; }
    }
}