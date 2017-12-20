namespace BetINK.Web.Areas.Moderator.Controllers
{
    using BetINK.Common.Helpers;
    using BetINK.Common.Resources;
    using BetINK.Services.Interfaces.Admin;
    using BetINK.Services.Interfaces.Moderator;
    using BetINK.Web.Areas.Moderator.Models.Match;
    using BetINK.Web.Infrastructure.Extensions;
    using BetINK.Web.Models.Shared;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MatchController : BaseModeratorController
    {
        private readonly IMatchService matchService;

        private readonly IRoundService roundService;

        private readonly ILeagueService leagueService;

        public MatchController(IMatchService matchService,
            IRoundService roundService,
            ILeagueService leagueService)
        {
            this.matchService = matchService;
            this.roundService = roundService;
            this.leagueService = leagueService;
        }

        public IActionResult All(int roundId)
         => View(new MatchListingViewModel
         {
             Matches = this.matchService.All(roundId, User.Identity.Name),
             RoundId = roundId
         });

        public IActionResult ChooseLeague(int roundId)
        {
            bool allowedToEdit = this.roundService
                .IsUserAllowedToEdit(roundId, User.Identity.Name);

            if (!allowedToEdit)
            {
                TempData.AddErrorMessage(MessageResources.msgNotAllowedToAddMatchForThisRound);
                return RedirectToAction(nameof(All), new { roundId = roundId });
            }

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
            bool allowedToEdit = this.roundService
                .IsUserAllowedToEdit(roundId, User.Identity.Name);

            if (!allowedToEdit)
            {
                TempData.AddErrorMessage(MessageResources.msgNotAllowedToAddMatchForThisRound);
                return RedirectToAction(nameof(All), new { roundId = roundId });
            }

            var teams = GetSelectListItemTeams(leagueId);
            return View(new MatchFormViewModel
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
        public IActionResult AddMatch(MatchFormViewModel model)
        {
            bool allowedToEdit = this.roundService
                .IsUserAllowedToEdit(model.RoundId, User.Identity.Name);

            if (!allowedToEdit)
            {
                TempData.AddErrorMessage(MessageResources.msgNotAllowedToAddMatchForThisRound);
                return RedirectToAction(nameof(All), new { roundId = model.RoundId });
            }

            if (!ModelState.IsValid)
            {
                model.Teams = GetSelectListItemTeams(model.LeagueId);
                return View(model);
            }

            this.matchService.Create(model.HomeTeam,
                model.AwayTeam,
                model.HomeWinPoints,
                model.DrawPoints,
                model.AwayWinPoints,
                model.MatchStart,
                model.RoundId,
                model.LeagueId);

            this.TempData.AddSuccessMessage(MessageResources.msgSuccessfullyAdded);

            return RedirectToAction(nameof(All), new { roundId = model.RoundId });
        }

        public IActionResult EditMatch(int matchId, int roundId)
        {
            bool allowedToEdit = this.roundService
                .IsUserAllowedToEdit(matchId, roundId, User.Identity.Name);

            if (!allowedToEdit)
            {
                TempData.AddErrorMessage(MessageResources.msgNotAllowedToEditMatch);
                return RedirectToAction(nameof(All), new { roundId = roundId });
            }

            var match = this.matchService.ByIdAndRoundId(matchId, roundId);
            if (match == null)
            {
                return NotFound();
            }

            return View(new MatchFormViewModel()
            {
                HomeTeam = match.HomeTeam,
                AwayTeam = match.AwayTeam,
                HomeWinPoints = match.HomeWinPoints,
                DrawPoints = match.DrawPoints,
                AwayWinPoints = match.AwayWinPoints,
                LeagueId = match.LeagueId,
                MatchStart = match.MatchStart,
                RoundId = match.RoundId,
                Teams = GetSelectListItemTeams(match.LeagueId)
            });
        }

        [HttpPost]
        public IActionResult EditMatch(int matchId, MatchFormViewModel model)
        {
            bool allowedToEdit = this.roundService
                .IsUserAllowedToEdit(matchId, model.RoundId, User.Identity.Name);

            if (!allowedToEdit)
            {
                TempData.AddErrorMessage(MessageResources.msgNotAllowedToEditMatch);
                return RedirectToAction(nameof(All), new { roundId = model.RoundId });
            }

            if (!ModelState.IsValid)
            {
                model.Teams = GetSelectListItemTeams(model.LeagueId);
                return View(model);
            }

            this.matchService.Edit(matchId,
                                   model.HomeTeam,
                                   model.AwayTeam,
                                   model.HomeWinPoints,
                                   model.DrawPoints,
                                   model.AwayWinPoints,
                                   model.MatchStart);

            this.TempData.AddSuccessMessage(MessageResources.msgSuccessfullyEdited);

            return RedirectToAction(nameof(All), new { roundId = model.RoundId });
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