using BetINK.Services.Models.Chart;
using System.Collections.Generic;

namespace BetINK.Services.Interfaces.UserInteractions
{
    public interface IChartService
    {
        IEnumerable<ChartServiceModel> GetUsersChart();
    }
}
