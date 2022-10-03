using B2NetStandard.Extensions;
using B2NetStandard.IOpration;
using B2NetStandard.Models;
using B2NetStandard.Opration;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace B2NetStandard
{
    public class FilesController : ClientBase
    {
        private readonly IB2Client _client;
        private IFileOpration _files;
        public bool IsAuthorize = false;

        public FilesController(string applicationKeyId, string applicationKey) : base(applicationKeyId, applicationKey)
        {
            _client = new B2Client();
        }

        public async Task AuthorizeAsync()
        {
            try
            {
                await _client.Initialize(Options);
                if (_files == null)
                    _files = new FileOpration(_client);
                IsAuthorize = true;
            }
            catch (Exception ex)
            {
                IsAuthorize = false;
                throw ex;
            }
        }

        public async Task<B2File> Upload(FileStream file, string fileName, string bucketId, string folderName = "", CancellationToken cancelToken = default)
        {
            try
            {
                if (!IsAuthorize) throw new UnauthorizedAccessException("Please do authorize first.");

                fileName = !string.IsNullOrWhiteSpace(folderName) ? $"{folderName}/{fileName}" : fileName;
                var responce = await _files.UploadFile(await file.ToMemoryStream(), fileName, bucketId, cancelToken: cancelToken);
                return responce;
            }
            catch (Exception ex)
            {
               return new B2File { IsSuccessful = false, Message = ex.ToAllString() };
            }
        }

        public async Task<B2File> Download(string fileId, CancellationToken cancelToken = default)
        {
            try
            {
                if (!IsAuthorize) throw new UnauthorizedAccessException("Please do authorize first.");
                var responce = await _files.DownloadFile(fileId, cancelToken: cancelToken);
                return responce;
            }
            catch (Exception ex)
            {
                return new B2File { IsSuccessful = false, Message = ex.ToAllString() };
            }
        }
    }
}