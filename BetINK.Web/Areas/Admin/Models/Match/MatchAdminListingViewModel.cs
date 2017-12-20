namespace BetINK.Web.Areas.Admin.Models.Match
{
    using BetINK.Services.Models.Match;
    using System.Collections.Generic;

    public class MatchAdminListingViewModel
    {
        public IEnumerable<MatchAdminListingServiceModel> Matches { get; set; }

        public int RoundId { get; set; }
    }
}
