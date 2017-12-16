namespace BetINK.Services.Models.Season
{
    using BetINK.Common.Mapping;
    using BetINK.DataAccess.Models;

    public class SeasonServiceModel : IMapFrom<Season>
    {
        public string Description { get; set; }

        public string Name { get; set; }
    }
}
