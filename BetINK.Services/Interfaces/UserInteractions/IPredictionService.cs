namespace BetINK.Services.Interfaces.UserInteractions
{
    using BetINK.Common.Enums;
    using BetINK.Services.Models.Prediction;
    using System.Collections.Generic;

    public interface IPredictionService
    {
        bool IsCurrentRoundPredicted(string userId);

        IEnumerable<MatchServiceModel> GetActiveMatches(string userId);

        List<int> GetAllActiveMatchesIds();

        void AddPrediction(Dictionary<int,ResultEnum> predictions, string userId);

        int GetActiveRoundNumber();
    }
}
