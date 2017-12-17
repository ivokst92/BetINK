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

        [Authorize]
        public ActionResult Bet()
        {
            var userId = GetUserId();
            var matches = this.predictionService.GetActiveMatches(userId);
            var model = new ActiveRoundViewModel()
            {
                RoundNumber = this.predictionService.GetActiveRoundNumber(),
                AlreadyPredicted = this.predictionService.IsCurrentRoundPredicted(userId),
                Matches = matches.Where(x => x.IsPredictionAllowed).ProjectTo<MatchViewModel>().ToList(),
                StartedMatches = matches.Where(x => x.IsPredictionAllowed == false).ProjectTo<MatchViewModel>().ToList()
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Bet(ActiveRoundViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var anyNonPredictedMatch = model.Matches.Any(x => x.UserPrediction == null);
            if (anyNonPredictedMatch)
            {
                this.TempData.AddErrorMessage(MessageResources.msgBetAllMatches);
                return View(model);
            }
            List<int> activeMatchesIds = this.predictionService.GetAllActiveMatchesIds();
            Dictionary<int, ResultEnum> predictions = new Dictionary<int, ResultEnum>();
            foreach (var match in model.Matches)
            {
                bool activeMatch = activeMatchesIds.Contains(match.Id);
                if (activeMatch)
                {
                    predictions.Add(match.Id, match.UserPrediction.Value);
                }
            }

            this.predictionService.AddPrediction(predictions, GetUserId());
            return RedirectToAction(nameof(Bet));
        }

        private string GetUserId()
        => this.userManager.GetUserId(User);

    }
}