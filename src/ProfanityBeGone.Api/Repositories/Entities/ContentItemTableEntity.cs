using Microsoft.Azure.Cosmos.Table;
using ProfanityBeGone.Api.Extensions;

namespace ProfanityBeGone.Api.Repositories.Entities
{
    public sealed class ContentItemTableEntity : TableEntity
    {
        public ContentItemTableEntity()
        {
        }

        public ContentItemTableEntity(string owner, string discriminator, string value)
        {
            owner.ShouldNotBeNullOrWhitespace(nameof(owner));
            discriminator.ShouldNotBeNullOrWhitespace(nameof(discriminator));
            value.ShouldNotBeNullOrWhitespace(nameof(value));

            this.PartitionKey = $"{owner}:{discriminator}";
            this.RowKey = value;
        }

        public string Value => this.RowKey;
    }
}