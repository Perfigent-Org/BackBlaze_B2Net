using B2Net.Client;
using B2Net.Core.Models;
using B2NetCore.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace B2NetCore.API.Controllers
{
    [Route("api/backblaze/buckets")]
    [ApiController]
    public class BucketsController : ControllerBase
    {
        private readonly IB2NetClient _client;
        private readonly ILogger<KeyController> _logger;

        public BucketsController(IB2NetClient client, ILogger<KeyController> logger)
        {
            _client = client;
            _logger = logger;
        }

        [HttpGet("get-all")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestObjectResult))]
        public async Task<IActionResult> Get(CancellationToken cancelToken = default)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                return Ok(await _client.Buckets.GetAll(cancelToken));
            }
            catch (Exception ex)
            {
                var errorMessage = ex.ToAllString();
                _logger.Log(LogLevel.Error, errorMessage);
                return BadRequest(new BucketsResponse { IsSuccessful = false, Message = errorMessage });
            }
        }

        [HttpPost("create")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestObjectResult))]
        public async Task<IActionResult> Create(string bucketName, bool isPrivate, CancellationToken cancelToken = default)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                return Ok(await _client.Buckets.Create(bucketName, isPrivate, cancelToken));
            }
            catch (Exception ex)
            {
                var errorMessage = ex.ToAllString();
                _logger.Log(LogLevel.Error, errorMessage);
                return BadRequest(new BucketResponse { IsSuccessful = false, Message = errorMessage });
            }
        }

        [HttpPut("update")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestObjectResult))]
        public async Task<IActionResult> Update(bool isPrivate, string bucketId, CancellationToken cancelToken = default)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                return Ok(await _client.Buckets.Update(isPrivate, bucketId, cancelToken));
            }
            catch (Exception ex)
            {
                var errorMessage = ex.ToAllString();
                _logger.Log(LogLevel.Error, errorMessage);
                return BadRequest(new BucketResponse { IsSuccessful = false, Message = errorMessage });
            }
        }

        [HttpDelete("delete")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestObjectResult))]
        public async Task<IActionResult> Delete(string bucketId, CancellationToken cancelToken = default)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                return Ok(await _client.Buckets.Delete(bucketId, cancelToken));
            }
            catch (Exception ex)
            {
                var errorMessage = ex.ToAllString();
                _logger.Log(LogLevel.Error, errorMessage);
                return BadRequest(new BucketResponse { IsSuccessful = false, Message = errorMessage });
            }
        }
    }
}
