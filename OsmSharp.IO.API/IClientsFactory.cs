namespace OsmSharp.IO.API
{
    /// <summary>
    /// This factory is used to create all the OSM API clients
    /// This inteface is provided to allow mocking and DI
    /// </summary>
    public interface IClientsFactory
    {
        /// <summary>
        /// Creates a client that does not need authentication and thus can't perform authenticated operations
        /// </summary>
        /// <returns></returns>
        INonAuthClient CreateNonAuthClient();

        /// <summary>
        /// Creates a client that will use OAuth 2.0 credentials provided from the OAuth OSM page
        /// </summary>
        /// <param name="token">The token that you got after getting the code and posting it to the token server</param>
        /// <returns></returns>
        IAuthClient CreateOAuth2Client(string token);
    }
}