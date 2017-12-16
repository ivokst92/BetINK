namespace BetINK.Services.Implementations.Moderator
{
    using AutoMapper.QueryableExtensions;
    using BetINK.Services.Interfaces.Moderator;
    using BetINK.Services.Models.Match;
    using BetINK.Web.Data;
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using BetINK.DataAccess.Models;

    public class MatchService : IMatchService
    {
        private readonly BetINKDbContext db;

        public MatchService(BetINKDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<MatchListingServiceModel> All(int roundId, string username)
         => this.db.Matches
             .Where(x => x.RoundId == roundId)
             .OrderByDescending(x => x.Id)
             .ProjectTo<MatchListingServiceModel>(new { username = username });

        public IEnumerable<string> AllTeams(int leagueId)
        {
            var teamIds = this.db
                .TeamInLeagues
                .Where(x => x.LeagueId == leagueId)
                .Select(x => x.TeamId);

            return this.db.Teams
                .Where(x => teamIds.Contains(x.Id))
                .Select(x => x.Name);
        }

        public MatchServiceModel ByIdAndRoundId(int matchId, int roundId)
        => this.db.Matches
            .Where(x => x.Id == matchId && x.RoundId == roundId)
            .ProjectTo<MatchServiceModel>()
            .FirstOrDefault();

        public void Create(string homeTeam, string awayTeam, decimal homeWinPoints, decimal drawPoints, decimal awayWinPoints, DateTime matchStart, int roundId, int? leagueId)
        {
            this.db.Matches.Add(new Match
            {
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                HomeWinPoints = homeWinPoints,
                DrawPoints = drawPoints,
                AwayWinPoints = awayWinPoints,
                MatchStart = matchStart,
                RoundId = roundId,
                LeagueId = leagueId
            });
            this.db.SaveChanges();
        }

        public void Edit(int id, string homeTeam, string awayTeam, decimal homeWinPoints, decimal drawPoints, decimal awayWinPoints, DateTime matchStart)
        {

            var match = this.db.Matches.Find(id);

            if (match == null)
            {
                return;
            }

            match.HomeTeam = homeTeam;
            match.AwayTeam = awayTeam;
            match.HomeWinPoints = homeWinPoints;
            match.DrawPoints = drawPoints;
            match.AwayWinPoints = awayWinPoints;
            match.MatchStart = matchStart;

            this.db.SaveChanges();
        }
    }
}
