using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;

namespace ProfanityBeGone.Api.Repositories.Interfaces
{
    public interface IContentBlobRepository
    {
        Task<BlobDownloadInfo> GetBlobAsync(string containerName, string blobName);
    }
}