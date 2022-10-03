using System;
using System.Collections.Generic;

namespace B2Net.Models
{
    public class B2Key
    {
        public string KeyName { get; set; }
        public string ApplicationKeyId { get; set; }
		public string ApplicationKey { get; set; }
		public string[] Capabilities { get; set; }
		public string AccountId { get; set; }
		public string ExpirationTimestamp { get; set; }
		public string BucketId { get; set; }

		public DateTime UploadTimestampDate
		{
			get
			{
				if (!string.IsNullOrEmpty(ExpirationTimestamp))
				{
					var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
					return epoch.AddMilliseconds(double.Parse(ExpirationTimestamp));
				}
				else
				{
					return DateTime.Now;
				}
			}
		}
	}

	public class B2KeyListDeserializeModel
	{
		public List<B2Key> Keys { get; set; }
	}
}
