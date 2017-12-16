namespace BetINK.Services.Models.Team
{
    using BetINK.Common.Mapping;
    using System.Collections.Generic;
    using BetINK.DataAccess.Models;
    using AutoMapper;
    using System.Linq;

    public class TeamListingServiceModel : BaseTeamServiceModel, IMapFrom<Team>, ICustomMapping
    {
        public int Id { get; set; }

        public string EmblemUrl { get; set; }

        public List<string> Leagues { get; set; }

        public void ConfigureMapping(Profile mapper)
        => mapper.CreateMap<Team, TeamListingServiceModel>()
                 .ForMember(t => t.Leagues, cfg =>
                 cfg.MapFrom(p => p.Leagues.Select(x => x.League.Name)));
    }
}
