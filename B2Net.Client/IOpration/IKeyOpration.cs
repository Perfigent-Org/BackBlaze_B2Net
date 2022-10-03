using B2Net.Core.Models;
using System.Threading;
using System.Threading.Tasks;

namespace B2Net.Client.IOpration
{
    public interface IKeyOpration
    {
        Task<KeysResponse> GetAll(CancellationToken cancelToken = default);
        Task<KeyResponse> Create(string keyName, string[] capabilities, int? validDurationInSeconds = null, CancellationToken cancelToken = default);
        Task<KeyResponse> Create(string keyName, string[] capabilities, string bucketId, int? validDurationInSeconds = null, CancellationToken cancelToken = default);
        Task<KeyResponse> Delete(string applicationKeyId, CancellationToken cancelToken = default);
    }
}
