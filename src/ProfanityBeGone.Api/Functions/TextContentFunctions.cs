using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ProfanityBeGone.Api.Models;
using ProfanityBeGone.Api.Repositories.Interfaces;

namespace ProfanityBeGone.Api.Functions
{
    public class TextContentFunctions
    {
        private const string ContentType = "text";
        private const string Owner = "public";
        private const string Discriminator = "hackathon2020";

        private readonly IContentBlobRepository _contentBlobRepository;
        private readonly ITextCollectionRepository _textCollectionRepository;
        private readonly ITextItemRepository _textItemRepository;
        private readonly ILogger<TextContentFunctions> _logger;

        public TextContentFunctions(
            IContentBlobRepository contentBlobRepository,
            ITextCollectionRepository textCollectionRepository,
            ITextItemRepository textItemRepository,
            ILogger<TextContentFunctions> logger)
        {
            _contentBlobRepository = contentBlobRepository ?? throw new ArgumentNullException(nameof(contentBlobRepository));
            _textCollectionRepository = textCollectionRepository ?? throw new ArgumentNullException(nameof(textCollectionRepository));
            _textItemRepository = textItemRepository ?? throw new ArgumentNullException(nameof(textItemRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [FunctionName("text-content-build")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {
            var items = await this._textItemRepository.GetContentItemsAsync(Owner, Discriminator).ConfigureAwait(false);

            var content = new List<ContentItemModel>();

            foreach (var item in items)
            {
                content.Add(new ContentItemModel()
                {
                    Value = item.Value
                });
            }

            var collection = await this._textCollectionRepository.GetContentCollectionAsync(Owner, Discriminator).ConfigureAwait(false);
            
            var collectionContent = new TextCollectionContentModel()
            {
                Owner = collection.Owner,
                Discriminator = collection.Discriminator,
                Name = collection.Name,
                Description = collection.Description,
                LanguageCode = collection.LanguageCode,
                Content = content
            };


            return new OkObjectResult(collectionContent);
        }
    }
}
