using System;
using System.Net;
using System.Threading.Tasks;
using B2Net.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using B2Net.Client;
using B2NetCore.API.Extensions;

namespace B2NetCore.API.Controllers
{
    [Route("api/backblaze/key")]
    [ApiController]
    public class KeyController : ControllerBase
    {
        private readonly IB2NetClient _client;
        private readonly ILogger<KeyController> _logger;

        public KeyController(IB2NetClient client, ILogger<KeyController> logger)
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
                return Ok(await _client.Keys.GetAll(cancelToken));
            }
            catch (Exception ex)
            {
                var errorMessage = ex.ToAllString();
                _logger.Log(LogLevel.Error, errorMessage);
                return BadRequest(new KeysResponse { IsSuccessful = false, Message = errorMessage });
            }
        }

        [HttpPost("create")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestObjectResult))]
        public async Task<IActionResult> Create(string keyName, string[] capabilities, string bucketId = "", int? validDurationInSeconds = null, CancellationToken cancelToken = default)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                if (string.IsNullOrWhiteSpace(bucketId))
                {
                    return Ok(await _client.Keys.Create(keyName, capabilities, validDurationInSeconds, cancelToken));
                }
                else
                {
                    return Ok(await _client.Keys.Create(keyName, capabilities, bucketId, validDurationInSeconds, cancelToken));
                }
            }
            catch (Exception ex)
            {
                var errorMessage = ex.ToAllString();
                _logger.Log(LogLevel.Error, errorMessage);
                return BadRequest(new KeyResponse { IsSuccessful = false, Message = errorMessage });
            }
        }

        [HttpDelete("delete")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(BadRequestObjectResult))]
        public async Task<IActionResult> Delete(string applicationKeyId, CancellationToken cancelToken = default)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                return Ok(await _client.Keys.Delete(applicationKeyId, cancelToken));
            }
            catch (Exception ex)
            {
                var errorMessage = ex.ToAllString();
                _logger.Log(LogLevel.Error, errorMessage);
                return BadRequest(new KeyResponse { IsSuccessful = false, Message = errorMessage });
            }
        }
    }
}
