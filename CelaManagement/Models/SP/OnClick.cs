using System;
using Newtonsoft.Json;

namespace CelaEngagement.Models.SP
{
	public class Onclick
	{
		[JsonProperty("url")]
		public Uri Url { get; set; }
	}
}
