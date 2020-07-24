using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ProfanityBeGone.Api.Repositories.Interfaces;

namespace ProfanityBeGone.Api.Functions
{
    public class BadWordsFunction
    {
        private const string ContentBlobContainerName = "bad-words";
        private const string ContentBlobName = "english.json";

        private readonly IContentBlobRepository _contentBlobRepository;

        public BadWordsFunction(IContentBlobRepository contentBlobRepository)
        {
            _contentBlobRepository = contentBlobRepository ?? throw new ArgumentNullException(nameof(contentBlobRepository));
        }

        [FunctionName("bad-words")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req, ILogger log)
        {
            IActionResult result;

            var blob = await _contentBlobRepository.GetBlobAsync(ContentBlobContainerName, ContentBlobName).ConfigureAwait(false);

            if (blob != null)
            {
                result = new FileStreamResult(blob.Content, "text/plain");
            }
            else
            {
                result = new NoContentResult();
            }

            return result;
        }
    }
}
