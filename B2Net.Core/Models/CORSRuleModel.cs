namespace B2Net.Core.Models
{
    public class CORSRuleModel
    {
		public string CorsRuleName { get; set; }
		public string[] AllowedOrigins { get; set; }
		public string[] AllowedOperations { get; set; }
		public string[] AllowedHeaders { get; set; }
		public string[] ExposeHeaders { get; set; }
		public int MaxAgeSeconds { get; set; }
	}
}
