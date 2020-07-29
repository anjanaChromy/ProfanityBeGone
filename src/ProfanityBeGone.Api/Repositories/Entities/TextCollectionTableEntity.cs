namespace ProfanityBeGone.Api.Repositories.Entities
{
    public class TextCollectionTableEntity : ContentCollectionTableEntity
    {
        public TextCollectionTableEntity() : base()
        {
        }

        public TextCollectionTableEntity(string owner, string discriminator) : base(owner, discriminator)
        {
        }

        public string LanguageCode { get; set; }
    }
}