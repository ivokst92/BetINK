namespace BetINK.Web.Areas.Admin.Controllers
{
    using BetINK.Common.Constants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area(WebConstants.AdminArea)]
    [Authorize(Roles = WebConstants.AdministratorRole)]
    public abstract class BaseAdminController : Controller
    {

    }
}