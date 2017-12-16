namespace BetINK.Web.Areas.Admin.Controllers
{
    using BetINK.Common.Resources;
    using BetINK.Services.Interfaces.Admin;
    using BetINK.Web.Areas.Admin.Models.League;
    using BetINK.Web.Areas.Admin.Models.Shared;
    using BetINK.Web.Areas.Admin.Models.Team;
    using BetINK.Web.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Linq;

    public class TeamController : BaseAdminController
    {
        private readonly ITeamService teamService;
        private readonly ILeagueService leagueService;

        public TeamController(ITeamService teamService,
            ILeagueService leagueService)
        {
            this.teamService = teamService;
            this.leagueService = leagueService;
        }

        public IActionResult AllLeagues()
        => View(this.leagueService.AllLeagues());

        public IActionResult All(int leagueId)
        {
            string LeagueName = leagueService.GetLeagueNameById(leagueId);

            if (LeagueName == null)
            {
                return NotFound();
            }

            var teams = this.teamService.AllByLeagueId(leagueId);

            return View(new TeamListingViewModel
            {
                Teams = teams,
                League = new LeagueViewModel
                {
                    Id = leagueId,
                    Name = LeagueName
                }
            });
        }

        public IActionResult AddTeam(int leagueId)
        {
            bool leageExists = leagueService.Exists(leagueId);

            if (!leageExists)
            {
                return NotFound();
            }

            return View(new TeamFormViewModel()
            {
                LeagueIds = new List<int>() { leagueId },
                Leagues = GetSelectListLeagues()
            });
        }

        [HttpPost]
        public IActionResult AddTeam(TeamFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Leagues = GetSelectListLeagues();
                return View(model);
            }
            if (model.LeagueIds.Count == 0)
            {
                TempData.AddErrorMessage(MessageResources.msgChooseLeague);
                return View(model);
            }
            this.teamService.Create(model.Name,
                                      model.Country,
                                      model.EmblemUrl,
                                      model.LeagueIds);
            this.TempData.AddSuccessMessage(MessageResources.msgSuccessfullyAdded);
            return RedirectToAction(nameof(All), new { leagueId = model.LeagueIds.First() });
        }

        public IActionResult EditTeam(int teamId)
        {
            var Team = this.teamService.ById(teamId);
            if (Team == null)
            {
                return NotFound();
            }

            return View(new TeamFormViewModel()
            {
                Name = Team.Name,
                Country = Team.Country,
                EmblemUrl = Team.EmblemUrl,
                LeagueIds = Team.Leagues,
                Leagues = GetSelectListLeagues()
            });
        }

        [HttpPost]
        public IActionResult EditTeam(int teamId, TeamFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Leagues = GetSelectListLeagues();
                return View(model);
            }

            if (model.LeagueIds.Count == 0)
            {
                TempData.AddErrorMessage(MessageResources.msgChooseLeague);
                return View(model);
            }

            this.teamService.Edit(teamId,
                                  model.Name,
                                  model.Country,
                                  model.EmblemUrl,
                                  model.LeagueIds);
            this.TempData.AddSuccessMessage(MessageResources.msgSuccessfullyEdited);
            return RedirectToAction(nameof(All), new { leagueId = model.LeagueIds.First() });
        }

        [HttpGet]
        public IActionResult DeleteTeam(int teamId, int leagueId)
        {
            var controller = this.ControllerContext.ActionDescriptor.RouteValues["controller"].ToLower();
            var area = this.ControllerContext.ActionDescriptor.RouteValues["area"];

            Dictionary<string, string> additionalParams = new Dictionary<string, string>()
            {
                {nameof(leagueId),leagueId.ToString() }
            };

            DeleteConfirmationViewModel model = new DeleteConfirmationViewModel()
            {
                Id = teamId,
                ItemName = controller,
                CancelPath = UrlBuilder.GetUrl(controller, nameof(All), area: area, additionalParameters: additionalParams),
                DestroyPath = UrlBuilder.GetUrl(controller, nameof(DestroyTeam), teamId.ToString(), area, additionalParams)
            };

            return View("DeleteConfirmationView", model);
        }

        public IActionResult DestroyTeam(int Id, int leagueId)
        {
            this.teamService.Delete(Id);
            return RedirectToAction(nameof(All), new { leagueId = leagueId });
        }

        private List<SelectListItem> GetSelectListLeagues()
        {
            return (from a in this.leagueService.AllLeagues()
                    select new SelectListItem
                    {
                        Text = a.Name,
                        Value = a.Id.ToString()
                    }).ToList();
        }
    }
}