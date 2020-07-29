using Microsoft.Extensions.Options;
using ProfanityBeGone.Api.Extensions;
using ProfanityBeGone.Api.Repositories.Entities;
using ProfanityBeGone.Api.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ProfanityBeGone.Api.Repositories
{
    public class TextCollectionRepository : BaseContentCollectionRepository<TextCollectionTableEntity>, ITextCollectionRepository
    {
        private readonly IEnumerable<string> SupportedLanguageCodes = new List<string>()
        {
            "eng"
        };

        public TextCollectionRepository(IOptions<AppSettings> options) : base(options)
        {
        }

        protected override string TableName => "TextCollectionRepository";

        protected override bool IsValidEntity(TextCollectionTableEntity entity)
        {
            entity.ShouldNotBeNull(nameof(entity));

            var isValidEntity = true;

            if (string.IsNullOrWhiteSpace(entity.PartitionKey))
            {
                isValidEntity = false;
            }
            else if (string.IsNullOrWhiteSpace(entity.RowKey))
            {
                isValidEntity = false;
            }
            else if (string.IsNullOrWhiteSpace(entity.LanguageCode) || !SupportedLanguageCodes.Contains(entity.LanguageCode))
            {
                isValidEntity = false;
            }

            return isValidEntity;
        }
    }
}