using Microsoft.Extensions.Options;
using ProfanityBeGone.Api.Repositories.Interfaces;

namespace ProfanityBeGone.Api.Repositories
{
    public class TextItemRepository : BaseContentItemRepository, ITextItemRepository
    {
        public TextItemRepository(IOptions<AppSettings> options) : base(options)
        {
        }

        protected override string TableName => "TextItemRepository";
    }
}