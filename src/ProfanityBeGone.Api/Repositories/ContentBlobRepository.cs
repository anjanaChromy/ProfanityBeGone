using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using ProfanityBeGone.Api.Extensions;
using ProfanityBeGone.Api.Repositories.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

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

        public async Task<bool> CreateBlobAsync(string containerName, string blobName, Stream content)
        {
            containerName.ShouldNotBeNullOrWhitespace(nameof(containerName));
            blobName.ShouldNotBeNullOrWhitespace(nameof(blobName));
            content.ShouldNotBeNull(nameof(content));

            var blobServiceClient = new BlobServiceClient(_options.Value.AzureStorageConnectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            await containerClient.CreateIfNotExistsAsync().ConfigureAwait(false);

            var blobClient = containerClient.GetBlobClient(blobName);
            var response = await blobClient.UploadAsync(content).ConfigureAwait(false);

            return response.GetRawResponse().Status < 300;
        }
    }
}