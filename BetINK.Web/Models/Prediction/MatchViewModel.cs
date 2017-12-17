namespace BetINK.Web.Models.Prediction
{
    using BetINK.Common.Enums;
    using BetINK.Common.Mapping;
    using BetINK.Services.Models.Match;

    public class MatchViewModel : IMapFrom<MatchServiceModel>
    {
        public int Id { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string Result { get; set; }
        public decimal HomeWinPoints { get; set; }
        public decimal DrawPoints { get; set; }
        public decimal AwayWinPoints { get; set; }
        public ResultEnum? UserPrediction { get; set; }
    }
}
