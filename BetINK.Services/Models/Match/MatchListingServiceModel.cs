namespace BetINK.Services.Models.Match
{
    using AutoMapper;
    using BetINK.Common.Enums;
    using BetINK.Common.Helpers;
    using BetINK.Common.Mapping;
    using BetINK.DataAccess.Models;

    public class MatchListingServiceModel : MatchServiceModel, IMapFrom<Match>, ICustomMapping
    {
        public int Id { get; set; }

        public ResultEnum? Result { get; set; }

        public bool IsEditAllowed { get; set; }

        public string ResultFriendlyNamed
        {
            get
            {
                return ResultHelper.GetNullableResult(this.Result);
            }
        }

        public void ConfigureMapping(Profile mapper)
        {
            string username = null;
            mapper.CreateMap<Match, MatchListingServiceModel>()
                .ForMember(x => x.IsEditAllowed,
                cfg =>
                {
                    cfg.MapFrom(x => x.Round.CreatedBy == username
                    && (x.Round.ModifiedBy == username || x.Round.ModifiedBy == null));
                });
        }
    }
}
