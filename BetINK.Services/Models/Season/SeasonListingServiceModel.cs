namespace BetINK.Services.Models.Season
{
    using BetINK.Common.Mapping;
    using BetINK.DataAccess.Models;

    public class SeasonListingServiceModel : SeasonServiceModel, IMapFrom<Season>
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }
    }
}
