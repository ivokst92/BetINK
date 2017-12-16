namespace BetINK.Services.Models.Round
{
    using AutoMapper;
    using BetINK.Common.Mapping;
    using BetINK.DataAccess.Models;

    public class RoundListingServiceModel : RoundServiceModel, IMapFrom<Round>, ICustomMapping
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }

        public bool IsEditAllowed { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            string username = null;
            mapper.CreateMap<Round, RoundListingServiceModel>()
                .ForMember(x => x.IsEditAllowed,
                cfg =>
                {
                    cfg.MapFrom(x => x.CreatedBy == username
                    && (x.ModifiedBy == username || x.ModifiedBy == null));
                });
        }
    }
}
