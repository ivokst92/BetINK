namespace BetINK.Services.Interfaces.Moderator
{
    using BetINK.Services.Models.Match;
    using System.Collections.Generic;
    using System;

    public interface IMatchService
    {
        IEnumerable<MatchListingServiceModel> All(int roundId, string username);

        IEnumerable<string> AllTeams(int leagueId);

        void Create(string homeTeam,
                    string awayTeam,
                    decimal homeWinPoints,
                    decimal drawPoints,
                    decimal awayWinPoints,
                    DateTime matchStart,
                    int roundId,
                    int? leagueId);


        MatchServiceModel ByIdAndRoundId(int matchId, int roundId);

        void Edit(int id,
                  string homeTeam,
                  string awayTeam,
                  decimal homeWinPoints,
                  decimal drawPoints,
                  decimal awayWinPoints,
                  DateTime matchStart);

    }
}
