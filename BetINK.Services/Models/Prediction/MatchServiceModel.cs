namespace BetINK.Services.Models.Prediction
{
    using AutoMapper;
    using BetINK.Common.Enums;
    using BetINK.Common.Mapping;
    using BetINK.DataAccess.Models;
    using System;
    using System.Linq;

    public class MatchServiceModel : IMapFrom<Match>, ICustomMapping
    {
        public int Id { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public ResultEnum? Result { get; set; }
        public decimal HomeWinPoints { get; set; }
        public decimal DrawPoints { get; set; }
        public decimal AwayWinPoints { get; set; }
        public ResultEnum? UserPrediction { get; set; }
        public bool IsPredictionAllowed { get; set; }

        public void ConfigureMapping(Profile mapper)
        {
            string userId = null;
            mapper.CreateMap<Match, MatchServiceModel>()
                .ForMember(u => u.UserPrediction, cfg =>
                    cfg.MapFrom(u => u.Predictions
                      .Where(x => x.UserId == userId)
                      .Select(x => x.MatchPrediction).FirstOrDefault()))
                .ForMember(u => u.IsPredictionAllowed, cfg =>
                cfg.MapFrom(u => u.MatchStart > DateTime.Now));
        }
    }
}
