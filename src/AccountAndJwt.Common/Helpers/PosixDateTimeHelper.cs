using AccountAndJwt.Common.Exceptions;
using System;

namespace AccountAndJwt.Common.Helpers
{
	public static class PosixDateTimeHelper
	{
		private static readonly DateTime _posixZeroPoint;


		static PosixDateTimeHelper()
		{
			_posixZeroPoint = new DateTime(1970, 1, 1);
		}


		// PROPERTIES /////////////////////////////////////////////////////////////////////////////
		public static DateTime Epoch => _posixZeroPoint;


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
	}
}