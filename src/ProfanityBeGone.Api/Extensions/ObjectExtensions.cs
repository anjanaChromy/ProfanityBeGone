using System;

namespace ProfanityBeGone.Api.Extensions
{
    public static class ObjectExtensions
    {
        public static void ShouldNotBeNull(this object source, string name)
        {
            if (source == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void ShouldNotBeNullOrWhitespace(this string source, string name)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}