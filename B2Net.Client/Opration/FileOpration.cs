using B2Net.Client.IOpration;
using B2Net.Core.Mapper;
using B2Net.Core.Models;
using B2Net.Core.Settings;
using B2Net.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace B2Net.Client.Opration
{
    public class FileOpration : IFileOpration
    {
        private readonly B2Client _client = null;
        private const string DefaultContentType = "application/octet-stream";

        public FileOpration(B2Client b2Client) { _client = b2Client; }

        public async Task<FilesResponse> GetAll(string bucketId, CancellationToken cancelToken = default)
        {
            return Map.FilesResponse(await _client.Files.GetList(bucketId: bucketId, cancelToken: cancelToken));
        }

        public string GetFriendlyDownloadUrl(string fileName, string bucketName, CancellationToken cancelToken = default)
        {
            return _client.Files.GetFriendlyDownloadUrl(fileName, bucketName, cancelToken);
        }

        public async Task<FileResponse> UploadFile(MemoryStream file, string fileName, string bucketId, CancellationToken cancelToken = default)
        {
            if (file.Length <= FileSize.Small)
            {
                var fileData = file.ToArray();
                file.Close(); await file.DisposeAsync();

                return await UploadSmallFile(fileData, fileName, bucketId, cancelToken);
            }
            else
            {
                return await UploadLargeFile(file, fileName, bucketId, cancelToken);
            }
        }

        public async Task<FileResponse> DownloadFile(string fileId, CancellationToken cancelToken = default)
        {
            B2File file = await _client.Files.DownloadById(fileId, cancelToken);

            if (string.IsNullOrWhiteSpace(file.ContentType))
            {
                if (!MimeTypes.TryGetMimeType(file.FileName, out var mimeType))
                {
                    mimeType = DefaultContentType;
                }
                file.ContentType = mimeType;
            }

            return Map.FileResponse(file);
        }

        public async Task<FileResponse> Copy(string sourceFileId, string newFileName, string destinationBucketId = "", CancellationToken cancelToken = default)
        {
            var copied = await _client.Files.Copy(sourceFileId, newFileName, destinationBucketId: destinationBucketId, cancelToken: cancelToken);

            if (copied.Action != "copy")
                throw new Exception("Action was not as expected for the copy operation.");

            return Map.FileResponse(copied);
        }

        public async Task<FileResponse> Hide(string fileName, string bucketId, CancellationToken cancelToken = default)
        {
            return Map.FileResponse(await _client.Files.Hide(fileName, bucketId: bucketId, cancelToken: cancelToken));
        }

        public async Task<FileResponse> Delete(string fileId, string fileName, CancellationToken cancelToken = default)
        {
            return Map.FileResponse(await _client.Files.Delete(fileId, fileName));
        }

        private async Task<FileResponse> UploadSmallFile(byte[] file, string fileName, string bucketId, CancellationToken cancelToken = default)
        {
            string hash = Utilities.GetSHA1Hash(file);
            var uploadedFile = await _client.Files.Upload(file, fileName, bucketId, cancelToken: cancelToken);
            uploadedFile.FileData = null;

            if (hash != uploadedFile.ContentSHA1)
                throw new Exception("File hashes did not match.");

            return Map.FileResponse(uploadedFile);
        }

        private async Task<FileResponse> UploadLargeFile(MemoryStream file, string fileName, string bucketId, CancellationToken cancelToken = default)
        {
            using (MemoryStream fileStream = file)
            {
                byte[] c = null;
                List<byte[]> parts = new List<byte[]>();
                var shas = new List<string>();
                long fileSize = fileStream.Length;
                long totalBytesParted = 0;
                long minPartSize = 1024 * (5 * 1024);

                while (totalBytesParted < fileSize)
                {
                    var partSize = minPartSize;
                    // If last part is less than min part size, get that length
                    if (fileSize - totalBytesParted < minPartSize)
                    {
                        partSize = fileSize - totalBytesParted;
                    }

                    c = new byte[partSize];
                    fileStream.Seek(totalBytesParted, SeekOrigin.Begin);
                    fileStream.Read(c, 0, c.Length);

                    parts.Add(c);
                    totalBytesParted += partSize;
                }

                foreach (var part in parts)
                {
                    string hash = Utilities.GetSHA1Hash(part);
                    shas.Add(hash);
                }

                B2File start = null;
                B2File finish = null;

                try
                {
                    start = await _client.LargeFiles.StartLargeFile(fileName, "", bucketId);

                    for (int i = 0; i < parts.Count; i++)
                    {
                        var uploadUrl = await _client.LargeFiles.GetUploadPartUrl(start.FileId, cancelToken);
                        var part = await _client.LargeFiles.UploadPart(parts[i], i + 1, uploadUrl, cancelToken);
                    }

                    finish = await _client.LargeFiles.FinishLargeFile(start.FileId, shas.ToArray(), cancelToken);
                }
                catch (Exception)
                {
                    await _client.LargeFiles.CancelLargeFile(start.FileId, cancelToken);
                    throw;
                }

                if (start.FileId != finish.FileId)
                    throw new Exception("File Ids did not match.");

                start.FileData = null;
                return Map.FileResponse(start);
            }
        }
    }
}
