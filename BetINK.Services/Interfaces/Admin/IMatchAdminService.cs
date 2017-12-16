namespace BetINK.Services.Interfaces.Admin
{
    using System;
    using BetINK.Services.Models.Match;
    using System.Collections.Generic;
    using BetINK.Common.Enums;

    public interface IMatchAdminService
    {
        IEnumerable<MatchAdminListingServiceModel> All(int roundId);

        MatchAdminServiceModel ByIdAndRoundId(int matchId, int roundId);

        void Create(string homeTeam,
                    string awayTeam,
                    decimal homeWinPoints,
                    decimal awayWinPoints,
                    decimal drawPoints,
                    DateTime matchStart,
                    ResultEnum? result,
                    int roundId,
                    int? leagueId);

        bool RoundExists(int roundId);

        void Edit(int id,
                  string homeTeam,
                  string awayTeam,
                  decimal homeWinPoints,
                  decimal awayWinPoints,
                  decimal drawPoints,
                  DateTime matchStart,
                  ResultEnum? result);

        void Delete(int id);
    }
}
