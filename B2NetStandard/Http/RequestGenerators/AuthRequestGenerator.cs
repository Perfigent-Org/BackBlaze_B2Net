﻿using B2NetStandard.Models;
using System;
using System.Net.Http;

namespace B2NetStandard.Http.RequestGenerators
{
    public static class AuthRequestGenerator
	{
		private static class Endpoints
		{
			public const string Auth = "b2_authorize_account";
		}

		public static HttpRequestMessage Authorize(B2Options options)
		{
			var uri = new Uri(Constants.ApiBaseUrl + "/" + Constants.Version + "/" + Endpoints.Auth);
			var request = new HttpRequestMessage()
			{
				Method = HttpMethod.Get,
				RequestUri = uri
			};

			request.Headers.Add("Authorization", Utilities.CreateAuthorizationHeader(options.KeyId, options.ApplicationKey));

			return request;
		}
	}
}
