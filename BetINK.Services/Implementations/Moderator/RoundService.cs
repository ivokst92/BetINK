namespace BetINK.Services.Implementations.Moderator
{
    using System.Collections.Generic;
    using BetINK.Services.Interfaces.Moderator;
    using BetINK.Services.Models.Round;
    using BetINK.Web.Data;
    using System.Linq;
    using AutoMapper.QueryableExtensions;
    using BetINK.DataAccess.Models;
    using System;
    using BetINK.Common.Helpers;

    public class RoundService : IRoundService
    {
        private readonly BetINKDbContext db;

        public RoundService(BetINKDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<RoundListingServiceModel> All(string username)
        {
            var season = this.db.Seasons
                        .Where(x => x.IsActive == true)
                        .FirstOrDefault();

            if (season == null)
            {
                return null;
            }

            return this.db.Rounds
                 .OrderByDescending(x => x.Id)
                 .Where(x => x.SeasonId == season.Id)
                 .ProjectTo<RoundListingServiceModel>(new { username = username });
        }

        public RoundServiceModel ById(int id)
        => this.db.Rounds
            .Where(x => x.Id == id)
            .ProjectTo<RoundServiceModel>()
            .FirstOrDefault();

        public void Create(int number, string description, string username)
        {
            var activeSeasonId = this.db.Seasons.Where(x => x.IsActive == true).Select(x => x.Id).First();
            this.db.Rounds.Add(new Round
            {
                Number = number,
                Description = description,
                CreatedBy = username,
                CreatedOn = DateTime.Now.ToFLEStandartTime(),
                SeasonId = activeSeasonId
            });
            this.db.SaveChanges();
        }

        public void Edit(int id, int number, string description, string username)
        {
            var round = this.db.Rounds.Find(id);

            if (round == null)
            {
                return;
            }

            round.Number = number;
            round.Description = description;
            round.ModifiedBy = username;
            round.ModifiedOn = DateTime.Now.ToFLEStandartTime();

            this.db.SaveChanges();
        }

        public bool IsUserAllowedToEdit(int roundId, string username)
        {
            var user = this.db.Users.Where(x => x.UserName == username).FirstOrDefault();

            if (user == null)
                return false;

            return this.db.Rounds.Where(x => x.CreatedBy == username
                 && (x.ModifiedBy == null || x.ModifiedBy == username) && x.Id == roundId).Any();
        }

        public bool IsUserAllowedToEdit(int matchId, int roundId, string name)
        {
            bool isAllowed = IsUserAllowedToEdit(roundId, name);

            if (!isAllowed)
            {
                return isAllowed;
            }

            return this.db.Matches
                .Where(x => x.Id == matchId
                && x.RoundId == roundId)
                .Any();
        }
    }
}
