using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoiGiangApp.Helpers
{
	public static class TimeExtensions
	{
		public static bool OverlapsWith(this string time1, string time2)
		{
			var (bd1, kt1) = time1.ParseTimeRange();
			var (bd2, kt2) = time2.ParseTimeRange();
			return bd1 < kt2 && bd2 < kt1;
		}

		public static (int start, int end) ParseTimeRange(this string time)
		{
			var parts = time.Split('-').Select(t => t.Trim()).ToArray();
			return (TimeToMinutes(parts[0]), TimeToMinutes(parts[1]));
		}

		private static int TimeToMinutes(string time)
		{
			var p = time.Split(':');
			return int.Parse(p[0]) * 60 + (p.Length > 1 ? int.Parse(p[1]) : 0);
		}
	}
}