namespace BetINK.Web.Areas.Admin.Controllers
{
    using BetINK.Common.Resources;
    using BetINK.Services.Interfaces.Admin;
    using BetINK.Web.Areas.Admin.Models.League;
    using BetINK.Web.Areas.Admin.Models.Shared;
    using BetINK.Web.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;

    public class LeagueController : BaseAdminController
    {
        private readonly ILeagueService leagueService;

        public LeagueController(ILeagueService leagueService)
        {
            this.leagueService = leagueService;
        }

        public IActionResult All()
        => View(this.leagueService.All());

        public IActionResult AddLeague()
         => View();

        [HttpPost]
        public IActionResult AddLeague(LeagueFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.leagueService.Create(model.Name,
                                      model.Country,
                                      model.EmblemUrl);

            this.TempData.AddSuccessMessage(MessageResources.msgSuccessfullyAdded);

            return RedirectToAction(nameof(All));
        }

        public IActionResult EditLeague(int Id)
        {
            var league = this.leagueService.ById(Id);
            if (league == null)
            {
                return NotFound();
            }

            return View(new LeagueFormViewModel()
            {
                Name = league.Name,
                Country = league.Country,
                EmblemUrl = league.EmblemUrl
            });
        }

        [HttpPost]
        public IActionResult EditLeague(int Id, LeagueFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.leagueService.Edit(Id,
                                    model.Name,
                                    model.Country,
                                    model.EmblemUrl);

            this.TempData.AddSuccessMessage(MessageResources.msgSuccessfullyEdited);
            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public IActionResult DeleteLeague(int id)
        {
            var controller = this.ControllerContext.ActionDescriptor.RouteValues["controller"];
            var area = this.ControllerContext.ActionDescriptor.RouteValues["area"];

            DeleteConfirmationViewModel model = new DeleteConfirmationViewModel()
            {
                Id = id,
                ItemName = controller,
                CancelPath = UrlBuilder.GetUrl(controller, nameof(All), area: area),
                DestroyPath = UrlBuilder.GetUrl(controller, nameof(DestroyLeague), id.ToString(), area)
            };

            return View("DeleteConfirmationView", model);
        }

        public IActionResult DestroyLeague(int Id)
        {
            this.leagueService.Delete(Id);
            return RedirectToAction(nameof(All));
        }
    }
}