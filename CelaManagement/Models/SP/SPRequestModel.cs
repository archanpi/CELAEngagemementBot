using CelaEngagement.Common;

namespace CelaEngagement.Models.SP
{
    public class SPRequestModel
    {
        public string SearchQuery { get; set; }
        public Rating Rating { get; set; }
        public string OffSet { get; set; }
        public string SPUrl { get; set; }
        public string ApiKey { get; set; }
    }
}
