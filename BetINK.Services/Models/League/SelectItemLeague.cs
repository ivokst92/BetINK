namespace BetINK.Services.Models.League
{
    using BetINK.Common.Mapping;
    using BetINK.DataAccess.Models;

    public class SelectItemLeague : IMapFrom<League>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
