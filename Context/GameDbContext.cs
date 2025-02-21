using game2048cs.model;

namespace game2048cs.Context;

using Microsoft.EntityFrameworkCore;


public class GameDbContext : DbContext
{
    public DbSet<GameScore> Scores { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=game2048.db");
    }
}
