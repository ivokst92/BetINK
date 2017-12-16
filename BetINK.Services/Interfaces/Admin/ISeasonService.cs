namespace BetINK.Services.Interfaces.Admin
{
    using BetINK.Services.Models.Season;
    using System.Collections.Generic;

    public interface ISeasonService
    {
        IEnumerable<SeasonListingServiceModel> All();

        bool AnyActiveSeason();

        bool Exists(int id);

        bool AnyRoundsIn(int id);

        void SetAsActiveSeason(int id);

        SeasonServiceModel ById(int id);

        void Create(string description, string name);

        void Edit(int id, string description, string name);

        void Delete(int id);
    }
}
