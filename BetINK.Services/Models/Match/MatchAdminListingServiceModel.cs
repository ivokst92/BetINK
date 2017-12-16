namespace BetINK.Services.Models.Match
{
    using BetINK.Common.Enums;
    using BetINK.Common.Helpers;
    using BetINK.Common.Mapping;
    using BetINK.DataAccess.Models;

    public class MatchAdminListingServiceModel : MatchAdminServiceModel, IMapFrom<Match>
    {
        public int Id { get; set; }
        
        public string ResultFriendlyNamed
        {
            get
            {
                return Result.GetNullableResult();
            }
        }
    }
}
