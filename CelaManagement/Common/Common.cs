﻿

namespace CelaEngagement.Common
{
	public static class Common
	{
		public static string ConvertRatingToString(Rating rating)
		{
			switch (rating)
			{
				case Rating.G:
					return "g";
				case Rating.PG:
					return "pg";
				case Rating.PG13:
					return "pg-13";
				case Rating.R:
					return "r";
				default:
					return "g";
			}
		}
	}

	public enum Rating
	{
		G,
		PG,
		PG13,
		R
	}
}
