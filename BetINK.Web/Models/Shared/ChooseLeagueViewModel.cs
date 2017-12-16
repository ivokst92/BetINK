namespace BetINK.Web.Models.Shared
{
    using BetINK.Services.Models.League;
    using System.Collections.Generic;

    public class ChooseLeagueViewModel
    {
        public IEnumerable<SelectItemLeague> Leagues { get; set; }

        public int RoundId { get; set; }

        public string Controller { get; set; }
    }
}
