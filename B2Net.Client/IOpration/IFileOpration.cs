using B2Net.Core.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace B2Net.Client.IOpration
{
    public interface IFileOpration
    {
        Task<FilesResponse> GetAll(string bucketId, CancellationToken cancelToken = default);
        string GetFriendlyDownloadUrl(string fileName, string bucketName, CancellationToken cancelToken = default);
        Task<FileResponse> DownloadFile(string fileId, CancellationToken cancelToken = default);
        Task<FileResponse> UploadFile(MemoryStream file, string fileName, string bucketId, CancellationToken cancelToken = default);
        Task<FileResponse> Copy(string sourceFileId, string newFileName, string destinationBucketId = "", CancellationToken cancelToken = default);
        Task<FileResponse> Hide(string fileName, string bucketId, CancellationToken cancelToken = default);
        Task<FileResponse> Delete(string fileId, string fileName, CancellationToken cancelToken = default);
    }
}
