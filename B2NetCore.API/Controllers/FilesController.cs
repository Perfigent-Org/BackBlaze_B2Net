using System;
using System.Net;
using System.Threading.Tasks;
using B2NetCore.API.Extensions;
using B2Net.Core.Models;
using B2Net.Core.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using B2Net.Client;
using System.Threading;

namespace B2NetCore.API.Controllers
{
    [Route("api/backblaze/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IB2NetClient _client;
        private readonly ILogger<FilesController> _logger;

        public FilesController(IB2NetClient client, ILogger<FilesController> logger)
        {
            _client = client;
            _logger = logger;
        }

        [HttpGet("get-all")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestObjectResult))]
        public async Task<IActionResult> GetAll(string bucketId, CancellationToken cancelToken = default)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                return Ok(await _client.Files.GetAll(bucketId, cancelToken));
            }
            catch (Exception ex)
            {
                var errorMessage = ex.ToAllString();
                _logger.Log(LogLevel.Error, errorMessage);
                return BadRequest(new FilesResponse { IsSuccessful = false, Message = errorMessage });
            }
        }

        [HttpGet("get-download-url")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestObjectResult))]
        public IActionResult GetFriendlyDownloadUrl(string fileName, string bucketName, CancellationToken cancelToken = default)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                return Ok(_client.Files.GetFriendlyDownloadUrl(fileName, bucketName, cancelToken));
            }
            catch (Exception ex)
            {
                var errorMessage = ex.ToAllString();
                _logger.Log(LogLevel.Error, errorMessage);
                return BadRequest(ex.ToAllString());
            }
        }

        [HttpGet("get-file")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestObjectResult))]
        public async Task<IActionResult> GetFile(string fileId, CancellationToken cancelToken = default)
        {
            if (!ModelState.IsValid) return null;

            try
            {
                return Ok(await _client.Files.DownloadFile(fileId, cancelToken));
            }
            catch (Exception ex)
            {
                var errorMessage = ex.ToAllString();
                _logger.Log(LogLevel.Error, errorMessage);
                return BadRequest(new FileResponse { IsSuccessful = false, Message = errorMessage });
            }
        }

        [HttpGet("download")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IActionResult))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestObjectResult))]
        public async Task<IActionResult> Download(string fileId, CancellationToken cancelToken = default)
        {
            if (!ModelState.IsValid) return null;

            try
            {
                var responce = await _client.Files.DownloadFile(fileId, cancelToken);
                var stream = new MemoryStream(responce.FileData) { Position = 0 };
                return File(stream, responce.ContentType, responce.FileName);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.ToAllString();
                _logger.Log(LogLevel.Error, errorMessage);
                return BadRequest(new FileResponse { IsSuccessful = false, Message = errorMessage });
            }
        }

        [HttpPost("upload")]
        [RequestFormLimits(MultipartBodyLengthLimit = FileSize.Max)]
        [RequestSizeLimit(FileSize.Max)]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestObjectResult))]
        public async Task<IActionResult> Upload(IFormFile file, string bucketId, string folderName = "", CancellationToken cancelToken = default)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var fileName = !string.IsNullOrWhiteSpace(folderName) ? $"{folderName}/{file.FileName}" : file.FileName;
                return Ok(await _client.Files.UploadFile(await file.ToMemoryStream(), fileName, bucketId, cancelToken));
            }
            catch (Exception ex)
            {
                var errorMessage = ex.ToAllString();
                _logger.Log(LogLevel.Error, errorMessage);
                return BadRequest(new FileResponse { IsSuccessful = false, Message = errorMessage });
            }
        }

        [HttpPost("copy")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestObjectResult))]
        public async Task<IActionResult> Copy(string sourceFileId, string newFileName, string destinationBucketId = "", CancellationToken cancelToken = default)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                return Ok(await _client.Files.Copy(sourceFileId, newFileName, destinationBucketId, cancelToken));
            }
            catch (Exception ex)
            {
                var errorMessage = ex.ToAllString();
                _logger.Log(LogLevel.Error, errorMessage);
                return BadRequest(new FileResponse { IsSuccessful = false, Message = errorMessage });
            }
        }

        [HttpPost("hide")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestObjectResult))]
        public async Task<IActionResult> Hide(string fileName, string bucketId, CancellationToken cancelToken = default)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                return Ok(await _client.Files.Hide(fileName, bucketId, cancelToken));
            }
            catch (Exception ex)
            {
                var errorMessage = ex.ToAllString();
                _logger.Log(LogLevel.Error, errorMessage);
                return BadRequest(new FileResponse { IsSuccessful = false, Message = errorMessage });
            }
        }

        [HttpDelete("delete")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestObjectResult))]
        public async Task<IActionResult> Delete(string fileId, string fileName, CancellationToken cancelToken = default)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                return Ok(await _client.Files.Delete(fileId, fileName, cancelToken));
            }
            catch (Exception ex)
            {
                var errorMessage = ex.ToAllString();
                _logger.Log(LogLevel.Error, errorMessage);
                return BadRequest(new FileResponse { IsSuccessful = false, Message = errorMessage });
            }
        }
    }
}
