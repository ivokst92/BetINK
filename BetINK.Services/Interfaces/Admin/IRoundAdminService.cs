namespace BetINK.Services.Interfaces.Admin
{
    using BetINK.Services.Models.Round;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRoundAdminService
    {
        Task<IEnumerable<RoundAdminListingServiceModel>> AllBySeason(int seasonId);

        Task<IEnumerable<RoundAdminListingServiceModel>> All();

        void Create(int seasonId, int number, string description, string username);

        RoundServiceModel ById(int id);

        int Edit(int id, int number, string description, string username);

        bool SetAsReadonlyForEditFromModerator(int id, string username);

        bool SetAsActive(int id, string username);

        bool IsActive(int id);

        void Delete(int id);

        int GetActiveSeasonId();

        bool AnyActiveSeasoin();
    }
}
