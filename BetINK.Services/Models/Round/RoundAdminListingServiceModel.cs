namespace BetINK.Services.Models.Round
{
    using AutoMapper;
    using BetINK.Common.Mapping;
    using BetINK.DataAccess.Models;
    using System;
    using System.Collections.Generic;

    public class RoundAdminListingServiceModel : RoundServiceModel, IMapFrom<Round>, ICustomMapping
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }

        public int MatchesCount { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool EditableForModerator { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            List<string> moderators = null;
            mapper.CreateMap<Round, RoundAdminListingServiceModel>()
                 .ForMember(x => x.MatchesCount,
                 cfg => cfg.MapFrom(x => x.Matches.Count))
                 .ForMember(x => x.EditableForModerator,
                 cfg => cfg.MapFrom(x =>
                 (x.CreatedBy == x.ModifiedBy || x.ModifiedBy == null) &&
                 moderators.Contains(x.CreatedBy)));

        }
    }
}
