using Microsoft.Azure.Cosmos.Table;

namespace ProfanityBeGone.Api.Repositories.Entities
{
    public sealed class ContentItemTableEntity : TableEntity
    {
        public ContentItemTableEntity()
        {
        }

        public ContentItemTableEntity(string owner, string name)
        {
        }

        public string Value => this.RowKey;
    }
}