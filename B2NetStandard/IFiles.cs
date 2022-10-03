using B2NetStandard.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace B2NetStandard
{
    public interface IFiles
    {
        Task<B2File> Upload(byte[] fileData, string fileName, string bucketId = "", Dictionary<string, string> fileInfo = null, CancellationToken cancelToken = default(CancellationToken));
        Task<B2File> DownloadById(string fileId, CancellationToken cancelToken = default(CancellationToken));
    }
}
