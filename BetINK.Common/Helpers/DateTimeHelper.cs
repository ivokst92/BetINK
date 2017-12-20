namespace BetINK.Common.Helpers
{
    using System;

    public static class DateTimeHelper
    {
        private const string TimeZone = "FLE Standard Time";

        public static DateTime ToFLEStandartTime(this DateTime date)
        {
            DateTime serverTime = date;
            DateTime _localTime =
                TimeZoneInfo.ConvertTimeBySystemTimeZoneId(serverTime,
                TimeZoneInfo.Local.Id, TimeZone);
            return _localTime; ;
        }
    }


}
