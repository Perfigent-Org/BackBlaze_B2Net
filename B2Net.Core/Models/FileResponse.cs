using System;
using System.Collections.Generic;

namespace B2Net.Core.Models
{
    public class FileResponse : IResponse
    {
        public string FileId { get; set; }
        public string FileName { get; set; }
        public string Action { get; set; }
        public long Size { get; set; }
        public string UploadTimestamp { get; set; }
        public byte[] FileData { get; set; }
        public string ContentLength { get; set; }
        public string ContentSHA1 { get; set; }
        public string ContentType { get; set; }
        public Dictionary<string, string> FileInfo { get; set; }
        public DateTime UploadTimestampDate { get; set; }
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
    }
}
