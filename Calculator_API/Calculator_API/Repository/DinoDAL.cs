using Calculator_API.Data;
using Calculator_API.IRepository;
using Microsoft.EntityFrameworkCore;
using Calculator_API.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Calculator_API.Repository
{
    // ---------------------------------------------------------------------
    // DinoDAL
    //
    // Purpose: Data Access Layer (DAL) for all dinosaur-related database
    // operations.
    //
    // Layering (cheat sheet):
    // Controller -> receives requests and builds response DTOs.
    // DinoDAL -> reads/writes data using EF Core.
    // Database -> stores BreedingLines, Dinosaurs, Stats, and MutatedStats.
    //
    // This class is responsible for database communication only.
    // It does not handle HTTP requests or build API response objects.
    // ---------------------------------------------------------------------
    public class DinoDAL : IDino
    {
        #region Constructor

        // Database context used for all EF Core database operations.
        private readonly CalculatorContext context;

        // Application configuration.
        // Currently not used inside this class but remains available
        // for future settings and dependency injection requirements.
        private readonly IConfiguration _config;

        // ASP.NET dependency injection automatically provides the
        // database context and configuration during application startup.
        public DinoDAL(CalculatorContext Context, IConfiguration config)
        {
            context = Context;
            _config = config;
        }

        #endregion

        #region Get All BreedingLines Method

        // Returns every breeding line currently stored in the database.
        //
        // Returns:
        // List<BreedingLine> = all breeding lines found
        //
        // Notes:
        // AsNoTracking() tells EF Core not to track these entities because
        // they are only being read, not modified. This improves performance
        // and uses less memory.
        public List<BreedingLine> GetAllBreedingLines()
        {
            try
            {
                // Return entities directly because the interface requires
                // List<BreedingLine>. Conversion to DTOs happens later
                // inside the controller.
                return context.BreedingLines.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                // Log the actual exception and allow it to continue
                // up the stack for higher-level handling.
                Console.WriteLine("GetAllBreedingLines --- " + ex.Message);
                throw;
            }
        }

        #endregion

        #region Get all Dinosaurs in a single BreedingLine by ID

        // Returns all dinosaurs that belong to a specific breeding line.
        //
        // Parameters:
        // breedingLineId = primary key of the breeding line to search
        //
        // Returns:
        // List<Dinosaur> = matching dinosaurs
        // null = no dinosaurs were found
        //
        // Related entities are loaded using Include() so the caller
        // receives complete dinosaur information without additional queries.
        public List<Dinosaur> GetDinosaursByBreedingLine(int breedingLineId)
        {
            // Initialize List<Dinosaur> object so that it is
            // accessible accross the method and we dont forget
            // to initialize it later.
            List<Dinosaur> dinosaurList = new List<Dinosaur>();

            try
            {
                // Build a query that loads:
                //
                // BreedingLine   -> breeding line details
                // Stats          -> base stat values
                // MutatedStats   -> mutation stat values
                // Father         -> father dinosaur information
                // Mother         -> mother dinosaur information
                //
                // Include() performs eager loading so these related
                // entities are retrieved in the same database operation.
                var dinosaurs = context.Dinosaurs
                    .Include(d => d.BreedingLine)
                    .Include(d => d.Stats)
                    .Include(d => d.MutatedStats)
                    .Include(d => d.Father)
                    .Include(d => d.Mother)
                    .Where(d => d.BreedingLineId == breedingLineId)
                    .ToList();

                // Verify that a result was returned before attempting
                // to process the collection.
                if (dinosaurs != null)
                {
                    foreach (var dinosaur in dinosaurs)
                    {
                        // Create a new Dinosaur object and copy the data.
                        //
                        // This prevents higher layers from working directly
                        // with EF Core's tracked entity instances.
                        Dinosaur currentDinosaur = new Dinosaur()
                        {
                            DinosaurId = dinosaur.DinosaurId,
                            BreedingLineId = dinosaur.BreedingLineId,
                            BreedingLine = dinosaur.BreedingLine,
                            DinosaurName = dinosaur.DinosaurName,
                            Species = dinosaur.Species,
                            Gender = dinosaur.Gender,
                            Generation = dinosaur.Generation,
                            StatsId = dinosaur.StatsId,
                            Stats = dinosaur.Stats,
                            MutatedStatsId = dinosaur.MutatedStatsId,
                            MutatedStats = dinosaur.MutatedStats,
                            Mutated = dinosaur.Mutated,
                            AquaticDino = dinosaur.AquaticDino,
                            FatherId = dinosaur.FatherId,
                            Father = dinosaur.Father,
                            MotherId = dinosaur.MotherId,
                            Mother = dinosaur.Mother
                        };

                        dinosaurList.Add(currentDinosaur);
                    }
                }

                // The DAL's convention is to return null when no dinosaurs
                // exist for the breeding line.
                if (dinosaurList.Count == 0)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Log the exception and allow higher layers to decide
                // how to handle the error.
                Console.WriteLine($"GetAllDinosaursByBreedingLineId -- {ex.Message}");
                throw;
            }

            return dinosaurList;
        }

        #endregion

        #region Get Stats for a single Dinosaur by StatsId

        // Retrieves a Stats record using its primary key.
        //
        // Parameters:
        // statsId = ID of the desired stats record
        //
        // Returns:
        // A populated Stats object.
        //
        // Throws:
        // InvalidOperationException if no matching record exists.
        public Stats GetStatsById(int statsId)
        {
            // Initialize stats object so that it is accessible
            // accross the method and we dont forget to initialize
            // it later.
            var stats = new Stats();

            try
            {
                // Search for the matching stats record.
                // FirstOrDefault() returns null if no record exists.
                var dinosaurStats = context.Stats
                    .Where(s => s.StatsId == statsId)
                    .FirstOrDefault();

                // Check if the object is null, if so, then throw an
                // InvalidOperationException
                if (dinosaurStats == null)
                {
                    throw new InvalidOperationException(
                        $"Stats record with {statsId} was not found");
                }

                // Copy the database entity into a new Stats object.
                stats = new Stats()
                {
                    StatsId = dinosaurStats.StatsId,
                    Health = dinosaurStats.Health,
                    Stamina = dinosaurStats.Stamina,
                    Oxygen = dinosaurStats.Oxygen,
                    Food = dinosaurStats.Food,
                    Water = dinosaurStats.Water,
                    Weight = dinosaurStats.Weight,
                    Melee = dinosaurStats.Melee,
                    MovementSpeed = dinosaurStats.MovementSpeed
                };
            }
            // If some unforseen error happens throw an exception with
            // a mesage to be handled by a higher layer.
            catch (Exception ex)
            {
                Console.WriteLine("GetStatsById --- " + ex.Message);
                throw;
            }

            // Return the stats object
            return stats;
        }

        #endregion

        #region Get MutatedStats for a single Dinosaur by Id

        // Retrieves a MutatedStats record using its primary key.
        //
        // Parameters:
        // mutatedStatsId = ID of the desired MutatedStat record
        //
        // Returns:
        // A populated MutatedStats object.
        //
        // Throws:
        // InvalidOperationException if no matching record exists.
        public MutatedStats GetMutatedStatsById(int mutatedStatsId)
        {
            // Initialize mutatedStats object so that it is accessible
            // accross the method and we dont forget to initialize
            // it later.
            var mutatedStats = new MutatedStats();

            try
            {
                // Search for the matching MutatedStats record.
                // FirstOrDefault() returns null if no record exists.
                var dinosaurMutatedStats = context.MutatedStats
                    .Where(s => s.MutatedStatsId == mutatedStatsId)
                    .FirstOrDefault();

                // Check if the object is null, if so, then throw an
                // InvalidOperationException
                if (dinosaurMutatedStats == null)
                {
                    throw new InvalidOperationException(
                        $"MutatedStats record with {mutatedStatsId} was not found");
                }

                // Copy the database entity into a new MutatedStats object.
                mutatedStats = new MutatedStats()
                {
                    MutatedStatsId = dinosaurMutatedStats.MutatedStatsId,
                    Health = dinosaurMutatedStats.Health,
                    Stamina = dinosaurMutatedStats.Stamina,
                    Oxygen = dinosaurMutatedStats.Oxygen,
                    Food = dinosaurMutatedStats.Food,
                    Water = dinosaurMutatedStats.Water,
                    Weight = dinosaurMutatedStats.Weight,
                    Melee = dinosaurMutatedStats.Melee,
                    MovementSpeed = dinosaurMutatedStats.MovementSpeed
                };
            }
            // If some unforseen error happens throw an exception with
            // a mesage to be handled by a higher layer.
            catch (Exception ex)
            {
                Console.WriteLine("GetMutatedStatsById --- " + ex.Message);
                throw;
            }

            // Return the mutatedStats object
            return mutatedStats;
        }

        #endregion

        #region Post new Dinosaur to an existing BreedingLine

        // Adds a new dinosaur (along with its Stats and MutatedStats)
        // into an existing breeding line.
        //
        // Process:
        // 1. Validate required data.
        // 2. Create Dinosaur, Stats, and MutatedStats entities.
        // 3. Add them to EF Core.
        // 4. Save everything to the database.
        // 5. Return the newly created IDs and dinosaur information.
        //
        // Notes:
        // EF Core automatically inserts Stats and MutatedStats because
        // they are attached to the new Dinosaur entity.
        public async Task<SaveDinosaursResponse> AddDinosaurToBreedingLine(
            DinosaurRegistrationModel dinosaur)
        {
            // Initialize response object so that it is accessible
            // accross the method and we dont forget to initialize
            // it later.
            SaveDinosaursResponse response = new();

            // Stats and MutatedStats are required because the Dinosaur
            // entity depends on them for its foreign key relationships.
            if (dinosaur.Stats == null || dinosaur.MutatedStats == null)
            {
                response.Status = false;
                response.StatusCode = 400;
                response.Message = "Stats and MutatedStats are required";
                return response;
            }

            try
            {
                // Build a new Dinosaur entity using values
                // from the registration model.
                var newDinosaur = new Dinosaur
                {
                    BreedingLineId = dinosaur.BreedingLineId,
                    DinosaurName = dinosaur.DinosaurName,
                    Species = dinosaur.Species,
                    Gender = dinosaur.Gender,
                    Generation = dinosaur.Generation,
                    Mutated = dinosaur.Mutated,
                    AquaticDino = dinosaur.AquaticDino,
                    FatherId = dinosaur.FatherId,
                    MotherId = dinosaur.MotherId,

                    // Create the dinosaur's base stats record.
                    Stats = new Stats
                    {
                        Health = dinosaur.Stats.Health,
                        Stamina = dinosaur.Stats.Stamina,
                        Oxygen = dinosaur.Stats.Oxygen,
                        Food = dinosaur.Stats.Food,
                        Water = dinosaur.Stats.Water,
                        Weight = dinosaur.Stats.Weight,
                        Melee = dinosaur.Stats.Melee,
                        MovementSpeed = dinosaur.Stats.MovementSpeed
                    },

                    // Create the dinosaur's mutation stats record.
                    MutatedStats = new MutatedStats
                    {
                        Health = dinosaur.MutatedStats.Health,
                        Stamina = dinosaur.MutatedStats.Stamina,
                        Oxygen = dinosaur.MutatedStats.Oxygen,
                        Food = dinosaur.MutatedStats.Food,
                        Water = dinosaur.MutatedStats.Water,
                        Weight = dinosaur.MutatedStats.Weight,
                        Melee = dinosaur.MutatedStats.Melee,
                        MovementSpeed = dinosaur.MutatedStats.MovementSpeed
                    }
                };

                // Mark the dinosaur and all attached child entities
                // for insertion into the database.
                context.Dinosaurs.Add(newDinosaur);

                // SaveChangesAsync performs the actual INSERT statements.
                //
                // Once this completes, EF Core updates the entity with
                // its database-generated primary keys.
                await context.SaveChangesAsync();

                // Set the values of the response object for a Success
                response.Status = true;
                response.StatusCode = 200;

                response.Message =
                    $"{newDinosaur.DinosaurName} was added successfully.";

                // Build the response object using the newly created entity.
                response.Data = new DinosaurData
                {
                    DinosaurId = newDinosaur.DinosaurId,
                    BreedingLineId = newDinosaur.BreedingLineId,
                    DinosaurName = newDinosaur.DinosaurName,
                    Species = newDinosaur.Species,
                    Gender = newDinosaur.Gender,
                    Generation = newDinosaur.Generation,
                    StatsId = newDinosaur.StatsId,
                    MutatedStatsId = newDinosaur.MutatedStatsId,
                    Mutated = newDinosaur.Mutated,
                    AquaticDino = newDinosaur.AquaticDino,
                    FatherId = newDinosaur.FatherId,
                    MotherId = newDinosaur.MotherId
                };

                // At this point SaveChangesAsync has completed,
                // so database-generated IDs should now exist.
                Console.WriteLine($"DinosaurId: {newDinosaur.DinosaurId}");
                Console.WriteLine($"StatsId: {newDinosaur.StatsId}");
                Console.WriteLine($"MutatedStatsId: {newDinosaur.MutatedStatsId}");
            }
            // If some unforseen error happens set correct values for the
            // response object.
            catch (Exception ex)
            {
                // Log the full exception details for debugging and return
                // a generic error message to the caller.
                Console.WriteLine($"AddDinosaurToBreedingLine -- {ex}");

                response.Status = false;
                response.StatusCode = 500;
                response.Message = $"An error occured while adding the dinosaur";
            }

            // Return the response object
            return response;
        }

        #endregion
    }
}