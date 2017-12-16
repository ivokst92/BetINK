namespace BetINK.Services.Implementations.Admin
{
    using System.Collections.Generic;
    using BetINK.Services.Interfaces.Admin;
    using BetINK.Services.Models.Match;
    using BetINK.Web.Data;
    using System.Linq;
    using AutoMapper.QueryableExtensions;
    using System;
    using BetINK.DataAccess.Models;
    using BetINK.Common.Enums;

    public class MatchAdminService : IMatchAdminService
    {
        private readonly BetINKDbContext db;

        public MatchAdminService(BetINKDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<MatchAdminListingServiceModel> All(int roundId)
        => this.db.Matches
            .OrderByDescending(x => x.Id)
            .Where(x => x.RoundId == roundId)
            .ProjectTo<MatchAdminListingServiceModel>();

        public MatchAdminServiceModel ByIdAndRoundId(int matchId, int roundId)
        => this.db.Matches
            .Where(x => x.Id == matchId && x.RoundId == roundId)
            .ProjectTo<MatchAdminServiceModel>()
            .FirstOrDefault();


        public void Create(string homeTeam,
                           string awayTeam,
                           decimal homeWinPoints,
                           decimal awayWinPoints,
                           decimal drawPoints,
                           DateTime matchStart,
                           ResultEnum? result,
                           int roundId,
                           int? leagueId)
        {
            Match match = new Match()
            {
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                HomeWinPoints = homeWinPoints,
                AwayWinPoints = awayWinPoints,
                DrawPoints = drawPoints,
                MatchStart = matchStart,
                RoundId = roundId,
                LeagueId = leagueId,
                Result = result
            };

            this.db.Matches.Add(match);
            this.db.SaveChanges();
        }

        public void Delete(int id)
        {
            var match = this.db.Matches.Find(id);

            if (match == null)
            {
                return;
            }

            this.db.Matches.Remove(match);
            this.db.SaveChanges();
        }

        public void Edit(int id,
                         string homeTeam,
                         string awayTeam,
                         decimal homeWinPoints,
                         decimal awayWinPoints,
                         decimal drawPoints,
                         DateTime matchStart,
                         ResultEnum? result)
        {
            var match = this.db.Matches.Find(id);

            if (match == null)
            {
                return;
            }

            match.HomeTeam = homeTeam;
            match.AwayTeam = awayTeam;
            match.HomeWinPoints = homeWinPoints;
            match.AwayWinPoints = awayWinPoints;
            match.DrawPoints = drawPoints;
            match.MatchStart = matchStart;
            match.Result = result;

            this.db.SaveChanges();
        }

        public bool RoundExists(int roundId)
          => this.db.Rounds.Any(x => x.Id == roundId);
    }
}
