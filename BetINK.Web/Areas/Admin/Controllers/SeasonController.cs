namespace BetINK.Web.Areas.Admin.Controllers
{
    using BetINK.Common.Resources;
    using BetINK.Services.Interfaces.Admin;
    using BetINK.Web.Areas.Admin.Models.Season;
    using BetINK.Web.Areas.Admin.Models.Shared;
    using BetINK.Web.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;

    public class SeasonController : BaseAdminController
    {
        private ISeasonService seasonService;

        public SeasonController(ISeasonService seasonService)
        {
            this.seasonService = seasonService;
        }

        public IActionResult All()
        => View(this.seasonService.All());

        public IActionResult AddSeason()
         => View();

        [HttpPost]
        public IActionResult AddSeason(SeasonFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.seasonService.Create(model.Description,
                                      model.Name);

            this.TempData.AddSuccessMessage(MessageResources.msgSuccessfullyAdded);

            return RedirectToAction(nameof(All));
        }

        public IActionResult EditSeason(int Id)
        {
            var season = this.seasonService.ById(Id);
            if (season == null)
            {
                return NotFound();
            }

            return View(new SeasonFormViewModel()
            {
                Name = season.Name,
                Description = season.Description
            });
        }

        [HttpPost]
        public IActionResult EditSeason(int Id, SeasonFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.seasonService.Edit(Id,
                                    model.Description,
                                    model.Name);
            this.TempData.AddSuccessMessage(MessageResources.msgSuccessfullyEdited);
            return RedirectToAction(nameof(All));
        }


        public IActionResult SetActiveSeason(int seasonId)
        {
            bool exists = this.seasonService.Exists(seasonId);
            if (!exists)
            {
                return NotFound();
            }

            this.seasonService.SetAsActiveSeason(seasonId);

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public IActionResult DeleteSeason(int id)
        {
            bool hasRounds = this.seasonService.AnyRoundsIn(id);

            if (hasRounds)
            {
                TempData.AddErrorMessage(MessageResources.msgRemovingSeasonNotAllowed);
                return RedirectToAction(nameof(All));
            }
            var controller = this.ControllerContext.ActionDescriptor.RouteValues["controller"].ToLower();
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
            bool hasRounds = this.seasonService.AnyRoundsIn(Id);
            if (hasRounds)
            {
                return this.BadRequest();
            }
            this.seasonService.Delete(Id);
            return RedirectToAction(nameof(All));
        }
    }
}