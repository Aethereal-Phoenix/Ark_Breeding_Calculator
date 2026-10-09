using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Calculator_API.Data;

public class CalculatorContext : DbContext
{
    // Represents the Dinosaur and BreedingLine tables in the database
    // EF Core maps database records to these model classes
    public DbSet<Dinosaur> Dinosaurs { get; set; } = null!;
    public DbSet<Stats> Stats { get; set; } = null!;
    public DbSet<MutatedStats> MutatedStats { get; set; } = null!;
    public DbSet<BreedingLine> BreedingLines { get; set; } = null!;

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
            // Specifies the database table for this entity
            entity.ToTable("breeding_line");

            // Configures the primary key
            entity.HasKey(e => e.BreedingLineId);

            // Maps entity properties to the database columns
            entity.Property(e => e.BreedingLineId).HasMaxLength(36).HasColumnName("breeding_line_id");
            
            entity.Property(e => e.Species).HasMaxLength(25).HasColumnName("species_id");

            entity.Property(e => e.BreedingLineName).HasMaxLength(50).HasColumnName("breeding_line_name");
        });
        #endregion

        #region Stats entity
        modelBuilder.Entity<Stats>(entity =>
        {
            // Specifies the database table for this entity
            entity.ToTable("stats");

            // Configures the primary key
            entity.HasKey(e => e.DinosaurId);

            // Maps entity properties to the database columns
            entity.Property(e => e.DinosaurId).HasMaxLength(36).HasColumnName("dinosaur_id");

            entity.Property(e => e.Health).HasColumnName("health");

            entity.Property(e => e.Stamina).HasColumnName("stamina");

            entity.Property(e => e.Oxygen).HasColumnName("oxygen");

            entity.Property(e => e.Food).HasColumnName("food");

            entity.Property(e => e.Water).HasColumnName("water");

            entity.Property(e => e.Weight).HasColumnName("weight");

            entity.Property(e => e.Melee).HasColumnName("melee");

            entity.Property(e => e.MovementSpeed).HasColumnName("movement_speed");

            // Sets up the FK relationship
            entity.HasOne<Dinosaur>().WithOne(d => d.Stats).HasForeignKey<Stats>(s => s.DinosaurId).OnDelete(DeleteBehavior.Cascade);
        });
        #endregion

        #region MutatedStats entity
        // Configures the Stats table mapping
        modelBuilder.Entity<MutatedStats>(entity =>
        {
            // Specifies the database table for this entity
            entity.ToTable("mutated_stats");

            // Configures the primary key
            entity.HasKey(e => e.DinosaurId);

            // Maps entity properties to the database columns
            entity.Property(e => e.DinosaurId).HasMaxLength(36).HasColumnName("dinosaur_id");

            entity.Property(e => e.Health).HasColumnName("health");

            entity.Property(e => e.Stamina).HasColumnName("stamina");

            entity.Property(e => e.Oxygen).HasColumnName("oxygen");

            entity.Property(e => e.Food).HasColumnName("food");

            entity.Property(e => e.Water).HasColumnName("water");

            entity.Property(e => e.Weight).HasColumnName("weight");

            entity.Property(e => e.Melee).HasColumnName("melee");

            entity.Property(e => e.MovementSpeed).HasColumnName("movement_speed");
        });
        #endregion

        #region Dinosaur region
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

            entity.Property(e => e.Mutated).HasColumnName("mutated");

            entity.Property(e => e.AquaticDino).HasColumnName("aquatic_dino");

            entity.Property(e => e.FatherId).HasMaxLength(36).HasColumnName("father_id");

            entity.Property(e => e.MotherId).HasMaxLength(36).HasColumnName("mother_id");



            // Configure the FK relationship
            entity.HasOne<BreedingLine>().WithMany(b => b.Dinosaurs).HasForeignKey(d => d.BreedingLineId).OnDelete(DeleteBehavior.Restrict);
        });
        #endregion
    }
    #endregion 
}