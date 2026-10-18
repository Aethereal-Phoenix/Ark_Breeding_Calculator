using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Calculator_API.Data;

public class CalculatorContext : DbContext
{
    private readonly IConfiguration _config;

    public CalculatorContext(IConfiguration config)
    {
        _config = config;
    }

    // Represents the Dinosaur and BreedingLine tables in the database
    // EF Core maps database records to these model classes
    #region EF Core mappings
    public DbSet<Dinosaur> Dinosaurs { get; set; } = null!;
    public DbSet<Stats> Stats { get; set; } = null!;
    public DbSet<MutatedStats> MutatedStats { get; set; } = null!;
    public DbSet<BreedingLine> BreedingLines { get; set; } = null!;
    #endregion

    // Configures the SQLite database connection
    #region OnConfiguring
    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = Path.Combine
            (AppContext.BaseDirectory,
            "Data",
            "dinosaur_database.db");

        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }
    #endregion

    // Configures the entity mappings for the database tables
    #region OnModelCreating
    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        #region BreedingLine entity
        modelBuilder.Entity<BreedingLine>(entity =>
        {
            // !TODO: Remake this entity
        });
        #endregion

        #region Stats entity
        modelBuilder.Entity<Stats>(entity =>
        {
            // !TODO: Remake this entity
        });
        #endregion

        #region MutatedStats entity
        // Configures the Stats table mapping
        modelBuilder.Entity<MutatedStats>(entity =>
        {
            // !TODO: Remake this entity
        });
        #endregion

        #region Dinosaur entity
        modelBuilder.Entity<Dinosaur>(entity =>
        {
            // Specifies the database table for this entity
            entity.ToTable("dinosaur");

            // Configures the primary key
            entity.HasKey(e => e.DinosaurId);

            // Maps entity properties to the database columns
            entity.Property(e => e.DinosaurId).HasMaxLength(36).HasColumnName("dinosaur_id");

            entity.Property(e => e.BreedingLineId).HasMaxLength(36).HasColumnName("breeding_line_id");

            entity.Property(e => e.DinosaurName).HasMaxLength(100).HasColumnName("dinosaur_name");

            entity.Property(e => e.Species).HasColumnName("species");

            entity.Property(e => e.Gender).HasColumnName("gender");

            entity.Property(e => e.Generation).HasColumnName("generation");

            entity.Property(e => e.StatsId).HasColumnName("stats_id");

            entity.Property(e => e.MutatedStatsId).HasColumnName("mutated_stats_id");

            entity.Property(e => e.Mutated).HasColumnName("mutated");

            entity.Property(e => e.AquaticDino).HasColumnName("aquatic_dino");

            entity.Property(e => e.FatherId).HasMaxLength(36).HasColumnName("father_id");

            entity.Property(e => e.MotherId).HasMaxLength(36).HasColumnName("mother_id");

            // Configure the FK relationship
            entity.HasOne(e => e.Stats).WithOne().HasForeignKey<Dinosaur>(d => d.StatsId);

            entity.HasOne(e => e.MutatedStats).WithOne().HasForeignKey<Dinosaur>(e => e.MutatedStatsId);

            entity.HasOne(e => e.Father).WithMany().HasForeignKey(e => e.FatherId).OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Mother).WithMany().HasForeignKey(e => e.MotherId).OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.BreedingLine).WithMany(d => d.Dinosaurs).HasForeignKey(b => b.BreedingLineId);
        });
        #endregion
    }
    #endregion 
}