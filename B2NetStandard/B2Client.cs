using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;
using B2NetStandard.Models;
using B2NetStandard.Http;
using B2NetStandard.Http.RequestGenerators;

namespace B2NetStandard
{
    public class B2Client : IB2Client
    {
        private B2Options _options;
        private B2Capabilities _capabilities { get; set; }

        public B2Capabilities Capabilities
        {
            get
            {
                if (_options.Authenticated)
                {
                    return _capabilities;
                }
                else
                {
                    throw new NotAuthorizedException("You attempted to load the cabapilities of this key before authenticating with Backblaze. You must Authorize before you can access Capabilities.");
                }
            }
        }
        public IFiles Files { get; private set; }
        public ILargeFiles LargeFiles { get; private set; }

        public B2Client() { }

        public async Task Initialize(B2Options options, bool authorizeOnInitialize = true)
        {
            // Should we authorize on the class initialization?
            if (authorizeOnInitialize)
            {
                _options = await AuthorizeAsync(options);
                Files = new Files(_options);
                LargeFiles = new LargeFiles(_options);
                _capabilities = _options.Capabilities;
            }
            else
            {
                // If not, then the user will have to Initialize() before making any calls.
                _options = options;
            }
        }

        public static async Task<B2Options> AuthorizeAsync(B2Options options)
        {
            // Return if already authenticated.
            if (options.Authenticated)
            {
                return options;
            }

            if (string.IsNullOrWhiteSpace(options.KeyId) || string.IsNullOrWhiteSpace(options.ApplicationKey))
            {
                throw new AuthorizationException("Either KeyId or ApplicationKey were not specified.");
            }

            var client = HttpClientFactory.CreateHttpClient(options.RequestTimeout);

            var requestMessage = AuthRequestGenerator.Authorize(options);
            var response = await client.SendAsync(requestMessage);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var authResponse = JsonConvert.DeserializeObject<B2AuthResponse>(jsonResponse);

                options.SetState(authResponse);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Return a better exception because of confusing Keys api.
                throw new AuthorizationException("If you are using an Application key and not a Master key, make sure that you are supplying the Key ID and Key Value for that Application Key. Do not mix your Account ID with your Application Key.");
            }
            else
            {
                throw new AuthorizationException(jsonResponse);
            }

            return options;
        }
    }
}
