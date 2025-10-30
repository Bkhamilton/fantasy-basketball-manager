using Microsoft.EntityFrameworkCore;
using FantasyBasketballApi.Models;

namespace FantasyBasketballApi.Data;

public class FantasyBasketballContext : DbContext
{
    public FantasyBasketballContext(DbContextOptions<FantasyBasketballContext> options)
        : base(options)
    {
    }

    public DbSet<Player> Players { get; set; }
    public DbSet<NbaGame> NbaGames { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Player entity
        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Team).IsRequired().HasMaxLength(10);
            entity.Property(e => e.Position).IsRequired().HasMaxLength(10);
            entity.OwnsOne(e => e.Stats, stats =>
            {
                stats.Property(s => s.Points).HasColumnType("decimal(5,2)");
                stats.Property(s => s.Rebounds).HasColumnType("decimal(5,2)");
                stats.Property(s => s.Assists).HasColumnType("decimal(5,2)");
                stats.Property(s => s.Steals).HasColumnType("decimal(5,2)");
                stats.Property(s => s.Blocks).HasColumnType("decimal(5,2)");
                stats.Property(s => s.FieldGoalPercentage).HasColumnType("decimal(5,2)");
                stats.Property(s => s.ThreePointPercentage).HasColumnType("decimal(5,2)");
            });
            entity.OwnsOne(e => e.Injury, injury =>
            {
                injury.Property(i => i.Status).IsRequired().HasMaxLength(50);
                injury.Property(i => i.Description).HasMaxLength(500);
            });
            entity.OwnsOne(e => e.GameToday, game =>
            {
                game.Property(g => g.Opponent).HasMaxLength(10);
                game.Property(g => g.Time).HasMaxLength(20);
            });
        });

        // Configure NbaGame entity
        modelBuilder.Entity<NbaGame>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.HomeTeam).IsRequired().HasMaxLength(10);
            entity.Property(e => e.AwayTeam).IsRequired().HasMaxLength(10);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
        });
    }
}
