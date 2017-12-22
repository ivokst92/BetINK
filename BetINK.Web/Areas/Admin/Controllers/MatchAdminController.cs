namespace BetINK.Web.Areas.Admin.Controllers
{
    using BetINK.Common.Resources;
    using BetINK.Services.Interfaces.Admin;
    using BetINK.Services.Interfaces.Moderator;
    using BetINK.Web.Areas.Admin.Models.Match;
    using BetINK.Web.Areas.Admin.Models.Shared;
    using BetINK.Web.Areas.Moderator.Models.Match;
    using BetINK.Web.Infrastructure.Extensions;
    using BetINK.Web.Models.Shared;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BetINK.Common.Helpers;

    public class MatchAdminController : BaseAdminController
    {
        private const string Match = "match";

        private readonly IMatchAdminService matchAdminService;

        private readonly IMatchService matchService;

        private readonly ILeagueService leagueService;

        public MatchAdminController(IMatchAdminService matchAdminService,
                IMatchService matchService,
                ILeagueService leagueService)
        {
            this.matchAdminService = matchAdminService;
            this.matchService = matchService;
            this.leagueService = leagueService;
        }

        public IActionResult All(int roundId)
         => View(new MatchAdminListingViewModel
         {
             Matches = this.matchAdminService.All(roundId),
             RoundId = roundId
         });

        public IActionResult ChooseLeague(int roundId)
        {

            var leagues = this.leagueService.AllLeagues();
            if (leagues.Count() == 0)
            {
                return RedirectToAction(nameof(AddMatch), new { roundId = roundId });
            }

            return View(new ChooseLeagueViewModel()
            {
                RoundId = roundId,
                Leagues = leagues,
                Controller = this.ControllerContext.ActionDescriptor.RouteValues["controller"]
            });
        }

        public IActionResult AddMatch(int roundId, int? leagueId)
        {
            bool roundExists = matchAdminService.RoundExists(roundId);

            if (!roundExists)
            {
                return NotFound();
            }

            var teams = GetSelectListItemTeams(leagueId);
            return View(new MatchAdminFormViewModel
            {
                HomeWinPoints = 1,
                AwayWinPoints = 1,
                DrawPoints = 1,
                MatchStart = DateTime.Now.ToFLEStandartTime(),
                Teams = teams,
                RoundId = roundId,
                LeagueId = leagueId
            });
        }

        [HttpPost]
        public IActionResult AddMatch(MatchAdminFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Teams = GetSelectListItemTeams(model.LeagueId);
                return View(model);
            }

            bool roundExists = matchAdminService.RoundExists(model.RoundId);

            if (!roundExists)
            {
                return NotFound();
            }

            this.matchAdminService.Create(model.HomeTeam,
                                          model.AwayTeam,
                                          model.HomeWinPoints,
                                          model.AwayWinPoints,
                                          model.DrawPoints,
                                          model.MatchStart,
                                          model.Result,
                                          model.RoundId,
                                          model.LeagueId);

            this.TempData.AddSuccessMessage(MessageResources.msgSuccessfullyAdded);

            return RedirectToAction(nameof(All), new { roundId = model.RoundId });
        }

        public IActionResult EditMatch(int matchId, int roundId)
        {
            var match = this.matchAdminService.ByIdAndRoundId(matchId, roundId);
            if (match == null)
            {
                return NotFound();
            }

            return View(new MatchAdminFormViewModel()
            {
                HomeTeam = match.HomeTeam,
                AwayTeam = match.AwayTeam,
                HomeWinPoints = match.HomeWinPoints,
                DrawPoints = match.DrawPoints,
                AwayWinPoints = match.AwayWinPoints,
                LeagueId = match.LeagueId,
                MatchStart = match.MatchStart,
                RoundId = match.RoundId,
                Result = match.Result,
                Teams = GetSelectListItemTeams(match.LeagueId)
            });
        }

        [HttpPost]
        public IActionResult EditMatch(int matchId, MatchAdminFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Teams = GetSelectListItemTeams(model.LeagueId);
                return View(model);
            }

            this.matchAdminService.Edit(matchId,
                                        model.HomeTeam,
                                        model.AwayTeam,
                                        model.HomeWinPoints,
                                        model.AwayWinPoints,
                                        model.DrawPoints,
                                        model.MatchStart,
                                        model.Result);

            this.TempData.AddSuccessMessage(MessageResources.msgSuccessfullyEdited);

            return RedirectToAction(nameof(All), new { roundId = model.RoundId });
        }

        [HttpGet]
        public IActionResult DeleteMatch(int matchId, int roundId)
        {
            var controller = this.ControllerContext.ActionDescriptor.RouteValues["controller"].ToLower();
            var area = this.ControllerContext.ActionDescriptor.RouteValues["area"];

            Dictionary<string, string> additionalParams = new Dictionary<string, string>()
            {
                {nameof(roundId),roundId.ToString() }
            };

            DeleteConfirmationViewModel model = new DeleteConfirmationViewModel()
            {
                Id = matchId,
                ItemName = Match,
                CancelPath = UrlBuilder.GetUrl(controller, nameof(All), area: area, additionalParameters: additionalParams),
                DestroyPath = UrlBuilder.GetUrl(controller, nameof(DestroyTeam), matchId.ToString(), area, additionalParams)
            };

            return View("DeleteConfirmationView", model);
        }

        public IActionResult DestroyTeam(int Id, int roundId)
        {
            this.matchAdminService.Delete(Id);
            return RedirectToAction(nameof(All), new { roundId = roundId });
        }

        private List<SelectListItem> GetSelectListItemTeams(int? leagueId)
        {
            if (leagueId == null)
                return new List<SelectListItem>();

            List<string> teams = this.matchService.AllTeams(leagueId.Value).ToList();

            var selectListItems = new List<SelectListItem>();
            teams.ForEach(x => selectListItems.Add(new SelectListItem() { Text = x, Value = x }));
            return selectListItems;
        }

    }
}