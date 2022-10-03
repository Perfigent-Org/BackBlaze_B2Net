using System.Collections.Generic;

namespace B2Net.Core.Models
{
    public class BucketsResponse : IResponse
    {
        public List<BucketResponse> Buckets { get; set; }
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
    }
}
