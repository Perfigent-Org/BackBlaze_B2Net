using B2NetStandard.Extensions;
using B2NetStandard.IOpration;
using B2NetStandard.Models;
using B2NetStandard.Opration;
using log4net;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace B2NetStandard.UploadFile
{
    public class UploadFilesController : ClientBase
    {
        private readonly IB2Client _client;
        private IFileOpration _files;
        public bool IsAuthorize = false;
        private readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public UploadFilesController(string applicationKeyId, string applicationKey) : base(applicationKeyId, applicationKey)
        {
            _client = new B2Client();
        }

        public async Task AuthorizeAsync()
        {
            log.Info("B2Net Client Authorizetion Started...");

            try
            {
                await _client.Initialize(Options);
                if (_files == null)
                    _files = new FileOpration(_client);
                IsAuthorize = true;

                log.Info("B2Net Client Authorizetion Completed.");
            }
            catch (Exception ex)
            {
                IsAuthorize = false; 
                log.Error("Authorizetion Error");
                log.Error(ex.ToAllString());
                throw ex;
            }
        }

        public async Task<B2File> Upload(FileStream file, string fileName, string bucketId, string folderName, CancellationToken cancelToken = default)
        {
            try
            {
                if (!IsAuthorize) throw new UnauthorizedAccessException("Please do authorize first.");

                log.Info($"[{fileName}] File Upload Process Started...");
                log.Info($"File Bytes are: [{file.Length}], FileName is: [{fileName}], BucketId is: [{bucketId}], FolderName is: [{folderName}]");

                var fileWithFolderName = !string.IsNullOrWhiteSpace(folderName) ? $"{folderName}/{fileName}" : fileName;
                var responce = await _files.UploadFile(await file.ToMemoryStream(), fileWithFolderName, bucketId, cancelToken: cancelToken);

                log.Info($"[{fileName}] File Upload Process Completed.");
                log.Info($"[{fileName}] File Uploaded In BucketId is [{bucketId}] and Folder is [{folderName}].");
                log.Info($"Uploaded FileId is: [{responce.FileId}]");

                return responce;
            }
            catch (Exception ex)
            {
                var error = ex.ToAllString();
                log.Error($"[{fileName}] File Upload Error: ");
                log.Error(error);
                return new B2File { IsSuccessful = false, Message = error };
            }
        }
    }
}