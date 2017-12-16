namespace BetINK.Web.Areas.Admin.Models.Round
{
    using BetINK.Services.Models.Round;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class RoundAdminListingViewModel
    {
        public IEnumerable<RoundAdminListingServiceModel> Rounds { get; set; }

        public int SeasonId { get; set; }
    }
}
