namespace BetINK.Web.Infrastructure.Extensions
{
    using BetINK.Common.Enums;
    using BetINK.Common.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumExtensions
    {
        public static string ToDisplayName(this ResultEnum result)
        {
            return result.GetResult();
        }

        public static List<T> GetResultData<T>(this Enum enumeration) where T : struct
        {
            var enumData = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            var element = enumData[1];
            enumData[1] = enumData[0];
            enumData[0] = element;
            return enumData;
        }
    }
}
