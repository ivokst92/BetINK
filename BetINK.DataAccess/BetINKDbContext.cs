namespace BetINK.Web.Data
{
    using BetINK.DataAccess.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class BetINKDbContext : IdentityDbContext<User>
    {
        public BetINKDbContext(DbContextOptions<BetINKDbContext> options)
            : base(options)
        {
        }

        public DbSet<League> Leagues { get; set; }

        public DbSet<Match> Matches { get; set; }

        public DbSet<Prediction> Predictions { get; set; }

        public DbSet<Round> Rounds { get; set; }

        public DbSet<Season> Seasons { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<FootballLeague> TeamInLeagues { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<FootballLeague>()
                .HasKey(tl => new { tl.LeagueId, tl.TeamId });

            builder.Entity<FootballLeague>()
                .HasOne(tl => tl.Team)
                .WithMany(l => l.Leagues)
                .HasForeignKey(t => t.TeamId);

            builder.Entity<FootballLeague>()
               .HasOne(tl => tl.League)
               .WithMany(c => c.Teams)
               .HasForeignKey(l => l.LeagueId);

            builder.Entity<Prediction>()
               .HasKey(p => new { p.MatchId, p.UserId });

            builder.Entity<Prediction>()
               .HasOne(p => p.User)
               .WithMany(p => p.Predictions)
               .HasForeignKey(t => t.UserId);

            builder.Entity<Prediction>()
               .HasOne(p => p.Match)
               .WithMany(p => p.Predictions)
               .HasForeignKey(p => p.MatchId);

            builder.Entity<Round>()
              .HasOne(p => p.Season)
              .WithMany(p => p.Rounds)
              .HasForeignKey(p => p.SeasonId);

            builder.Entity<Round>()
             .HasMany(p => p.Matches)
             .WithOne(p => p.Round)
             .HasForeignKey(p => p.RoundId);

            builder.Entity<Match>()
                .HasOne(x => x.League)
                .WithMany(x => x.Matches)
                .HasForeignKey(x => x.LeagueId)
                .IsRequired(false);

            base.OnModelCreating(builder);
        }
    }
}
