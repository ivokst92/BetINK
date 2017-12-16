namespace BetINK.Services.Models.League
{
    using BetINK.Common.Mapping;
    using BetINK.DataAccess.Models;

    public class LeagueServiceModel : BaseLeagueServiceModel, IMapFrom<League>
    {
        public string EmblemUrl { get; set; }
    }
}
