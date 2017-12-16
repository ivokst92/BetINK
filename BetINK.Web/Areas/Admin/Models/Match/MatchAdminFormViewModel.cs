namespace BetINK.Web.Areas.Admin.Models.Match
{
    using BetINK.Common.Enums;
    using BetINK.Web.Areas.Moderator.Models.Match;

    public class MatchAdminFormViewModel : MatchFormViewModel
    {
        public ResultEnum? Result { get; set; }
    }
}
