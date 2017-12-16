namespace BetINK.Web.Areas.Moderator.Models.Match
{
    using BetINK.Services.Models.Match;
    using System.Collections.Generic;

    public class MatchListingViewModel
    {
        public IEnumerable<MatchListingServiceModel> Matches { get; set; }

        public int RoundId { get; set; }
    }
}

