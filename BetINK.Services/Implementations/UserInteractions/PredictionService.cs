namespace BetINK.Services.Implementations.UserInteractions
{
    using AutoMapper.QueryableExtensions;
    using BetINK.Common.Enums;
    using BetINK.Common.Helpers;
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
                    CreatedOn = DateTime.Now.ToFLEStandartTime()
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

        public IQueryable<MatchServiceModel> GetMatches(string userId, int round)
        {
            int activeSeason = GetActiveSeason();
            return this.db.Matches
                 .OrderByDescending(x => x.Id)
                 .Where(x => x.Round.Number == round 
                 && x.Round.SeasonId == activeSeason)
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

        //Getting all active matches didn't start yet.
        public List<int> GetCurrentActiveMatchesIds()
        {
            int activeRoundId = GetCurrentActiveRound();
            return this.db.Matches
                 .OrderByDescending(x => x.Id)
                 .Where(x => x.RoundId == activeRoundId &&
                 x.MatchStart > DateTime.Now.ToFLEStandartTime())
                 .Select(x => x.Id)
                 .ToList();
        }

        //Getting all active matches (even started).
        private List<int> GetAllMatchesIdsFromActiveRound()
        {
            int activeRoundId = GetCurrentActiveRound();
            return this.db.Matches
                 .OrderByDescending(x => x.Id)
                 .Where(x => x.RoundId == activeRoundId)
                 .Select(x => x.Id)
                 .ToList();
        }

        public bool IsCurrentRoundPredicted(string userId)
        {
            List<int> activeMatches = GetAllMatchesIdsFromActiveRound();
            foreach (var matchId in activeMatches)
            {
                bool Any = this.db.Predictions
                .Any(x => x.UserId == userId && x.MatchId == matchId);

                if (Any)
                    return Any;
            }
            return false;
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

        private DateTime GetActiveRoundCreateDate()
        {
            int activeSeason = GetActiveSeason();
            return this.db
                  .Rounds
                  .Where(x => x.IsActive == true
                  && x.SeasonId == activeSeason)
                  .Select(x => x.CreatedOn)
                  .FirstOrDefault();
        }

        public IEnumerable<TeamEmblemServiceModel> GetEmblems(List<string> teams)
        {
            return this.db.Teams
                  .Where(x => teams.Contains(x.Name))
                  .Select(x =>
                      new TeamEmblemServiceModel
                      {
                          Team = x.Name,
                          EmblemUrl = x.EmblemUrl
                      });
        }

        public List<int> GetCurrentSeasonRounds()
        {
            int activeSeason = GetActiveSeason();
            DateTime date = GetActiveRoundCreateDate();
            return this.db.Rounds
                 .Where(x => x.SeasonId == activeSeason 
                 && x.CreatedOn <= date)
                 .Select(x => x.Number)
                 .ToList();
        }
    }
}
