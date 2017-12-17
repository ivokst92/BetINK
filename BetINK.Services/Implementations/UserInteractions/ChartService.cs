namespace BetINK.Services.Implementations.UserInteractions
{
    using BetINK.Common.Enums;
    using BetINK.DataAccess.Models;
    using BetINK.Services.Interfaces.UserInteractions;
    using BetINK.Services.Models.Chart;
    using BetINK.Web.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class ChartService : IChartService
    {
        private readonly BetINKDbContext db;

        public ChartService(BetINKDbContext db)
        {
            this.db = db;
        }

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
                    .OrderByDescending(x=>x.Points)
                    .ToList();
        }


    }

}
