using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProfanityBeGone.Api.Extensions;
using ProfanityBeGone.Api.Repositories.Interfaces;

namespace ProfanityBeGone.Api.Repositories
{
    public class ContentBlobRepository : IContentBlobRepository
    {
        private readonly IOptions<AppSettings> _options;

        public ContentBlobRepository(IOptions<AppSettings> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<BlobDownloadInfo> GetBlobAsync(string containerName, string blobName)
        {
            containerName.ShouldNotBeNullOrWhitespace(nameof(containerName));
            blobName.ShouldNotBeNullOrWhitespace(nameof(blobName));

            BlobDownloadInfo blob = null;

            var blobServiceClient = new BlobServiceClient(_options.Value.AzureStorageConnectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            if (await containerClient.ExistsAsync().ConfigureAwait(false))
            {
                var blobClient = containerClient.GetBlobClient(blobName);

                if (await blobClient.ExistsAsync().ConfigureAwait(false))
                {
                    blob = await blobClient.DownloadAsync().ConfigureAwait(false);
                }
            }

            return blob;
        }
    }
}