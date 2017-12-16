namespace BetINK.Services.Models.League
{
    using BetINK.Common.Mapping;
    using BetINK.DataAccess.Models;

    public class LeagueListingServiceModel : BaseLeagueServiceModel, IMapFrom<League>
    {
        public int Id { get; set; }
    }
}
