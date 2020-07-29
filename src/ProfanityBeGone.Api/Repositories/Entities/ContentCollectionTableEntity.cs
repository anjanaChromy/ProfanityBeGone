using Microsoft.Azure.Cosmos.Table;
using ProfanityBeGone.Api.Extensions;

namespace ProfanityBeGone.Api.Repositories.Entities
{
    public abstract class ContentCollectionTableEntity : TableEntity
    {
        protected ContentCollectionTableEntity()
        {
        }

        protected ContentCollectionTableEntity(string owner, string discriminator)
        {
            owner.ShouldNotBeNullOrWhitespace(nameof(owner));
            discriminator.ShouldNotBeNullOrWhitespace(nameof(discriminator));

            this.PartitionKey = owner;
            this.RowKey = discriminator;
        }

        public string Owner => this.PartitionKey;

        public string Discriminator => this.RowKey;

        public string Name { get; set; }

        public string Description { get; set; }
    }
}