namespace BetINK.Services.Implementations.Admin
{
    using System.Collections.Generic;
    using BetINK.Services.Interfaces.Admin;
    using BetINK.Services.Models.League;
    using BetINK.Web.Data;
    using System.Linq;
    using AutoMapper.QueryableExtensions;
    using DataAccess.Models;

    public class LeagueService : ILeagueService
    {
        private readonly BetINKDbContext db;

        public LeagueService(BetINKDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<LeagueListingServiceModel> All()
        => this.db
            .Leagues
            .OrderBy(x => x.Id)
            .ProjectTo<LeagueListingServiceModel>();

        public LeagueServiceModel ById(int id)
         => this.db.Leagues
            .Where(x => x.Id == id)
            .ProjectTo<LeagueServiceModel>()
            .FirstOrDefault();

        public void Create(string name, string country, string emblemUrl)
        {
            this.db.Leagues.Add(new League
            {
                Name = name,
                Country = country,
                EmblemUrl = emblemUrl
            });
            this.db.SaveChanges();
        }

        public void Delete(int id)
        {
            var league = this.db.Leagues.Find(id);

            if (league == null)
            {
                return;
            }

            this.db.Leagues.Remove(league);
            this.db.SaveChanges();
        }

        public void Edit(int id, string name, string country, string emblemUrl)
        {
            var league = this.db.Leagues.Find(id);

            if (league == null)
            {
                return;
            }

            league.Name = name;
            league.Country = country;
            league.EmblemUrl = emblemUrl;

            this.db.SaveChanges();
        }

        public bool Exists(int leagueId)
        => this.db
            .Leagues
            .Any(x => x.Id == leagueId);

        public string GetLeagueNameById(int id)
        => this.db.Leagues
            .Where(x => x.Id == id)
            .Select(x => x.Name)
            .FirstOrDefault();

        public IEnumerable<SelectItemLeague> AllLeagues()
        => this.db
            .Leagues
            .OrderBy(x => x.Id)
            .ProjectTo<SelectItemLeague>();
    }
}
