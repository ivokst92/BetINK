namespace BetINK.Services.Models.Round
{
    using BetINK.Common.Mapping;
    using BetINK.DataAccess.Models;

    public class RoundServiceModel : IMapFrom<Round>
    {
        public string Description { get; set; }

        public int Number { get; set; }
    }
}
