using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Calculator_API.Data;

public class CalculatorContext : DbContext
{
    // Represents the Dinosaur and BreedingLine tables in the database
    // EF Core maps database records to these model classes
    public DbSet<Dinosaur> Dinosaurs { get; set; } = null!;
    public DbSet<Stats> Stats { get; set; } = null!;
    public DbSet<Stats> MutatedStats { get; set; } = null!;
    public DbSet<BreedingLine> BreedingLines { get; set; } = null!;

    // Configures the SQLite database connection
    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = Path.Combine
            (AppContext.BaseDirectory,
            "Data",
            "dinosaur_database.db");

        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }

    // Configures the entity mappings for the database tables

    // Configures the BreedingLine table mapping
    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BreedingLine>(entity =>
        {
            // Specifies the database table for this entity
            entity.ToTable("breeding_line");

            // Configures the primary key
            entity.HasKey(e => e.BreedingLineId);

            // Maps entity properties to the database columns
            entity.Property(e => e.BreedingLineId)
                .HasColumnName("breeding_line_id");

            entity.Property(e => e.Species)
                .HasColumnName("species_id");

            entity.Property(e => e.BreedingLineName)
                .HasColumnName("breeding_line_name")
                .HasMaxLength(50);
        });

        // Configures the Stats table mapping
        modelBuilder.Entity<Stats>(entity =>
        {
            // Specifies the database table for this entity
            entity.ToTable("stats");

            // Configures the primary key
            entity.HasKey(e => e.DinosaurId);

            // Maps entity properties to the database columns
            entity.Property(e => e.DinosaurId).HasColumnName("dinosaur_id");

            entity.Property(e => e.Health).HasColumnName("health");

            entity.Property(e => e.Stamina).HasColumnName("oxygen");

            entity.Property(e => e.Food).HasColumnName("food");

            entity.Property(e => e.Water).HasColumnName("water");

            entity.Property(e => e.Weight).HasColumnName("weight");

            entity.Property(e => e.Melee).HasColumnName("melee");

            entity.Property(e => e.MovementSpeed).HasColumnName("movement_speed");
        });

        // Configures the Stats table mapping
        modelBuilder.Entity<Stats>(entity =>
        {
            // Specifies the database table for this entity
            entity.ToTable("mutated_stats");

            // Configures the primary key
            entity.HasKey(e => e.DinosaurId);

            // Maps entity properties to the database columns
            entity.Property(e => e.DinosaurId).HasColumnName("dinosaur_id");

            entity.Property(e => e.Health).HasColumnName("health");

            entity.Property(e => e.Stamina).HasColumnName("oxygen");

            entity.Property(e => e.Food).HasColumnName("food");

            entity.Property(e => e.Water).HasColumnName("water");

            entity.Property(e => e.Weight).HasColumnName("weight");

            entity.Property(e => e.Melee).HasColumnName("melee");

            entity.Property(e => e.MovementSpeed).HasColumnName("movement_speed");
        });

        // Configures the Dinosaur table mapping
        modelBuilder.Entity<Dinosaur>(entity =>
        {
            // Specifies the database table for this entity
            entity.ToTable("dinosaur");

            // Configures the primary key
            entity.HasKey(e => e.DinosaurId);

            // Maps entity properties to the database columns
            entity.Property(e => e.DinosaurId)
                .HasColumnName("dinosaur_id");

            entity.Property(e => e.BreedingLineId)
                .HasColumnName("breeding_line_id");

            entity.Property(e => e.DinosaurName)
                .HasColumnName("dinosaur_name")
                .HasMaxLength(100);

            // Configure the FK relationship
            entity.HasOne(d => d.BreedingLine).WithMany(b => b.Dinosaurs).HasForeignKey(d => d.BreedingLineId);
        });
    }
}