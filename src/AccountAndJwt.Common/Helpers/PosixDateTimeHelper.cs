using AccountAndJwt.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AccountAndJwt.Common.Helpers
{
	public static class PosixDateTimeHelper
	{
		private static readonly DateTime _posixZeroPoint;


		static PosixDateTimeHelper()
		{
			_posixZeroPoint = new DateTime(1970, 1, 1);
		}


		// FUNCTIONS //////////////////////////////////////////////////////////////////////////////
		public static Int64 ToPosixTimeMs(this DateTime date)
		{
			if(date < _posixZeroPoint)
				throw new PosixDateTimeException($"Provided date {date} less than {_posixZeroPoint}!");

			return (Int64)date.Subtract(_posixZeroPoint).TotalMilliseconds;
		}
		public static Int64 ToPosixTimeSec(this DateTime date)
		{
			if(date < _posixZeroPoint)
				throw new PosixDateTimeException($"Provided date {date} less than {_posixZeroPoint}!");

			return (Int64)date.Subtract(_posixZeroPoint).TotalSeconds;
		}
		public static string ToIso8601(this DateTime date)
		{
			return date.ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz", DateTimeFormatInfo.InvariantInfo);
		}

		public static DateTime FromPosixTimeMs(this Int64 posixTime)
		{
			return _posixZeroPoint.Add(TimeSpan.FromMilliseconds(posixTime));
		}
		public static DateTime FromPosixTimeSec(this Int64 posixTime)
		{
			return _posixZeroPoint.Add(TimeSpan.FromSeconds(posixTime));
		}

		public static Int64 DaysToMs(this Int32 days)
		{
			return (Int64)TimeSpan.FromDays(days).TotalMilliseconds;
		}
		public static Int32 DaysToSec(this Int32 days)
		{
			return (Int32)TimeSpan.FromDays(days).TotalSeconds;
		}

		public static IEnumerable<DateTime> RangeByDay(this DateTime startDate, DateTime endDate)
		{
			return Enumerable.Range(0, (endDate - startDate).Days + 1).Select(d => startDate.AddDays(d));
		}
	}
}