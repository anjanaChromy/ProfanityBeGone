using System.Collections.Generic;
using System.Threading.Tasks;
using ProfanityBeGone.Api.Repositories.Entities;

namespace ProfanityBeGone.Api.Repositories.Interfaces
{
    public interface IContentItemRepository
    {
        Task<bool> CreateContentItemAsync(ContentItemTableEntity entity);

        Task<IEnumerable<ContentItemTableEntity>> GetContentItemsAsync(string owner, string discriminator);
    }
}