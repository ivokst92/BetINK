namespace BetINK.Tests.Web.Controllers
{
    using BetINK.Services.Interfaces.UserInteractions;
    using BetINK.Services.Models.Chart;
    using BetINK.Web.Controllers;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class ChartControllerTest
    {
        private readonly Mock<IChartService> chartService = new Mock<IChartService>();

        private const int firstPoints = 50;

        private const int secondPoints = 100;

        private readonly ChartServiceModel firstUser = new ChartServiceModel()
        {
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            Points = firstPoints,
            Username = Guid.NewGuid().ToString()
        };

        private readonly ChartServiceModel secondUser = new ChartServiceModel()
        {
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            Points = secondPoints,
            Username = Guid.NewGuid().ToString()
        };

        [Fact]
        public void IndexShouldReturnUsersWithPoints()
        {
            // Arrange
            chartService.Setup(p => p.GetUsersChart())
                .Returns(new List<ChartServiceModel>()
                {
                    firstUser,secondUser
                });

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var controller = new ChartController(chartService.Object, memoryCache);
            
            // Act
            var result = controller.Index();

            // Assert
            AssertIndex(result);
        }

        [Fact]
        public void IndexShouldReturnCachedUsersWithPoints()
        {
            // Arrange
            chartService.Setup(p => p.GetUsersChart())
                .Returns(new List<ChartServiceModel>()
                {
                    firstUser,secondUser
                });

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var controller = new ChartController(chartService.Object, memoryCache);

            controller.Index();

            chartService.Setup(p => p.GetUsersChart())
                .Returns(new List<ChartServiceModel>()
                {
                    firstUser,firstUser,firstUser
                });

            // Act
            var result = controller.Index();

            // Assert
            AssertIndex(result);
        }
        [Fact]
        public void StandingsShouldReturnLeaguesCollection()
        {
            // Arrange
            const string firstLeagueKey = "premier-league";
            const string firstLeagueValue = "PremierLeague";
            const string secondLeagueKey = "la-league";
            const string secondLeagueValue = "La League";
            var chartService = new Mock<IChartService>();
            chartService.Setup(p => p.GetLeagues())
                .Returns(new Dictionary<string, string> {
                     {firstLeagueKey,firstLeagueValue},
                     {secondLeagueKey,secondLeagueValue}
                });

            var controller = new ChartController(chartService.Object, null);

            // Act 
            var result = controller.Standings();

            // Assert
            result.Should().BeOfType<ViewResult>();

            var model = result.As<ViewResult>().Model;

            model.Should().BeOfType<Dictionary<string, string>>();

            var formModel = model.As<Dictionary<string, string>>();

            formModel.Should().ContainKey(firstLeagueKey);
            formModel.Should().ContainValue(firstLeagueValue);
            formModel.Should().ContainKey(secondLeagueKey);
            formModel.Should().ContainValue(secondLeagueValue);
        }

        private void AssertIndex(IActionResult result)
        {
            result.Should().BeOfType<ViewResult>();

            var model = result.As<ViewResult>().Model;

            model.Should().BeOfType<List<ChartServiceModel>>();

            var formModel = model.As<List<ChartServiceModel>>();

            formModel.First().Points.Should().Be(firstPoints);
            formModel.Last().Points.Should().Be(secondPoints);
        }
    }
}
