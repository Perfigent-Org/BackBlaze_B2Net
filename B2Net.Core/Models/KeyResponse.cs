using System;

namespace B2Net.Core.Models
{
    public class KeyResponse : IResponse
	{
		public string KeyName { get; set; }
		public string ApplicationKeyId { get; set; }
		public string ApplicationKey { get; set; }
		public string[] Capabilities { get; set; }
		public string AccountId { get; set; }
		public DateTime UploadTimestampDate { get; set; }
		public string BucketId { get; set; }
		public bool IsSuccessful { get; set; }
		public string Message { get; set; }
	}
}
