namespace BetINK.Web.Models.Prediction
{
    public class MatchViewModel
    {
        public int Id { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string Result { get; set; }
        public decimal HomeWinPoints { get; set; }
        public decimal DrawPoints { get; set; }
        public decimal AwayWinPoints { get; set; }
        public string UserPrediction { get; set; }
        public bool IsPredictionAllowed { get; set; }
    }
}
