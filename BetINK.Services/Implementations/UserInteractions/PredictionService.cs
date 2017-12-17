namespace BetINK.Services.Implementations.UserInteractions
{
    using AutoMapper.QueryableExtensions;
    using BetINK.Common.Enums;
    using BetINK.DataAccess.Models;
    using BetINK.Services.Interfaces.UserInteractions;
    using BetINK.Services.Models.Prediction;
    using BetINK.Web.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PredictionService : IPredictionService
    {
        private readonly BetINKDbContext db;

        public PredictionService(BetINKDbContext db)
        {
            this.db = db;
        }

        public void AddPrediction(Dictionary<int, ResultEnum> predictions, string userId)
        {
            foreach (var prediction in predictions)
            {
                var userPrediction = new Prediction
                {
                    MatchId = prediction.Key,
                    MatchPrediction = prediction.Value,
                    UserId = userId,
                    CreatedOn = DateTime.Now
                };
                this.db.Predictions.Add(userPrediction);
            }

            this.db.SaveChanges();
        }

        public IQueryable<MatchServiceModel> GetActiveMatches(string userId)
        {
            int activeRoundId = GetCurrentActiveRound();
            return this.db.Matches
                 .OrderByDescending(x => x.Id)
                 .Where(x => x.RoundId == activeRoundId)
                 .ProjectTo<MatchServiceModel>(new { userId = userId });
        }

        public int GetActiveRoundNumber()
        {
            int activeSeason = GetActiveSeason();
            return this.db
                  .Rounds
                  .Where(x => x.IsActive == true
                  && x.SeasonId == activeSeason)
                  .Select(x => x.Number)
                  .FirstOrDefault();
        }

        public List<int> GetAllActiveMatchesIds()
        {
            int activeRoundId = GetCurrentActiveRound();
            return this.db.Matches
                 .OrderByDescending(x => x.Id)
                 .Where(x => x.RoundId == activeRoundId &&
                 x.MatchStart > DateTime.Now)
                 .Select(x => x.Id)
                 .ToList();
        }

        public bool IsCurrentRoundPredicted(string userId)
        {
            int activeRoundId = GetCurrentActiveRound();
            List<int> activeMatches = GetAllActiveMatchesIds();
            return this.db.Predictions
                .Any(x => x.UserId == userId
                && activeMatches.Contains(x.MatchId));
        }

        private int GetActiveSeason()
        => this.db
            .Seasons
            .Where(x => x.IsActive == true)
            .Select(x => x.Id)
            .FirstOrDefault();

        private int GetCurrentActiveRound()
        {
            int activeSeason = GetActiveSeason();
            return this.db
                  .Rounds
                  .Where(x => x.IsActive == true
                  && x.SeasonId == activeSeason)
                  .Select(x => x.Id)
                  .FirstOrDefault();
        }
    }
}
