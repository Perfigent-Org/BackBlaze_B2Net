using B2NetStandard.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace B2NetStandard.IOpration
{
    public interface IFileOpration
    {
        Task<B2File> DownloadFile(string fileId, CancellationToken cancelToken = default);
        Task<B2File> UploadFile(MemoryStream file, string fileName, string bucketId, CancellationToken cancelToken = default);
    }
}
