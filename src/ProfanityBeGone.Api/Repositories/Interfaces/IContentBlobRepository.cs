using System.IO;
using Azure.Storage.Blobs.Models;
using System.Threading.Tasks;

namespace ProfanityBeGone.Api.Repositories.Interfaces
{
    public interface IContentBlobRepository
    {
        Task<BlobDownloadInfo> GetBlobAsync(string containerName, string blobName);
        Task<bool> CreateBlobAsync(string containerName, string blobName, Stream content);
    }
}