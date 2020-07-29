using Microsoft.Extensions.Options;
using ProfanityBeGone.Api.Extensions;
using ProfanityBeGone.Api.Repositories.Entities;
using ProfanityBeGone.Api.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProfanityBeGone.Api.Repositories
{
    public abstract class BaseContentCollectionRepository<T> : BaseTableStorageRepository<T>, IContentCollectionRepository<T> where T : ContentCollectionTableEntity, new()
    {
        protected BaseContentCollectionRepository(IOptions<AppSettings> options) : base (options)
        {
        }

        public async Task<bool> CreateContentCollectionAsync(T entity)
        {
            entity.ShouldNotBeNull(nameof(entity));

            return await this.InsertAsync(entity).ConfigureAwait(false);
        }

        public async Task<T> GetContentCollectionAsync(string owner, string discriminator)
        {
            owner.ShouldNotBeNullOrWhitespace(nameof(owner));
            discriminator.ShouldNotBeNullOrWhitespace(nameof(discriminator));

            return await this.GetEntityAsync(owner, discriminator).ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> GetContentCollectionsAsync(string owner)
        {
            owner.ShouldNotBeNullOrWhitespace(nameof(owner));

            return await this.GetAsync(owner).ConfigureAwait(false);
        }
    }
}