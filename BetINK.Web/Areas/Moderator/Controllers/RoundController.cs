namespace BetINK.Web.Areas.Moderator.Controllers
{
    using BetINK.Common.Resources;
    using BetINK.Services.Interfaces.Admin;
    using BetINK.Services.Interfaces.Moderator;
    using BetINK.Web.Areas.Moderator.Models.Round;
    using BetINK.Web.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;

    public class RoundController : BaseModeratorController
    {
        private IRoundService roundService;

        private ISeasonService seasonService;

        public RoundController(IRoundService roundService,
            ISeasonService seasonService)
        {
            this.seasonService = seasonService;
            this.roundService = roundService;
        }

        public IActionResult All()
            => View(this.roundService.All(User.Identity.Name));

        public IActionResult AddRound()
        {
            bool anyActiveSeason = this.seasonService.AnyActiveSeason();
            if (!anyActiveSeason)
            {
                TempData.AddErrorMessage(MessageResources.msgNoActiveSeasonsFound);
                return RedirectToAction(nameof(All));
            }

            return View();
        }

        [HttpPost]
        public IActionResult AddRound(RoundFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.roundService.Create(model.Number,
                                      model.Description,
                                      User.Identity.Name);

            return RedirectToAction(nameof(All));
        }

        public IActionResult EditRound(int Id)
        {
            bool allowedToEdit = this.roundService
                .IsUserAllowedToEdit(Id, User.Identity.Name);

            if (!allowedToEdit)
            {
                TempData.AddErrorMessage(MessageResources.msgNotAllowedToEditRound);
                return RedirectToAction(nameof(All));
            }

            var round = this.roundService.ById(Id);
            if (round == null)
            {
                return NotFound();
            }
            
            return View(new RoundFormViewModel()
            {
                Number = round.Number,
                Description = round.Description
            });
        }

        [HttpPost]
        public IActionResult EditRound(int Id, RoundFormViewModel model)
        {
            bool allowedToEdit = this.roundService
                .IsUserAllowedToEdit(Id, User.Identity.Name);

            if (!allowedToEdit)
            {
                TempData.AddErrorMessage(MessageResources.msgNotAllowedToEditRound);
                return RedirectToAction(nameof(All));
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.roundService.Edit(Id,
                                    model.Number,
                                    model.Description,
                                    User.Identity.Name);

            this.TempData.AddSuccessMessage(MessageResources.msgSuccessfullyEdited);

            return RedirectToAction(nameof(All));
        }
    }
}