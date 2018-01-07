using System.Collections.Generic;

namespace BetINK.Web.Models.Prediction
{
    public class UserPredictionsViewModel : PredictionViewModel
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<int> Rounds { get; set; }
    }
}
