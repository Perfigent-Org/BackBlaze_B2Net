namespace B2Net.Core.Models
{
    public class BucketLifecycleRuleModel
    {
        public int? DaysFromHidingToDeleting { get; set; }
        public int? DaysFromUploadingToHiding { get; set; }
        public string FileNamePrefix { get; set; }
    }
}
