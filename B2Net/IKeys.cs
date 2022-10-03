using B2Net.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace B2Net
{
    public interface IKeys
    {
        Task<List<B2Key>> GetList(CancellationToken cancelToken = default);
        Task<B2Key> Create(string keyName, string[] capabilities, int? validDurationInSeconds = null, CancellationToken cancelToken = default);
        Task<B2Key> Create(string keyName, string[] capabilities, string bucketId, int? validDurationInSeconds = null, CancellationToken cancelToken = default);
        Task<B2Key> Delete(string applicationKeyId, CancellationToken cancelToken = default);
    }
}
