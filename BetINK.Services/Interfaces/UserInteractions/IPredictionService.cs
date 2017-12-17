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

        List<int> GetAllActiveMatchesIds();

        void AddPrediction(Dictionary<int,ResultEnum> predictions, string userId);

        int GetActiveRoundNumber();
    }
}
