namespace BetINK.Web.Areas.Admin.Controllers
{
    using BetINK.Common.Resources;
    using BetINK.Services.Interfaces.Admin;
    using BetINK.Web.Areas.Admin.Models.Round;
    using BetINK.Web.Areas.Admin.Models.Shared;
    using BetINK.Web.Areas.Moderator.Models.Round;
    using BetINK.Web.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class RoundAdminController : BaseAdminController
    {
        private const string Round = "round";
        private const string SeasonId = "seasonId";
        private readonly IRoundAdminService roundService;

        public RoundAdminController(IRoundAdminService roundService)
        {
            this.roundService = roundService;
        }

        public async Task<IActionResult> All(int seasonId)
        => View(new RoundAdminListingViewModel()
        {
            SeasonId = seasonId,
            Rounds = await this.roundService.AllBySeason(seasonId)
        });

        public async Task<IActionResult> AllRounds()
        {
            bool anyActiveSeason = this.roundService.AnyActiveSeasoin();
            if (!anyActiveSeason)
            {
                this.TempData.AddSuccessMessage(MessageResources.msgNoActiveSeasonYet);
                return RedirectToAction(nameof(All), "Season");
            }

            var model = new RoundAdminListingViewModel()
            {
                SeasonId = this.roundService.GetActiveSeasonId(),
                Rounds = await this.roundService.All()
            };
            return View(nameof(All), model);
        }

        public IActionResult SetRoundAsActive(int roundId, int seasonId)
        {
            bool succeeded = this.roundService.SetAsActive(roundId, User.Identity.Name);

            if (!succeeded)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All), new { seasonId = seasonId });
        }

        public IActionResult SetAsReadonlyForModerator(int roundId, int seasonId)
        {
            bool succeeded = this.roundService.SetAsReadonlyForEditFromModerator(roundId, User.Identity.Name);

            if (!succeeded)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All), new { seasonId = seasonId });
        }

        public IActionResult AddRound(int seasonId)
        {
            TempData[SeasonId] = seasonId;
            return View();
        }

        [HttpPost]
        public IActionResult AddRound(RoundFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var seasonId = Int32.Parse(TempData[SeasonId].ToString());
            this.roundService.Create(seasonId,
                                      model.Number,
                                      model.Description,
                                      User.Identity.Name);

            this.TempData.AddSuccessMessage(MessageResources.msgSuccessfullyAdded);

            return RedirectToAction(nameof(All), new { seasonId = seasonId });
        }

        public IActionResult EditRound(int roundId, int seasonId)
        {
            var round = this.roundService.ById(roundId);
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
        public IActionResult EditRound(int roundId, RoundFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int seasonId = this.roundService.Edit(roundId,
                                     model.Number,
                                     model.Description,
                                     User.Identity.Name);

            if (seasonId == default(int))
            {
                return BadRequest();
            }

            this.TempData.AddSuccessMessage(MessageResources.msgSuccessfullyEdited);
            return RedirectToAction(nameof(All), new { seasonId = seasonId });
        }

        [HttpGet]
        public IActionResult DeleteRound(int roundId, int seasonId)
        {
            var controller = this.ControllerContext.ActionDescriptor.RouteValues["controller"].ToLower();
            var area = this.ControllerContext.ActionDescriptor.RouteValues["area"];
            Dictionary<string, string> additionalParams = new Dictionary<string, string>()
            {
                {nameof(seasonId),seasonId.ToString() }
            };
            DeleteConfirmationViewModel model = new DeleteConfirmationViewModel()
            {
                Id = roundId,
                ItemName = Round,
                CancelPath = UrlBuilder.GetUrl(controller, nameof(All), area: area, additionalParameters: additionalParams),
                DestroyPath = UrlBuilder.GetUrl(controller, nameof(DestroyRound), roundId.ToString(), area, additionalParameters: additionalParams)
            };

            return View("DeleteConfirmationView", model);
        }

        public IActionResult DestroyRound(int Id, int seasonId)
        {
            bool isActive = this.roundService.IsActive(Id);
            if (!isActive)
            {
                this.roundService.Delete(Id);
            }
            else
            {
                this.TempData.AddErrorMessage(MessageResources.msgActiveRoundCannotBeRemoved);
            }
            return RedirectToAction(nameof(All), new { seasonId = seasonId });
        }
    }
}