namespace BetINK.Services.Implementations.Admin
{
    using System.Collections.Generic;
    using BetINK.Services.Interfaces.Admin;
    using BetINK.Services.Models.Round;
    using BetINK.Web.Data;
    using System.Linq;
    using AutoMapper.QueryableExtensions;
    using System;
    using BetINK.DataAccess.Models;
    using Microsoft.AspNetCore.Identity;
    using BetINK.Common.Constants;
    using System.Threading.Tasks;

    public class RoundAdminService : IRoundAdminService
    {
        private readonly BetINKDbContext db;

        private readonly UserManager<User> userManager;

        public RoundAdminService(BetINKDbContext db,
            UserManager<User> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<RoundAdminListingServiceModel>> All()
        {
            var season = this.db.Seasons
                      .Where(x => x.IsActive == true)
                      .FirstOrDefault();

            if (season == null)
            {
                return null;
            }
            var users = await userManager.GetUsersInRoleAsync(WebConstants.ModeratorRole);
            List<string> moderators = new List<string>();
            foreach (var user in users)
            {
                moderators.Add(user.UserName);
            }

            return this.db.Rounds
                 .OrderByDescending(x => x.Id)
                 .Where(x => x.SeasonId == season.Id)
                 .ProjectTo<RoundAdminListingServiceModel>(new { moderators = moderators });
        }

        public async Task<IEnumerable<RoundAdminListingServiceModel>> AllBySeason(int seasonId)
        {
            var users = await userManager.GetUsersInRoleAsync(WebConstants.ModeratorRole);
            List<string> moderators = new List<string>();
            foreach (var user in users)
            {
                moderators.Add(user.UserName);
            }
            
            return this.db.Rounds
                  .OrderByDescending(x => x.Id)
                  .Where(x => x.SeasonId == seasonId)
                  .ProjectTo<RoundAdminListingServiceModel>(new { moderators = moderators });
        }

        public RoundServiceModel ById(int id)
        => this.db.Rounds
                  .Where(x => x.Id == id)
                  .ProjectTo<RoundServiceModel>()
                   .FirstOrDefault();

        public void Create(int seasonId, int number, string description, string username)
        {
            var season = this.db.Seasons.Where(x => x.Id == seasonId).FirstOrDefault();
            if (season == null)
            {
                return;
            }

            this.db.Rounds.Add(new Round
            {
                Number = number,
                Description = description,
                CreatedBy = username,
                CreatedOn = DateTime.Now,
                SeasonId = season.Id
            });

            this.db.SaveChanges();
        }

        public int Edit(int id, int number, string description, string username)
        {
            var round = this.db.Rounds.Find(id);

            if (round == null)
            {
                return default(int);
            }

            round.Number = number;
            round.Description = description;
            round.ModifiedBy = username;
            round.ModifiedOn = DateTime.Now;

            this.db.SaveChanges();

            return round.SeasonId;
        }

        public void Delete(int id)
        {
            var round = this.db.Rounds.Find(id);
            if (round == null)
            {
                return;
            }
            this.db.Rounds.Remove(round);
            this.db.SaveChanges();
        }

        public bool IsActive(int id)
        => this.db.
            Rounds
            .Where(x => x.Id == id)
            .Select(x => x.IsActive)
            .First();

        public bool SetAsActive(int id, string username)
        {
            var round = this.db.Rounds.Find(id);

            if (round == null)
                return false;

            var rounds = this.db.Rounds
                                .Where(x => x.IsActive == true &&
                                    x.SeasonId == round.SeasonId);

            foreach (var activeRound in rounds)
            {
                activeRound.IsActive = false;
            }

            round.IsActive = true;
            round.ModifiedBy = username;
            round.ModifiedOn = DateTime.Now;
            this.db.SaveChanges();
            return true;
        }

        public bool SetAsReadonlyForEditFromModerator(int id, string username)
        {
            var round = this.db.Rounds.Find(id);

            if (round == null)
                return false;

            round.ModifiedBy = username;
            round.ModifiedOn = DateTime.Now;
            this.db.SaveChanges();
            return true;
        }

        public int GetActiveSeasonId()
        => this.db
            .Seasons
            .Where(x => x.IsActive == true)
            .Select(x => x.Id)
            .First();

        public bool AnyActiveSeasoin()
         => this.db
             .Seasons
             .Where(x => x.IsActive == true)
             .Any();
    }
}
