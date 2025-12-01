namespace UrlShortner.Application.Interfaces
{
    public interface IHashingService
    {
        /// <summary>
        /// Generate Hash for the given plainText
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        string GetHash(string plainText);

        /// <summary>
        /// Verify that plainText and hash are matching
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="hashed"></param>
        /// <returns></returns>
        bool Verify(string plainText, string hashed);
    }
}
