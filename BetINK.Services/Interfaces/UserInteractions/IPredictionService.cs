namespace BetINK.Services.Interfaces.UserInteractions
{
    using BetINK.Common.Enums;
    using BetINK.Services.Models.Prediction;
    using System.Collections.Generic;
    using System.Linq;

    public interface IPredictionService
    {
        bool IsCurrentRoundPredicted(string userId);

        IQueryable<MatchServiceModel> GetActiveMatches(string userId);

        List<int> GetCurrentActiveMatchesIds();

        void AddPrediction(Dictionary<int, ResultEnum> predictions, string userId);

        int GetActiveRoundNumber();

        IQueryable<MatchServiceModel> GetMatches(string id, int round);

        IEnumerable<TeamEmblemServiceModel> GetEmblems(List<string> teams);

        List<int> GetCurrentSeasonRounds();
    }
}
