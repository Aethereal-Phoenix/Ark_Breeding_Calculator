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
    // Configures the database connection used by EF Core.
    // This method runs when the DbContext is created and tells EF Core
    // which database provider to use and where the SQLite database file is stored.
    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        // Builds the full path to the database file.
        // Directory.GetCurrentDirectory() returns the project's root folder
        // when the application is launched during development.
        var dbPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Data",
            "dinosaur_database.db");

        // Registers SQLite as the database provider and tells EF Core
        // which database file to connect to.
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }
    #endregion

    // Configures the entity mappings for the database tables
    #region OnModelCreating
    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        #region BreedingLine entity
        // Configures how the BreedingLine class is mapped to the
        // breeding_line table in the SQLite database.
        modelBuilder.Entity<BreedingLine>(entity =>
        {
            // Maps the BreedingLine entity to the breeding_line table in the database.
            entity.ToTable("breeding_line");

            // Defines BreedingLineId as the table's primary key.
            entity.HasKey(e => e.BreedingLineId);

            // Maps the BreedingLineId property to the breeding_line_id column.
            entity.Property(e => e.BreedingLineId)
                .HasColumnName("breeding_line_id");

            // Maps the Species property to the species column.
            entity.Property(e => e.Species)
                .HasColumnName("species");

            // Maps the BreedingLineName property to the breeding_line_name column.
            entity.Property(e => e.BreedingLineName)
                .HasColumnName("breeding_line_name");
        });
        #endregion

        #region Stats entity
        // Configures how the Stats class is mapped to the
        // stats table in the SQLite database.
        modelBuilder.Entity<Stats>(entity =>
        {
            // Maps the Stats entity to the stats table in the database
            entity.ToTable("stats");
            
            // Defines StatsId as the table's primary key
            entity.HasKey(e => e.StatsId);

            // Maps the StatsId property to the stats_id column
            entity.Property(e => e.StatsId).HasColumnName("stats_id");

            // Maps the Health property to the health column
            entity.Property(e => e.Health).HasColumnName("health");

            // Maps the Stamina property to the stamina column
            entity.Property(e => e.Stamina).HasColumnName("stamina");

            // Maps the Oxygen property to the oxygen column
            entity.Property(e => e.Oxygen).HasColumnName("oxygen");

            // Maps the Food property to the food column
            entity.Property(e => e.Food).HasColumnName("food");

            // Maps the Water property to the water column
            entity.Property(e => e.Water).HasColumnName("water");
            
            // Maps the Weight property to the weight column
            entity.Property(e => e.Weight).HasColumnName("weight");

            // Maps the Melee property to the melee column
            entity.Property(e => e.Melee).HasColumnName("melee");
            
            // Maps the MovementSpeed property to the movement_speed column
            entity.Property(e => e.MovementSpeed).HasColumnName("movement_speed");
        });
        #endregion

        #region MutatedStats entity
        // Configures how the MutatedStats class is mapped to the
        // stats table in the SQLite database.
        modelBuilder.Entity<MutatedStats>(entity =>
        {
            // Maps the MutatedStats entity to the mutated_stats table in the database
            entity.ToTable("mutated_stats");

            // Defines MutatedStatsId as the table's primary key
            entity.HasKey(e => e.MutatedStatsId);

            // Maps the MutatedStatsId property to the stats_id column
            entity.Property(e => e.MutatedStatsId).HasColumnName("mutated_stats_id");

            // Maps the Health property to the health column
            entity.Property(e => e.Health).HasColumnName("health");

            // Maps the Stamina property to the stamina column
            entity.Property(e => e.Stamina).HasColumnName("stamina");

            // Maps the Oxygen property to the oxygen column
            entity.Property(e => e.Oxygen).HasColumnName("oxygen");

            // Maps the Food property to the food column
            entity.Property(e => e.Food).HasColumnName("food");

            // Maps the Water property to the water column
            entity.Property(e => e.Water).HasColumnName("water");

            // Maps the Weight property to the weight column
            entity.Property(e => e.Weight).HasColumnName("weight");

            // Maps the Melee property to the melee column
            entity.Property(e => e.Melee).HasColumnName("melee");

            // Maps the MovementSpeed property to the movement_speed column
            entity.Property(e => e.MovementSpeed).HasColumnName("movement_speed");
        });
        #endregion

        #region Dinosaur entity
        // Configures how the Dinosaur class is mapped to the
        // dinosaur table in the SQLite database.
        modelBuilder.Entity<Dinosaur>(entity =>
        {
            // Maps the Dinosaur entity to the dinosaur table
            entity.ToTable("dinosaur");

            // Defines DinosaurId as the table's primary key
            entity.HasKey(e => e.DinosaurId);

            // Maps the DinosaurId property to the dinosaur_id column
            entity.Property(e => e.DinosaurId).HasColumnName("dinosaur_id");

            // Maps the BreedingLineId property to the breeding_line_id column
            entity.Property(e => e.BreedingLineId).HasColumnName("breeding_line_id");

            // Maps the DinosaurName property to the dinosaur_name column
            entity.Property(e => e.DinosaurName).HasMaxLength(100).HasColumnName("dinosaur_name");

            // Maps the Species property to the species column
            entity.Property(e => e.Species).HasColumnName("species");

            // Maps the Gender property to the gender column
            entity.Property(e => e.Gender).HasColumnName("gender");

            // Maps the Generation property to the generation column
            entity.Property(e => e.Generation).HasColumnName("generation");

            // Maps the StatsId property to the stats_id column
            entity.Property(e => e.StatsId).HasColumnName("stats_id");

            // Maps the MutatedStatsId property to the stats_id column
            entity.Property(e => e.MutatedStatsId).HasColumnName("mutated_stats_id");

            // Maps the Mutated property to the mutated column
            entity.Property(e => e.Mutated).HasColumnName("mutated");

            // Maps the AquaticDino property to the aquatic_dino column
            entity.Property(e => e.AquaticDino).HasColumnName("aquatic_dino");

            // Maps the FatherId property to the father_id column
            entity.Property(e => e.FatherId).HasColumnName("father_id");

            // Maps the MotherId property to the mother_id column
            entity.Property(e => e.MotherId).HasColumnName("mother_id");

            // Configures the relationships between Dinosaur and other entities.
            // These relationships allow EF Core to automatically load related data
            // when using Include().

            // One-to-one relationship:
            // Each Dinosaur references a single Stats record through StatsId.
            entity.HasOne(e => e.Stats).WithOne().HasForeignKey<Dinosaur>(d => d.StatsId);

            // One-to-one relationship:
            // Each Dinosaur references a single MutatedStats record through MutatedStatsId
            entity.HasOne(e => e.MutatedStats).WithOne().HasForeignKey<Dinosaur>(e => e.MutatedStatsId);

            // Self-referencing relationship:
            // FatherId stores the DinosaurId of this dinosaur's father.
            // Restrict delete prevents deleting a parent that is still referenced.
            entity.HasOne(e => e.Father).WithMany().HasForeignKey(e => e.FatherId).OnDelete(DeleteBehavior.Restrict);

            // Self-referencing relationship:
            // MotherId stores the DinosaurId of this dinosaur's mother.
            // Restrict delete prevents deleting a parent that is still referenced.
            entity.HasOne(e => e.Mother).WithMany().HasForeignKey(e => e.MotherId).OnDelete(DeleteBehavior.Restrict);

            // One-to-many relationship:
            // A BreedingLine can contain many Dinosaurs,
            // but each Dinosaur belongs to exactly one BreedingLine.
            entity.HasOne(e => e.BreedingLine).WithMany(d => d.Dinosaurs).HasForeignKey(b => b.BreedingLineId);
        });
        #endregion
    }
    #endregion 
}