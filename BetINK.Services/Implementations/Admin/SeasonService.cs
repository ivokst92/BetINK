namespace BetINK.Services.Implementations.Admin
{
    using AutoMapper.QueryableExtensions;
    using BetINK.DataAccess.Models;
    using BetINK.Services.Interfaces.Admin;
    using BetINK.Services.Models.Season;
    using BetINK.Web.Data;
    using System.Collections.Generic;
    using System.Linq;

    public class SeasonService : ISeasonService
    {
        private readonly BetINKDbContext db;

        public SeasonService(BetINKDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<SeasonListingServiceModel> All()
         => this.db
            .Seasons
            .OrderBy(x => x.Id)
            .ProjectTo<SeasonListingServiceModel>();

        public bool AnyActiveSeason()
        => this.db.Seasons.Any(x => x.IsActive == true);

        public SeasonServiceModel ById(int id)
         => this.db.Seasons
            .Where(x => x.Id == id)
            .ProjectTo<SeasonServiceModel>()
            .FirstOrDefault();

        public void Create(string description, string name)
        {
            int count = this.db.Seasons.Count();
            this.db.Seasons.Add(new Season
            {
                IsActive = count == 0 ? true : false,
                Name = name,
                Description = description
            });
            this.db.SaveChanges();
        }

        public void Delete(int id)
        {
            var season = this.db.Seasons.Find(id);

            if (season == null)
            {
                return;
            }

            this.db.Seasons.Remove(season);
            this.db.SaveChanges();
        }

        public void Edit(int id, string description, string name)
        {
            var season = this.db.Seasons.Find(id);
            season.Name = name;
            season.Description = description;

            this.db.SaveChanges();
        }

        public bool Exists(int id)
            => this.db.Seasons.Any(x => x.Id == id);

        public bool AnyRoundsIn(int id)
        => this.db
            .Seasons
            .Where(x => x.Id == id && x.Rounds.Count > 0)
            .Any();

        public void SetAsActiveSeason(int id)
        {
            var season = this.db.Seasons.Find(id);
            if (season == null)
            {
                return;
            }

            var seasons = this.db.Seasons
                    .Where(x => x.IsActive == true);

            foreach (var seson in seasons)
            {
                seson.IsActive = false;
            }

            season.IsActive = true;
            this.db.SaveChanges();
        }
    }
}
