namespace BetINK.Services.Implementations.UserInteractions
{
    using BetINK.Common.Enums;
    using BetINK.Services.Interfaces.UserInteractions;
    using BetINK.Services.Models.Chart;
    using BetINK.Web.Data;
    using System.Collections.Generic;
    using System.Linq;

    public class ChartService : IChartService
    {
        private readonly BetINKDbContext db;

        public ChartService(BetINKDbContext db)
        {
            this.db = db;
        }

        public Dictionary<string, string> GetLeagues()
        => new Dictionary<string, string>()
            {
            { "premier-league","Premier League" },
            {"bundesliga","Bundesliga" },
            {"serie-a", "Serie A" },
            {"liga","La Liga" },
            {"ligue1","Ligue 1" },
            {"eredivisie","Eredivisie" },
            };

        public IEnumerable<ChartServiceModel> GetUsersChart()
        {
            return (from u in db.Users
                    join p in db.Predictions on u.Id equals p.UserId into dp
                    select new ChartServiceModel
                    {
                        Username = u.UserName,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Points = dp.Sum(x =>
                        x.MatchPrediction == ResultEnum.HOME_WIN && x.Match.Result == x.MatchPrediction ?
                        x.Match.HomeWinPoints :
                        x.MatchPrediction == ResultEnum.DRAW && x.Match.Result == x.MatchPrediction ?
                        x.Match.DrawPoints :
                        x.MatchPrediction == ResultEnum.AWAY_WIN && x.Match.Result == x.MatchPrediction ?
                        x.Match.AwayWinPoints : 0
                        )
                    })
                    .OrderByDescending(x => x.Points)
                    .ToList();
        }


    }

}
