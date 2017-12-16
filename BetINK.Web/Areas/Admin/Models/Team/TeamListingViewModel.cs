namespace BetINK.Web.Areas.Admin.Models.Team
{
    using BetINK.Services.Models.Team;
    using BetINK.Web.Areas.Admin.Models.League;
    using System.Collections.Generic;

    public class TeamListingViewModel
    {
        public IEnumerable<TeamListingServiceModel> Teams { get; set; }

        public LeagueViewModel League { get; set; }
    }
}
