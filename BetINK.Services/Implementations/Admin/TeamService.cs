namespace BetINK.Services.Implementations.Admin
{
    using BetINK.Services.Interfaces.Admin;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using BetINK.Services.Models.Team;
    using BetINK.Web.Data;
    using System.Linq;
    using AutoMapper.QueryableExtensions;
    using BetINK.DataAccess.Models;

    public class TeamService : ITeamService
    {
        private readonly BetINKDbContext db;

        public TeamService(BetINKDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<TeamListingServiceModel> AllByLeagueId(int leagueId)
        {
            var listTeamIds = this.db.TeamInLeagues
                .Where(x => x.LeagueId == leagueId)
                .Select(x => x.TeamId);

            return this.db
             .Teams
             .Where(x => listTeamIds.Contains(x.Id))
             .ProjectTo<TeamListingServiceModel>()
             .OrderBy(x => x.Id);
        }

        public TeamServiceModel ById(int teamId)
        => this.db.Teams
            .Where(x => x.Id == teamId)
            .ProjectTo<TeamServiceModel>()
            .FirstOrDefault();

        public void Create(string name, string country, string emblemUrl, List<int> leagues)
        {
            Team team = new Team()
            {
                Name = name,
                Country = country,
                EmblemUrl = emblemUrl,
            };
            team.Leagues = new List<FootballLeague>();

            foreach (var leagueId in leagues)
            {
                team.Leagues.Add(new FootballLeague() { LeagueId = leagueId });
            }

            this.db.Add(team);
            this.db.SaveChanges();
        }

        public void Delete(int teamId)
        {
            var team = this.db.Teams.Find(teamId);

            if (team == null)
            {
                return;
            }

            var teamInLeagues = this.db.TeamInLeagues.Where(x => x.TeamId == teamId);


            foreach (var teamLeague in teamInLeagues)
            {
                this.db.TeamInLeagues.Remove(teamLeague);
            }

            this.db.Teams.Remove(team);
            this.db.SaveChanges();
        }

        public void Edit(int teamId, string name, string country, string emblemUrl, List<int> leagues)
        {
            var team = this.db.Teams.Find(teamId);

            if (team == null)
            {
                return;
            }

            team.Name = name;
            team.Country = country;
            team.EmblemUrl = emblemUrl;

            var InDbLeagueIds = this.db.TeamInLeagues.Where(x => x.TeamId == teamId).Select(x => x.LeagueId);

            foreach (var InDbLeagueId in InDbLeagueIds)
            {
                if (!leagues.Contains(InDbLeagueId))
                {
                    var teamInLeague = this.db.TeamInLeagues.Where(x => x.LeagueId == InDbLeagueId && x.TeamId == teamId).First();
                    this.db.TeamInLeagues.Remove(teamInLeague);
                }
            }

            foreach (var leagueToAddId in leagues)
            {
                if (!InDbLeagueIds.Contains(leagueToAddId))
                {
                    team.Leagues.Add(new FootballLeague()
                    {
                        LeagueId = leagueToAddId,
                        TeamId = teamId
                    });
                }
            }

            this.db.SaveChanges();
        }
    }
}
