namespace BetINK.Web.Controllers
{
    using BetINK.Common.Enums;
    using BetINK.Common.Resources;
    using BetINK.DataAccess.Models;
    using BetINK.Services.Interfaces.UserInteractions;
    using BetINK.Web.Infrastructure.Extensions;
    using BetINK.Web.Models.Prediction;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper.QueryableExtensions;

    [Authorize]
    public class PredictionController : Controller
    {
        IPredictionService predictionService;
        UserManager<User> userManager;

        public PredictionController(IPredictionService predictionService,
            UserManager<User> userManager)
        {
            this.predictionService = predictionService;
            this.userManager = userManager;
        }
        

        public ActionResult Check(string username)
        {
            var user = this.userManager.Users
                .Where(x => x.UserName == username)
                .SingleOrDefault();

            if (user == null)
            {
                return BadRequest();
            }

            UserPredictionsViewModel model = new UserPredictionsViewModel()
            {
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RoundNumber = this.predictionService.GetActiveRoundNumber(),
                Matches = this.predictionService
                        .GetActiveMatches(user.Id)
                        .ProjectTo<MatchViewModel>()
                        .ToList()
            };
            return View(model);
        }

        public ActionResult Bet()
        {
            var userId = GetUserId();
            var matches = this.predictionService.GetActiveMatches(userId);
            var model = new ActiveRoundViewModel()
            {
                RoundNumber = this.predictionService.GetActiveRoundNumber(),
                AlreadyPredicted = this.predictionService.IsCurrentRoundPredicted(userId),
                Matches = matches
                    .Where(x => x.IsPredictionAllowed)
                    .ProjectTo<MatchViewModel>()
                    .ToList(),
                StartedMatches = matches
                    .Where(x => x.IsPredictionAllowed == false)
                    .ProjectTo<MatchViewModel>()
                    .ToList()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Bet(ActiveRoundViewModel model)
        {

            //Return back to view if any non predicted match
            var anyNonPredictedMatch = model.Matches.Any(x => x.UserPrediction == null);
            if (anyNonPredictedMatch)
            {
                this.TempData.AddErrorMessage(MessageResources.msgBetAllMatches);
                return View(model);
            }

            List<int> activeMatchesIds = this.predictionService.GetCurrentActiveMatchesIds();
            Dictionary<int, ResultEnum> predictions = new Dictionary<int, ResultEnum>();

            //Compare client side matches with in db active matches.
            foreach (var match in model.Matches)
            {
                bool activeMatch = activeMatchesIds.Contains(match.Id);
                if (activeMatch)
                {
                    predictions.Add(match.Id, match.UserPrediction.Value);
                }
            }

            this.predictionService.AddPrediction(predictions, GetUserId());
            this.TempData.AddSuccessMessage(MessageResources.msgSuccessfulPredictions);
            return RedirectToAction(nameof(Bet));
        }

        private string GetUserId()
        => this.userManager.GetUserId(User);
    }
}