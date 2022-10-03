using System.Collections.Generic;

namespace B2Net.Core.Models
{
    public class BucketResponse : IResponse
    {
        public string BucketId { get; set; }
        public string BucketName { get; set; }
        public string BucketType { get; set; }
        public Dictionary<string, string> BucketInfo { get; set; }
        public List<BucketLifecycleRuleModel> LifecycleRules { get; set; }
        public List<CORSRuleModel> CORSRules { get; set; }
        public int Revision { get; set; }
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
    }
}
