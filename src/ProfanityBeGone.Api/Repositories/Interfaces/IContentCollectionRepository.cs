using System.Collections.Generic;
using System.Threading.Tasks;
using ProfanityBeGone.Api.Repositories.Entities;

namespace ProfanityBeGone.Api.Repositories.Interfaces
{
    public interface IContentCollectionRepository<T> where T : ContentCollectionTableEntity, new()
    {
        Task<bool> CreateContentCollectionAsync(T entity);

        Task<T> GetContentCollectionAsync(string owner, string discriminator);

        Task<IEnumerable<T>> GetContentCollectionsAsync(string owner);
    }
}