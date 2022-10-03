using B2Net.Client.IOpration;
using B2Net.Core.Mapper;
using B2Net.Core.Models;
using System.Threading;
using System.Threading.Tasks;

namespace B2Net.Client.Opration
{
    public class KeyOpration : IKeyOpration
    {
        private readonly B2Client _client = null;

        public KeyOpration(B2Client b2Client) { _client = b2Client; }

        public async Task<KeysResponse> GetAll(CancellationToken cancelToken = default)
        {
            return Map.KeysResponse(await _client.Keys.GetList(cancelToken));
        }

        public async Task<KeyResponse> Create(string keyName, string[] capabilities, int? validDurationInSeconds = null, CancellationToken cancelToken = default)
        {
            return Map.KeyResponse(await _client.Keys.Create(keyName, capabilities, validDurationInSeconds, cancelToken));
        }

        public async Task<KeyResponse> Create(string keyName, string[] capabilities, string bucketId, int? validDurationInSeconds = null, CancellationToken cancelToken = default)
        {
            return Map.KeyResponse(await _client.Keys.Create(keyName, capabilities, bucketId, validDurationInSeconds, cancelToken));
        }

        public async Task<KeyResponse> Delete(string applicationKeyId, CancellationToken cancelToken = default)
        {
            return Map.KeyResponse(await _client.Keys.Delete(applicationKeyId, cancelToken));
        }
    }
}
