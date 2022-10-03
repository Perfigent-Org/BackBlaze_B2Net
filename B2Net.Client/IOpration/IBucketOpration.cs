using B2Net.Core.Models;
using System.Threading;
using System.Threading.Tasks;

namespace B2Net.Client.IOpration
{
    public interface IBucketOpration
    {
        Task<BucketsResponse> GetAll(CancellationToken cancelToken = default);
        Task<BucketResponse> Create(string bucketName, bool isPrivate, CancellationToken cancelToken = default);
        Task<BucketResponse> Update(bool isPrivate, string bucketId, CancellationToken cancelToken = default);
        Task<BucketResponse> Delete(string bucketId, CancellationToken cancelToken = default);
    }
}
