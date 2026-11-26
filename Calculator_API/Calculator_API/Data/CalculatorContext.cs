using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Calculator_API.Data
{
    // ---------------------------------------------------------------------
    // CalculatorContext
    //
    // Purpose: EF Core database context used to communicate with the
    // SQLite database.
    //
    // Layering (cheat sheet):
    //
    // Controller -> calls the DAL
    // DAL -> uses CalculatorContext
    // CalculatorContext -> translates C# objects into SQL operations
    // SQLite Database -> stores the application data
    //
    // EF Core uses this class to:
    // - Connect to the database
    // - Configure table mappings
    // - Configure entity relationships
    // - Translate entities into database records
    // - Translate database records into entities
    // ---------------------------------------------------------------------
    public class CalculatorContext : DbContext
    {
        // Application configuration settings.
        //
        // Allows values from appsettings.json and other configuration
        // providers to be accessed when needed.
        private readonly IConfiguration _config;

        #region Constructors

        // Constructor used when only IConfiguration is provided.
        //
        // This allows the DbContext to access application configuration
        // settings, such as values stored in appsettings.json.
        public CalculatorContext(IConfiguration config)
        {
            _config = config;
        }

        // Constructor used by ASP.NET dependency injection during normal
        // application execution.
        //
        // DbContextOptions contains the database configuration while
        // IConfiguration provides access to application settings.
        //
        // : base(options) passes the database configuration to EF Core's
        // DbContext class so it knows how to connect to and
        // configure the database.
        public CalculatorContext(
            DbContextOptions<CalculatorContext> options,
            IConfiguration config) : base(options)
        {
            _config = config;
        }

        // Constructor primarily used during testing or when the database
        // configuration is supplied externally.
        //
        // This is commonly used with EF Core's InMemory database provider
        // because tests usually don't require access to IConfiguration.
        //
        // : base(options) passes the database configuration to EF Core's
        // DbContext class so it knows how to connect to
        // and configure the database.
        public CalculatorContext(
            DbContextOptions<CalculatorContext> options) : base(options)
        {
        }

        #endregion

        #region EF Core mappings

        // DbSet properties represent tables in the database.
        //
        // EF Core uses these properties to query, insert,
        // update, and delete records from their associated tables.
        public DbSet<Dinosaur> Dinosaurs { get; set; } = null!;

        public DbSet<Stats> Stats { get; set; } = null!;

        public DbSet<MutatedStats> MutatedStats { get; set; } = null!;

        public DbSet<BreedingLine> BreedingLines { get; set; } = null!;

        #endregion

        #region OnConfiguring

        // Configures the database connection used by EF Core.
        //
        // This method runs when the DbContext is created.
        //
        // If a database provider has not already been configured
        // through dependency injection, EF Core will configure
        // a SQLite connection using the local database file.
        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            // Guard clause: if a database provider has already been
            // configured elsewhere, do not overwrite it.
            //
            // This allows dependency injection and testing frameworks
            // to provide their own database configuration when needed.
            if (!optionsBuilder.IsConfigured)
            {
                // Build the full path to the SQLite database file.
                //
                // Directory.GetCurrentDirectory() typically returns
                // the application's root folder while running locally.
                var dbPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Data",
                    "dinosaur_database.db");

                // Register SQLite as the database provider and tell
                // EF Core which database file should be used.
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
        }

        #endregion

        #region OnModelCreating
        // Configures how entities are mapped to database tables.
        //
        // This method runs when EF Core builds the application's
        // database model.
        //
        // The configuration below defines:
        //
        // - Table names
        // - Primary keys
        // - Column mappings
        // - Entity relationships
        // - Foreign key relationships
        //
        // Without these mappings, EF Core would rely entirely on
        // naming conventions to determine how the database should
        // be structured.
        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            #region BreedingLine entity

            // Configures how the BreedingLine entity maps to
            // the breeding_line table in SQLite.
            //
            // A BreedingLine represents a collection of related
            // dinosaurs that belong to the same breeding project.
            modelBuilder.Entity<BreedingLine>(entity =>
            {
                // Table name used in the database.
                entity.ToTable("breeding_line");

                // Primary key for the breeding_line table.
                entity.HasKey(e => e.BreedingLineId);

                // Property-to-column mappings.
                entity.Property(e => e.BreedingLineId)
                    .HasColumnName("breeding_line_id");

                entity.Property(e => e.Species)
                    .HasColumnName("species");

                entity.Property(e => e.BreedingLineName)
                    .HasColumnName("breeding_line_name");
            });

            #endregion

            #region Stats entity

            // Configures how the Stats entity maps to the
            // stats table in SQLite.
            //
            // Stats contain the dinosaur's base values such as:
            // Health, Stamina, Oxygen, Food, Weight, and Melee.
            modelBuilder.Entity<Stats>(entity =>
            {
                // Table name used in the database.
                entity.ToTable("stats");

                // Primary key for the stats table.
                entity.HasKey(e => e.StatsId);

                // Property-to-column mappings.
                entity.Property(e => e.StatsId)
                    .HasColumnName("stats_id");

                entity.Property(e => e.Health)
                    .HasColumnName("health");

                entity.Property(e => e.Stamina)
                    .HasColumnName("stamina");

                entity.Property(e => e.Oxygen)
                    .HasColumnName("oxygen");

                entity.Property(e => e.Food)
                    .HasColumnName("food");

                entity.Property(e => e.Water)
                    .HasColumnName("water");

                entity.Property(e => e.Weight)
                    .HasColumnName("weight");

                entity.Property(e => e.Melee)
                    .HasColumnName("melee");

                entity.Property(e => e.MovementSpeed)
                    .HasColumnName("movement_speed");
            });

            #endregion

            #region MutatedStats entity

            // Configures how the MutatedStats entity maps to the
            // mutated_stats table in SQLite.
            //
            // MutatedStats store stat values after mutations have
            // occurred and allow mutation progress to be tracked
            // separately from the dinosaur's base stats.
            modelBuilder.Entity<MutatedStats>(entity =>
            {
                // Table name used in the database.
                entity.ToTable("mutated_stats");

                // Primary key for the mutated_stats table.
                entity.HasKey(e => e.MutatedStatsId);

                // Property-to-column mappings.
                entity.Property(e => e.MutatedStatsId)
                    .HasColumnName("mutated_stats_id");

                entity.Property(e => e.Health)
                    .HasColumnName("health");

                entity.Property(e => e.Stamina)
                    .HasColumnName("stamina");

                entity.Property(e => e.Oxygen)
                    .HasColumnName("oxygen");

                entity.Property(e => e.Food)
                    .HasColumnName("food");

                entity.Property(e => e.Water)
                    .HasColumnName("water");

                entity.Property(e => e.Weight)
                    .HasColumnName("weight");

                entity.Property(e => e.Melee)
                    .HasColumnName("melee");

                entity.Property(e => e.MovementSpeed)
                    .HasColumnName("movement_speed");
            });

            #endregion

            #region Dinosaur entity

            // Configures how the Dinosaur entity maps to the
            // dinosaur table in SQLite.
            //
            // This is the most complex entity in the application
            // because it contains relationships to:
            //
            // - BreedingLine
            // - Stats
            // - MutatedStats
            // - Father
            // - Mother
            //
            // EF Core uses the relationship configuration below
            // to automatically load related data when Include()
            // is used in queries.
            modelBuilder.Entity<Dinosaur>(entity =>
            {
                // Table name used in the database.
                entity.ToTable("dinosaur");

                // Primary key for the dinosaur table.
                entity.HasKey(e => e.DinosaurId);

                // Property-to-column mappings.
                entity.Property(e => e.DinosaurId)
                    .HasColumnName("dinosaur_id");

                entity.Property(e => e.BreedingLineId)
                    .HasColumnName("breeding_line_id");

                entity.Property(e => e.DinosaurName)
                    .HasMaxLength(100)
                    .HasColumnName("dinosaur_name");

                entity.Property(e => e.Species)
                    .HasColumnName("species");

                entity.Property(e => e.Gender)
                    .HasColumnName("gender");

                entity.Property(e => e.Generation)
                    .HasColumnName("generation");

                entity.Property(e => e.StatsId)
                    .HasColumnName("stats_id");

                entity.Property(e => e.MutatedStatsId)
                    .HasColumnName("mutated_stats_id");

                entity.Property(e => e.Mutated)
                    .HasColumnName("mutated");

                entity.Property(e => e.AquaticDino)
                    .HasColumnName("aquatic_dino");

                entity.Property(e => e.FatherId)
                    .HasColumnName("father_id");

                entity.Property(e => e.MotherId)
                    .HasColumnName("mother_id");

                // -----------------------------------------------------------
                // Relationship Configuration
                // -----------------------------------------------------------

                // One-to-one relationship:
                //
                // Each Dinosaur references exactly one Stats record.
                //
                // Dinosaur.StatsId -> Stats.StatsId
                entity.HasOne(e => e.Stats)
                    .WithOne()
                    .HasForeignKey<Dinosaur>(d => d.StatsId);

                // One-to-one relationship:
                //
                // Each Dinosaur references exactly one MutatedStats record.
                //
                // Dinosaur.MutatedStatsId -> MutatedStats.MutatedStatsId
                entity.HasOne(e => e.MutatedStats)
                    .WithOne()
                    .HasForeignKey<Dinosaur>(e => e.MutatedStatsId);

                // Self-referencing relationship:
                //
                // A Dinosaur can reference another Dinosaur as its father.
                //
                // Example:
                //
                // Baby Shadowmane
                //     FatherId -> Adult Male Shadowmane
                //
                // Restrict delete prevents a parent dinosaur from being
                // deleted while it is still referenced by offspring.
                entity.HasOne(e => e.Father)
                    .WithMany()
                    .HasForeignKey(e => e.FatherId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Self-referencing relationship:
                //
                // A Dinosaur can reference another Dinosaur as its mother.
                //
                // Restrict delete prevents a parent dinosaur from being
                // deleted while it is still referenced by offspring.
                entity.HasOne(e => e.Mother)
                    .WithMany()
                    .HasForeignKey(e => e.MotherId)
                    .OnDelete(DeleteBehavior.Restrict);

                // One-to-many relationship:
                //
                // One BreedingLine can contain many Dinosaurs.
                //
                // Example:
                //
                // Shadowmane Breeding Line
                //     ├─ Dino A
                //     ├─ Dino B
                //     └─ Dino C
                //
                // Each Dinosaur can belong to only one BreedingLine.
                entity.HasOne(e => e.BreedingLine)
                    .WithMany(d => d.Dinosaurs)
                    .HasForeignKey(b => b.BreedingLineId);
            });

            #endregion
        }

        #endregion
    }
}