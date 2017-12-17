namespace BetINK.Web.Controllers
{
    using BetINK.Services.Interfaces.UserInteractions;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    public class ChartController : Controller
    {
        private readonly IChartService chartService;

        public ChartController(IChartService chartService)
        {
            this.chartService = chartService;
        }

        public IActionResult Index()
        {
            var model = this.chartService.GetUsersChart();
            return View(model);
        }

    }
}