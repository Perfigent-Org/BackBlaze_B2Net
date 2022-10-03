using B2Net.Client.IOpration;
using B2Net.Core.Mapper;
using B2Net.Core.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace B2Net.Client.Opration
{
    public class BucketOpration : IBucketOpration
    {
        private readonly B2Client _client = null;

        public BucketOpration(B2Client b2Client) { _client = b2Client; }

        public async Task<BucketsResponse> GetAll(CancellationToken cancelToken = default)
        {
            return Map.BucketsResponse(await _client.Buckets.GetList());
        }

        public async Task<BucketResponse> Create(string bucketName, bool isPrivate, CancellationToken cancelToken = default)
        {
            return Map.BucketResponse(await _client.Buckets.Create(bucketName, isPrivate ? BucketTypes.allPrivate : BucketTypes.allPublic));
        }

        public async Task<BucketResponse> Update(bool isPrivate, string bucketId, CancellationToken cancelToken = default)
        {
            if (string.IsNullOrEmpty(bucketId))
            {
                throw new Exception("The bucket was not updated. The response did not contain a bucketid.");
            }

            return Map.BucketResponse(await _client.Buckets.Update(isPrivate ? BucketTypes.allPrivate : BucketTypes.allPublic, bucketId));
        }

        public async Task<BucketResponse> Delete(string bucketId, CancellationToken cancelToken = default)
        {
            if (string.IsNullOrEmpty(bucketId))
            {
                throw new Exception("The bucket was not deleted. The response did not contain a bucketid.");
            }

            return Map.BucketResponse(await _client.Buckets.Delete(bucketId));
        }
    }
}
