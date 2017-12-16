namespace BetINK.DataAccess.Models
{
    public class FootballLeague
    {
        public int TeamId { get; set; }

        public Team Team { get; set; }

        public int LeagueId { get; set; }

        public League League { get; set; }
    }
}
