namespace BetINK.Common.Helpers
{
    using BetINK.Common.Enums;

    public static class ResultHelper
    {
        public const string HOME_WIN = "1";
        public const string DRAW = "X";
        public const string AWAY_WIN = "2";

        public static string GetNullableResult(this ResultEnum? result)
        {
            switch (result)
            {
                case ResultEnum.HOME_WIN:
                    return HOME_WIN;
                case ResultEnum.DRAW:
                    return DRAW;
                case ResultEnum.AWAY_WIN:
                    return AWAY_WIN;
                default:
                    return null;
            }
        }

        public static string GetResult(this ResultEnum result)
        {
            switch (result)
            {
                case ResultEnum.HOME_WIN:
                    return HOME_WIN;
                case ResultEnum.DRAW:
                    return DRAW;
                case ResultEnum.AWAY_WIN:
                    return AWAY_WIN;
                default:
                    return result.ToString();
            }
        }
    }
}
