using System.Collections.Generic;
using Newtonsoft.Json;

namespace CelaEngagement.Models.SP
{
	public class SPSearchResponse
	{
		[JsonProperty("data")]
		public List<Datum> Data { get; set; }
		[JsonProperty("pagination")]
		public Pagination Pagination { get; set; }
		[JsonProperty("meta")]
		public Meta Meta { get; set; }
	}
}
