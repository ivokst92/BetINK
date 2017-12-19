namespace BetINK.Tests.Web.Controllers
{
    using BetINK.Common.Constants;
    using BetINK.Common.Enums;
    using BetINK.Common.Resources;
    using BetINK.Services.Interfaces.UserInteractions;
    using BetINK.Services.Models.Prediction;
    using BetINK.Tests.Mocks;
    using BetINK.Web.Controllers;
    using BetINK.Web.Models.Prediction;
    using FluentAssertions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using Xunit;

    public class PredictionControllerTest
    {
        private const int RoundNumber = 5;
        private const string userId = "testUserId";

        private MatchServiceModel firstMatch = new MatchServiceModel()
        {
            HomeTeam = "team1",
            AwayTeam = "team2",
            AwayWinPoints = 1,
            DrawPoints = 1,
            HomeWinPoints = 1,
            Id = 5,
            IsPredictionAllowed = true
        };

        private MatchServiceModel secondMatch = new MatchServiceModel()
        {
            HomeTeam = "team3",
            AwayTeam = "team4",
            AwayWinPoints = 1,
            DrawPoints = 1,
            HomeWinPoints = 1,
            Id = 6,
            IsPredictionAllowed = false
        };

        [Fact]
        public void PredictionControllerShouldBeForAuthorizedUsers()
        {
            // Arrange
            var controller = typeof(PredictionController);

            // Act
            var authorizeAttribute = controller
                .GetCustomAttributes(true)
                .FirstOrDefault(a => a.GetType() == typeof(AuthorizeAttribute))
                as AuthorizeAttribute;

            // Assert
            authorizeAttribute.Should().NotBeNull();
        }

        [Fact]
        public void BetShouldReturnViewWIthValidModel()
        {
            //Arrange
            var userManager = UserManagerMock.New;
            userManager.Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);

            var predictionService = new Mock<IPredictionService>();
            predictionService.Setup(p => p.GetActiveMatches(userId))
                .Returns(new List<MatchServiceModel>()
                {
                firstMatch,secondMatch
                }.AsQueryable());

            predictionService.Setup(r => r.GetActiveRoundNumber()).
                Returns(RoundNumber);

            predictionService.Setup(p => p.IsCurrentRoundPredicted(userId))
                .Returns(false);

            var controller = new PredictionController(predictionService.Object, userManager.Object);

            Mapper.Initialize();

            //Act
            var result = controller.Bet();

            //Assert
            result.Should().BeOfType<ViewResult>();

            var model = result.As<ViewResult>().Model;

            model.Should().BeOfType<ActiveRoundViewModel>();

            var formModel = model.As<ActiveRoundViewModel>();

            formModel.AlreadyPredicted.Should().Be(false);
            formModel.Matches.Count.Should().Be(1);
            formModel.StartedMatches.Count.Should().Be(1);
            formModel.Matches.First().HomeTeam.Should().Be(firstMatch.HomeTeam);
            formModel.Matches.First().AwayTeam.Should().Be(firstMatch.AwayTeam);
            formModel.StartedMatches.First().HomeTeam.Should().Be(secondMatch.HomeTeam);
            formModel.StartedMatches.First().AwayTeam.Should().Be(secondMatch.AwayTeam);
            formModel.RoundNumber.Should().Be(RoundNumber);
        }

        [Fact]
        public void BetPostReturnViewWithModelWhenAnyNonPredictedMatch()
        {
            // Arrange
            string errorMessage = null;
            ActiveRoundViewModel model = GetActiveRoundViewModel();
            var tempData = new Mock<ITempDataDictionary>();
            tempData
                .SetupSet(t => t[WebConstants.TempDataErrorMessageKey] = It.IsAny<string>())
                .Callback((string key, object message) => errorMessage = message as string);


            var controller = new PredictionController(null, null);
            controller.TempData = tempData.Object;
            // Act
            var result = controller.Bet(model);

            // Assert
            result.Should().BeOfType<ViewResult>();

            var viewModel = result.As<ViewResult>().Model;

            viewModel.Should().BeOfType<ActiveRoundViewModel>();

            var formModel = viewModel.As<ActiveRoundViewModel>();

            formModel.AlreadyPredicted.Should().Be(false);
            formModel.Matches.Count.Should().Be(2);
            formModel.Matches.First().HomeTeam.Should().Be(firstMatch.HomeTeam);
            formModel.Matches.First().AwayTeam.Should().Be(firstMatch.AwayTeam);
            formModel.Matches.Last().HomeTeam.Should().Be(secondMatch.HomeTeam);
            formModel.Matches.Last().AwayTeam.Should().Be(secondMatch.AwayTeam);
            formModel.RoundNumber.Should().Be(RoundNumber);

            errorMessage.Should().Be(MessageResources.msgBetAllMatches);
        }

        [Fact]
        public void BetPostSuccessfulySavedScoreAndRedirectToGetBet()
        {
            // Arrange
            ActiveRoundViewModel model = GetActiveRoundViewModel(ResultEnum.DRAW);
            string successMessage = null;
            var tempData = new Mock<ITempDataDictionary>();
            tempData
                .SetupSet(t => t[WebConstants.TempDataSuccessMessageKey] = It.IsAny<string>())
                .Callback((string key, object message) => successMessage = message as string);

            var userManager = UserManagerMock.New;
            userManager.Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);

            var predictionService = new Mock<IPredictionService>();
            predictionService.Setup(p => p.GetAllActiveMatchesIds())
                .Returns(new List<int>()
                {
                firstMatch.Id,secondMatch.Id
                });

            predictionService.Setup(p => p.AddPrediction(null, null));
            var controller = new PredictionController(predictionService.Object, userManager.Object);
            controller.TempData = tempData.Object;

            // Act
            var result = controller.Bet(model);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            redirectToActionResult.ControllerName.Should().Be(null);
            redirectToActionResult.ActionName.Should().BeEquivalentTo("Bet");
            successMessage.Should().Be(MessageResources.msgSuccessfulPredictions);
        }

        private ActiveRoundViewModel GetActiveRoundViewModel(ResultEnum? result = null)
        => new ActiveRoundViewModel()
        {
            Matches = new List<MatchViewModel>()
               {
                   GetMatchViewModel(firstMatch,result),
                   GetMatchViewModel(secondMatch,result)
               },
            AlreadyPredicted = false,
            RoundNumber = RoundNumber,
        };

        private MatchViewModel GetMatchViewModel(MatchServiceModel model, ResultEnum? result = null)
        {
            return new MatchViewModel
            {
                HomeTeam = model.HomeTeam,
                AwayTeam = model.AwayTeam,
                AwayWinPoints = model.AwayWinPoints,
                DrawPoints = model.DrawPoints,
                HomeWinPoints = model.HomeWinPoints,
                Id = model.Id,
                UserPrediction = result
            };
        }
    }
}

