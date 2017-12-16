using BetINK.Services.Models.Match;
using System.Collections.Generic;

namespace BetINK.Web.Areas.Admin.Models.Match
{
    public class MatchAdminListingViewModel
    {
        public IEnumerable<MatchAdminListingServiceModel> Matches { get; set; }

        public int RoundId { get; set; }
    }
}
