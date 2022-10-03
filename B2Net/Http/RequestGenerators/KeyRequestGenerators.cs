using B2Net.Http.RequestGenerators;
using B2Net.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace B2Net.Http
{
    public static class KeyRequestGenerators
    {
        private static class Endpoints
        {
            public const string List = "b2_list_keys";
            public const string Create = "b2_create_key";
            public const string Delete = "b2_delete_key";
        }

        public static HttpRequestMessage GetKeyList(B2Options options)
        {
            var json = JsonConvert.SerializeObject(new { accountId = options.AccountId });
            return BaseRequestGenerator.PostRequest(Endpoints.List, json, options);
        }

        public static HttpRequestMessage DeleteKey(B2Options options, string applicationKeyId)
        {
            var json = JsonConvert.SerializeObject(new { applicationKeyId });
            return BaseRequestGenerator.PostRequest(Endpoints.Delete, json, options);
        }

        /// <summary>
        /// Create a key.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="keyName"></param>
        /// <param name="capabilities"></param>
        /// <param name="validDurationInSeconds"></param>
        /// <returns></returns>
        public static HttpRequestMessage CreateKey(B2Options options, string keyName, string[] capabilities, int? validDurationInSeconds = null)
        {
            var keyNameRegex = new Regex("^[0-9A-Za-z-]{2,100}");
            if (!keyNameRegex.IsMatch(keyName))
            {
                throw new Exception(@"The key name specified does not match the requirements. 
                            Names can contain letters, numbers, and "" - "", and are limited to 100 characters.");
            }

            if (!capabilities.Any())
            {
                throw new Exception("The capabilities specified does not match the requirements. Value must be at least one");
            }

            var durationRegex = new Regex("^[1-9]+[0-9]*$");
            if (validDurationInSeconds != null && (!durationRegex.IsMatch(validDurationInSeconds.ToString()) || validDurationInSeconds > 86400000))
            {
                throw new Exception(@"The valid duration in seconds specified does not match the requirements. 
                            Value must be a positive integer, and must be less than 1000 days (in seconds)");
            }

            var body = new B2KeyCreateModel()
            {
                accountId = options.AccountId,
                capabilities = capabilities,
                keyName = keyName,
                validDurationInSeconds = validDurationInSeconds
            };

            var json = JsonSerialize(body);
            return BaseRequestGenerator.PostRequest(Endpoints.Create, json, options);
        }

        /// <summary>
        /// Create a key.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="keyName"></param>
        /// <param name="capabilities"></param>
        /// <param name="validDurationInSeconds"></param>
        /// <param name="bucketId"></param>
        /// <returns></returns>
        public static HttpRequestMessage CreateKey(B2Options options, string keyName, string[] capabilities, string bucketId, int? validDurationInSeconds = null)
        {
            var keyNameRegex = new Regex("^[0-9A-Za-z-]{2,100}");
            if (!keyNameRegex.IsMatch(keyName))
            {
                throw new Exception(@"The key name specified does not match the requirements. 
                            Names can contain letters, numbers, and "" - "", and are limited to 100 characters.");
            }

            if (!capabilities.Any())
            {
                throw new Exception("The capabilities specified does not match the requirements. Value must be at least one");
            }

            var durationRegex = new Regex("^[1-9]+[0-9]*$");
            if (validDurationInSeconds != null && (!durationRegex.IsMatch(validDurationInSeconds.ToString()) || validDurationInSeconds > 86400000))
            {
                throw new Exception(@"The valid duration in seconds specified does not match the requirements. 
                            Value must be a positive integer, and must be less than 1000 days (in seconds)");
            }

            if (string.IsNullOrWhiteSpace(bucketId))
            {
                throw new Exception("The BucketId must be requir");
            }

            var body = new B2KeyCreateModel()
            {
                accountId = options.AccountId,
                capabilities = capabilities,
                keyName = keyName,
                validDurationInSeconds = validDurationInSeconds,
                bucketId = bucketId
            };

            var json = JsonSerialize(body);
            return BaseRequestGenerator.PostRequest(Endpoints.Create, json, options);
        }

        private static string JsonSerialize(object data)
        {
            return JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
    }

    internal class B2KeyCreateModel
    {
        public string accountId { get; set; }
        public string[] capabilities { get; set; }
        public string keyName { get; set; }
        public int? validDurationInSeconds { get; set; }
        public string bucketId { get; set; }
    }
}
