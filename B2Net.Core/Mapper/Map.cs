using B2Net.Core.Models;
using B2Net.Models;
using System.Collections.Generic;
using System.Linq;

namespace B2Net.Core.Mapper
{
    public static class Map
    {
        #region Files Model Mapper
        
        public static FileResponse FileResponse(B2File b2File)
        {
            return new FileResponse
            {
                Action = b2File.Action,
                ContentLength = b2File.ContentLength,
                ContentSHA1 = b2File.ContentSHA1,
                ContentType = b2File.ContentType,
                FileData = b2File.FileData,
                FileId = b2File.FileId,
                FileInfo = b2File.FileInfo,
                FileName = b2File.FileName,
                Size = b2File.Size,
                UploadTimestamp = b2File.UploadTimestamp,
                UploadTimestampDate = b2File.UploadTimestampDate,
                IsSuccessful = true
            };
        }

        public static FilesResponse FilesResponse(B2FileList b2FileList)
        {
            return new FilesResponse
            {
                Files = new List<FileResponse>(b2FileList.Files.Select(FileResponse)),
                IsSuccessful = true
            };
        }

        #endregion End Files Model Mapper

        #region Keys Model Mapper

        public static KeyResponse KeyResponse(B2Key b2Key)
        {
            return new KeyResponse
            {
                AccountId = b2Key.AccountId,
                ApplicationKey = b2Key.ApplicationKey,
                ApplicationKeyId = b2Key.ApplicationKeyId,
                BucketId = b2Key.BucketId,
                Capabilities = b2Key.Capabilities,
                KeyName = b2Key.KeyName,
                UploadTimestampDate = b2Key.UploadTimestampDate,
                IsSuccessful = true,
            };
        }

        public static KeysResponse KeysResponse(List<B2Key> keys)
        {
            return new KeysResponse
            {
                Keys = new List<KeyResponse>(keys.Select(KeyResponse)),
                IsSuccessful = true
            };
        }

        #endregion End Keys Model Mapper

        #region Buckets Model Mapper

        public static BucketResponse BucketResponse(B2Bucket b2Bucket)
        {
            return new BucketResponse
            {
                BucketId = b2Bucket.BucketId,
                BucketName = b2Bucket.BucketName,
                BucketType = b2Bucket.BucketType,
                Revision = b2Bucket.Revision,
                BucketInfo = b2Bucket.BucketInfo,
                CORSRules = b2Bucket.CORSRules.Any() ? new List<CORSRuleModel>(b2Bucket.CORSRules.Select(CORSRuleModel)) : null,
                LifecycleRules = b2Bucket.LifecycleRules.Any() ? new List<BucketLifecycleRuleModel>(b2Bucket.LifecycleRules.Select(BucketLifecycleRuleModel)) : null,
                IsSuccessful = true
            };
        }

        public static BucketsResponse BucketsResponse(List<B2Bucket> b2Buckets)
        {
            return new BucketsResponse
            {
                Buckets = new List<BucketResponse>(b2Buckets.Select(BucketResponse)),
                IsSuccessful = true
            };
        }

        #endregion End Buckets Model Mapper

        #region CORS Model Mapper

        public static CORSRuleModel CORSRuleModel(B2CORSRule b2CORSRule)
        {
            return new CORSRuleModel
            {
                AllowedHeaders = b2CORSRule.AllowedHeaders,
                AllowedOperations = b2CORSRule.AllowedOperations,
                AllowedOrigins = b2CORSRule.AllowedOrigins,
                CorsRuleName = b2CORSRule.CorsRuleName,
                ExposeHeaders = b2CORSRule.ExposeHeaders,
                MaxAgeSeconds = b2CORSRule.MaxAgeSeconds
            };
        }

        #endregion End CORS Model Mapper

        #region BucketLifecycleRule Model Mapper

        public static BucketLifecycleRuleModel BucketLifecycleRuleModel(B2BucketLifecycleRule b2BucketLifecycleRule)
        {
            return new BucketLifecycleRuleModel
            {
                DaysFromHidingToDeleting = b2BucketLifecycleRule.DaysFromHidingToDeleting,
                DaysFromUploadingToHiding = b2BucketLifecycleRule.DaysFromUploadingToHiding,
                FileNamePrefix = b2BucketLifecycleRule.FileNamePrefix
            };
        }

        #endregion End BucketLifecycleRule Model Mapper
    }
}
