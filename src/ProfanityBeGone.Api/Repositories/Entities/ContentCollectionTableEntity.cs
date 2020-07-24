using Microsoft.Azure.Cosmos.Table;

namespace ProfanityBeGone.Api.Repositories.Entities
{
    public class ContentCollectionTableEntity : TableEntity
    {
        public ContentCollectionTableEntity()
        {
        }

        public ContentCollectionTableEntity(string owner, string discriminator)
        {

        }

        public string Owner => this.PartitionKey;

        public string Name => this.RowKey;
    }
}