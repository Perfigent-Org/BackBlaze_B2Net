using B2NetStandard.Http;
using B2NetStandard.Http.RequestGenerators;
using B2NetStandard.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace B2NetStandard
{
    public class Files : IFiles
	{
		private B2Options _options;
		private HttpClient _client;
		private string _api = "Files";

		public Files(B2Options options)
		{
			_options = options;
			_client = HttpClientFactory.CreateHttpClient(options.RequestTimeout);
		}

		/// <summary>
		/// DEPRECATED: This method has been deprecated in favor of the Upload that takes an UploadUrl parameter.
		/// The other Upload method is the preferred, and more efficient way, of uploading to B2.
		/// </summary>
		/// <param name="fileData"></param>
		/// <param name="fileName"></param>
		/// <param name="bucketId"></param>
		/// <param name="cancelToken"></param>
		/// <returns></returns>
		public async Task<B2File> Upload(byte[] fileData, string fileName, string bucketId = "", Dictionary<string, string> fileInfo = null, CancellationToken cancelToken = default(CancellationToken))
		{
			var operationalBucketId = Utilities.DetermineBucketId(_options, bucketId);

			// Get the upload url for this file
			var uploadUrlRequest = FileUploadRequestGenerators.GetUploadUrl(_options, operationalBucketId);
			var uploadUrlResponse = await _client.SendAsync(uploadUrlRequest, cancelToken);
			var uploadUrlData = await uploadUrlResponse.Content.ReadAsStringAsync();
			var uploadUrlObject = JsonConvert.DeserializeObject<B2UploadUrl>(uploadUrlData);
			// Set the upload auth token
			_options.UploadAuthorizationToken = uploadUrlObject.AuthorizationToken;

			// Now we can upload the file
			var requestMessage = FileUploadRequestGenerators.Upload(_options, uploadUrlObject.UploadUrl, fileData, fileName, fileInfo);
			var response = await _client.SendAsync(requestMessage, cancelToken);

			return await ResponseParser.ParseResponse<B2File>(response, _api);
		}

		/// <summary>
		/// Downloads one file from B2.
		/// </summary>
		/// <param name="fileId"></param>
		/// <param name="cancelToken"></param>
		/// <returns></returns>
		public async Task<B2File> DownloadById(string fileId, CancellationToken cancelToken = default(CancellationToken))
		{
			// Are we searching by name or id?
			HttpRequestMessage request = FileDownloadRequestGenerators.DownloadById(_options, fileId);

			// Send the download request
			var response = await _client.SendAsync(request, cancelToken);

			// Create B2File from response
			return await ParseDownloadResponse(response);
		}

		private async Task<B2File> ParseDownloadResponse(HttpResponseMessage response)
		{
			await Utilities.CheckForErrors(response, _api);

			var file = new B2File();
			IEnumerable<string> values;
			if (response.Headers.TryGetValues("X-Bz-Content-Sha1", out values))
			{
				file.ContentSHA1 = values.First();
			}
			if (response.Headers.TryGetValues("X-Bz-File-Name", out values))
			{
				file.FileName = values.First();
				// Decode file name
				file.FileName = file.FileName.b2UrlDecode();
			}
			if (response.Headers.TryGetValues("X-Bz-File-Id", out values))
			{
				file.FileId = values.First();
			}
			// File Info Headers
			var fileInfoHeaders = response.Headers.Where(h => h.Key.ToLower().Contains("x-bz-info"));
			var infoData = new Dictionary<string, string>();
			if (fileInfoHeaders.Count() > 0)
			{
				foreach (var fileInfo in fileInfoHeaders)
				{
					// Substring to parse out the file info prefix.
					infoData.Add(fileInfo.Key.Substring(10), fileInfo.Value.First());
				}
			}
			file.FileInfo = infoData;
			if (response.Content.Headers.ContentLength.HasValue)
			{
				file.Size = response.Content.Headers.ContentLength.Value;
			}
			file.FileData = await response.Content.ReadAsByteArrayAsync();

			return await Task.FromResult(file);
		}
	}
}
