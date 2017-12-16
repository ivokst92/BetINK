namespace BetINK.Web.Areas.Moderator.Controllers
{
    using BetINK.Common.Constants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area(WebConstants.ModeratorArea)]
    [Authorize(Roles = WebConstants.AdministratorRole + "," + WebConstants.ModeratorRole)]
    public abstract class BaseModeratorController : Controller
    {

    }
}