namespace BetINK.Services.Interfaces.Admin
{
    using BetINK.Services.Models.League;
    using System.Collections.Generic;

    public interface ILeagueService
    {
        IEnumerable<LeagueListingServiceModel> All();

        IEnumerable<SelectItemLeague> AllLeagues();

        LeagueServiceModel ById(int id);

        void Create(string name, string country, string emblemUrl);

        void Edit(int id, string name, string country, string emblemUrl);

        void Delete(int id);

        string GetLeagueNameById(int id);

        bool Exists(int leagueId);
    }
}
