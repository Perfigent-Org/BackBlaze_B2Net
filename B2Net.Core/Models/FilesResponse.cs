using System.Collections.Generic;

namespace B2Net.Core.Models
{
    public class FilesResponse : IResponse
    {
        public List<FileResponse> Files { get; set; }
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
    }
}
