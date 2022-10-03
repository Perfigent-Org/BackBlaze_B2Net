using System.Collections.Generic;

namespace B2Net.Core.Models
{
    public class KeysResponse : IResponse
	{
		public List<KeyResponse> Keys { get; set; }
		public bool IsSuccessful { get; set; }
		public string Message { get; set; }
	}
}
