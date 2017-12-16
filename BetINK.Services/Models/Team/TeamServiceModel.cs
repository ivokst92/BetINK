namespace BetINK.Services.Models.Team
{
    using AutoMapper;
    using BetINK.Common.Mapping;
    using BetINK.DataAccess.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class TeamServiceModel : BaseTeamServiceModel, IMapFrom<Team>, ICustomMapping
    {
        public string EmblemUrl { get; set; }
        public List<int> Leagues { get; set; }

        public void ConfigureMapping(Profile mapper)
        => mapper.CreateMap<Team, TeamServiceModel>()
                 .ForMember(t => t.Leagues, cfg =>
                 cfg.MapFrom(p => p.Leagues.Select(x => x.League.Id)));
    }
}
