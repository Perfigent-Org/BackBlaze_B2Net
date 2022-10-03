using B2Net.Http;
using B2Net.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace B2Net
{
    public class Keys : IKeys
    {
        private B2Options _options;
        private HttpClient _client;
        private string _api = "Keys";

        public Keys(B2Options options)
        {
            _options = options;
            _client = HttpClientFactory.CreateHttpClient(options.RequestTimeout);
        }

        public async Task<List<B2Key>> GetList(CancellationToken cancelToken = default)
        {
            var requestMessage = KeyRequestGenerators.GetKeyList(_options);
            var response = await _client.SendAsync(requestMessage, cancelToken);
            var keyList = await ResponseParser.ParseResponse<B2KeyListDeserializeModel>(response);
            return keyList.Keys;
        }

        /// <summary>
        /// Creates a new key. There is a limit of 100 million key creations per account. If validDurationInSeconds and bucketId is not set NULL will be used by default.
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="validDurationInSeconds"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task<B2Key> Create(string keyName, string[] capabilities, int? validDurationInSeconds = null, CancellationToken cancelToken = default)
        {
            var requestMessage = KeyRequestGenerators.CreateKey(_options, keyName, capabilities, validDurationInSeconds);
            var response = await _client.SendAsync(requestMessage, cancelToken);
            return await ResponseParser.ParseResponse<B2Key>(response, _api);
        }

        /// <summary>
        /// Creates a new key. There is a limit of 100 million key creations per account. If validDurationInSeconds and bucketId is not set NULL will be used by default.
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="validDurationInSeconds"></param>
        /// <param name="bucketId"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task<B2Key> Create(string keyName, string[] capabilities, string bucketId, int? validDurationInSeconds = null, CancellationToken cancelToken = default)
        {
            var requestMessage = KeyRequestGenerators.CreateKey(_options, keyName, capabilities, bucketId, validDurationInSeconds);
            var response = await _client.SendAsync(requestMessage, cancelToken);
            return await ResponseParser.ParseResponse<B2Key>(response, _api);
        }

        /// <summary>
        /// Deletes the application Key specified.
        /// </summary>
        /// <param name="applicationKeyId"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        public async Task<B2Key> Delete(string applicationKeyId, CancellationToken cancelToken = default)
        {
            var operationalApplicationKeyId = Utilities.DetermineApplicationId(applicationKeyId);

            var requestMessage = KeyRequestGenerators.DeleteKey(_options, operationalApplicationKeyId);
            var response = await _client.SendAsync(requestMessage, cancelToken);

            return await ResponseParser.ParseResponse<B2Key>(response, _api);
        }
    }
}
