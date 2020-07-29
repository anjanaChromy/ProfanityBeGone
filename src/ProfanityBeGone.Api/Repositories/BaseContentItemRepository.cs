using Microsoft.Extensions.Options;
using ProfanityBeGone.Api.Extensions;
using ProfanityBeGone.Api.Repositories.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProfanityBeGone.Api.Repositories
{
    public abstract class BaseContentItemRepository : BaseTableStorageRepository<ContentItemTableEntity>
    {
        protected BaseContentItemRepository(IOptions<AppSettings> options) : base(options)
        {
        }

        public async Task<bool> CreateContentItemAsync(ContentItemTableEntity entity)
        {
            entity.ShouldNotBeNull(nameof(entity));

            return await this.InsertAsync(entity).ConfigureAwait(false);
        }

        public async Task<IEnumerable<ContentItemTableEntity>> GetContentItemsAsync(string owner, string discriminator)
        {
            owner.ShouldNotBeNullOrWhitespace(nameof(owner));
            discriminator.ShouldNotBeNullOrWhitespace(nameof(discriminator));

            return await this.GetAsync($"{owner}:{discriminator}").ConfigureAwait(false);
        }

        protected override bool IsValidEntity(ContentItemTableEntity entity)
        {
            entity.ShouldNotBeNull(nameof(entity));

            var isValidEntity = true;

            if (string.IsNullOrWhiteSpace(entity.PartitionKey))
            {
                isValidEntity = false;
            }
            else if (string.IsNullOrWhiteSpace(entity.RowKey))
            {
                isValidEntity = false;
            }

            return isValidEntity;
        }
    }
}