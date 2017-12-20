namespace BetINK.Web.Controllers
{
    using BetINK.Services.Interfaces.UserInteractions;
    using BetINK.Services.Models.Chart;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using System.Collections.Generic;

    public class ChartController : Controller
    {
        private const string ChartCacheKey = "ChartCache";
        private readonly IChartService chartService;
        private readonly IMemoryCache memoryCache;
        public ChartController(IChartService chartService, IMemoryCache memoryCache)
        {
            this.chartService = chartService;
            this.memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            IEnumerable<ChartServiceModel> model;
            
            if (!memoryCache.TryGetValue(ChartCacheKey, out model))
            {
                model = this.chartService.GetUsersChart();
                
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));
                
                memoryCache.Set(ChartCacheKey, model, cacheEntryOptions);
            }
            return View(model);
        }


        public IActionResult Standings()
        => View(this.chartService.GetLeagues());
    }
}