namespace BetINK.Services.Interfaces.Moderator
{
    using BetINK.Services.Models.Round;
    using System.Collections.Generic;

    public interface IRoundService
    {
        IEnumerable<RoundListingServiceModel> All(string username);

        void Create(int number, string description, string username);

        bool IsUserAllowedToEdit(int roundId, string username);
        
        bool IsUserAllowedToEdit(int matchId, int roundId, string name);

        RoundServiceModel ById(int id);

        void Edit(int id, int number, string description, string username);

    }
}
