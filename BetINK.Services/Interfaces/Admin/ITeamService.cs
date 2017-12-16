namespace BetINK.Services.Interfaces.Admin
{
    using BetINK.Services.Models.Team;
    using System.Collections.Generic;

    public interface ITeamService
    {
        IEnumerable<TeamListingServiceModel> AllByLeagueId(int leagueId);
        TeamServiceModel ById(int id);
        void Create(string name, string country, string emblemUrl, List<int> leagues);
        void Edit(int id, string name, string country, string emblemUrl, List<int> leagues);
        void Delete(int id);
    }
}
