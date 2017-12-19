namespace BetINK.Web.Controllers
{
    using BetINK.Services.Interfaces.UserInteractions;
    using Microsoft.AspNetCore.Mvc;

    public class ChartController : Controller
    {
        private readonly IChartService chartService;

        public ChartController(IChartService chartService)
        {
            this.chartService = chartService;
        }

        public IActionResult Index()
            => View(this.chartService.GetUsersChart());

        public IActionResult Standings()
        => View(this.chartService.GetLeagues());
    }
}