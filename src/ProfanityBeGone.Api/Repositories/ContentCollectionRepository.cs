using Microsoft.Extensions.Options;
using ProfanityBeGone.Api.Repositories.Entities;
using ProfanityBeGone.Api.Repositories.Interfaces;

namespace ProfanityBeGone.Api.Repositories
{
    public abstract class ContentCollectionRepository : BaseTableStorageRepository<ContentItemTableEntity>, IContentCollectionRepository
    {
        protected ContentCollectionRepository(IOptions<AppSettings> options) : base (options)
        {
        }
    }
}