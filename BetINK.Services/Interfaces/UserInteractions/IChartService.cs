namespace BetINK.Services.Interfaces.UserInteractions
{
    using BetINK.Services.Models.Chart;
    using System.Collections.Generic;

    public interface IChartService
    {
        IEnumerable<ChartServiceModel> GetUsersChart();

        Dictionary<string, string> GetLeagues();
    }
}
