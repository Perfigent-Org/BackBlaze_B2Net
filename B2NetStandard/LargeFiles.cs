﻿using B2NetStandard.Http;
using B2NetStandard.Http.RequestGenerators;
using B2NetStandard.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace B2NetStandard
{
    public class LargeFiles : ILargeFiles
	{
		private B2Options _options;
		private HttpClient _client;
		private string _api = "Large Files";

		public LargeFiles(B2Options options)
		{
			_options = options;
			_client = HttpClientFactory.CreateHttpClient(options.RequestTimeout);
		}

		/// <summary>
		/// Starts a large file upload.
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="bucketId"></param>
		/// <param name="cancelToken"></param>
		/// <returns></returns>
		public async Task<B2File> StartLargeFile(string fileName, string contentType = "", string bucketId = "", Dictionary<string, string> fileInfo = null, CancellationToken cancelToken = default(CancellationToken))
		{
			var operationalBucketId = Utilities.DetermineBucketId(_options, bucketId);

			var request = LargeFileRequestGenerators.Start(_options, operationalBucketId, fileName, contentType, fileInfo);

			// Send the download request
			var response = await _client.SendAsync(request, cancelToken);

			// Create B2File from response
			return await ResponseParser.ParseResponse<B2File>(response, _api);
		}

		/// <summary>
		/// Get an upload url for use with one Thread.
		/// </summary>
		/// <param name="cancelToken"></param>
		/// <returns></returns>
		public async Task<B2UploadPartUrl> GetUploadPartUrl(string fileId, CancellationToken cancelToken = default(CancellationToken))
		{
			var request = LargeFileRequestGenerators.GetUploadPartUrl(_options, fileId);

			var uploadUrlResponse = await _client.SendAsync(request, cancelToken);

			var uploadUrl = await ResponseParser.ParseResponse<B2UploadPartUrl>(uploadUrlResponse, _api);

			return uploadUrl;
		}

		/// <summary>
		/// Upload one part of an already started large file upload.
		/// </summary>
		/// <param name="fileData"></param>
		/// <param name="cancelToken"></param>
		/// <returns></returns>
		public async Task<B2UploadPart> UploadPart(byte[] fileData, int partNumber, B2UploadPartUrl uploadPartUrl, CancellationToken cancelToken = default(CancellationToken))
		{
			var request = LargeFileRequestGenerators.Upload(fileData, partNumber, uploadPartUrl);

			var response = await _client.SendAsync(request, cancelToken);

			return await ResponseParser.ParseResponse<B2UploadPart>(response, _api);
		}

		/// <summary>
		/// Downloads one file by providing the name of the bucket and the name of the file.
		/// </summary>
		/// <param name="fileId"></param>
		/// <param name="cancelToken"></param>
		/// <returns></returns>
		public async Task<B2File> FinishLargeFile(string fileId, string[] partSHA1Array, CancellationToken cancelToken = default(CancellationToken))
		{
			var request = LargeFileRequestGenerators.Finish(_options, fileId, partSHA1Array);

			// Send the request
			var response = await _client.SendAsync(request, cancelToken);

			// Create B2File from response
			return await ResponseParser.ParseResponse<B2File>(response, _api);
		}

		/// <summary>
		/// Cancel a large file upload
		/// </summary>
		/// <param name="fileId"></param>
		/// <param name="cancelToken"></param>
		/// <returns></returns>
		public async Task<B2CancelledFile> CancelLargeFile(string fileId, CancellationToken cancelToken = default(CancellationToken))
		{
			var request = LargeFileRequestGenerators.Cancel(_options, fileId);

			// Send the request
			var response = await _client.SendAsync(request, cancelToken);

			// Create B2File from response
			return await ResponseParser.ParseResponse<B2CancelledFile>(response, _api);
		}
	}
}
