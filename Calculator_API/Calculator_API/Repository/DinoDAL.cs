using Calculator_API.Data;
using Calculator_API.IRepository;
using Microsoft.EntityFrameworkCore;
using Calculator_API.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Calculator_API.Repository
{
    public class DinoDAL : IDino
    {
        #region Constructor
        //context for the database connection
        private readonly CalculatorContext context;

        private readonly IConfiguration _config;

        public DinoDAL(CalculatorContext Context, IConfiguration config)
        {
            context = Context;
            _config = config;
        }
        #endregion

        #region Get All BreedingLines Method
        public List<BreedingLine> GetAllBreedingLines()
        {
            try
            {
                // Return entities here only because the interface requires List<User>.
                // Controller should map to UserData DTOs.
                return context.BreedingLines.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAllBreedingLines --- " + ex.Message);
                throw;
            }
        }
        #endregion

        #region Get all Dinosaurs in a single BreedingLine by ID
        public List<Dinosaur> GetDinosaursByBreedingLine(int breedingLineId)
        {
            List<Dinosaur> dinosaurList = new List<Dinosaur>();

            try
            {
                // Query the database to get all of the dinos in dinosaur table that have this BreedingLineId
                var dinosaurs = context.Dinosaurs
                    .Include(d => d.BreedingLine)
                    .Include(d => d.Stats)
                    .Include(d => d.MutatedStats)
                    .Include(d => d.Father)
                    .Include(d => d.Mother)
                    .Where(d => d.BreedingLineId == breedingLineId)
                    .ToList();

                // Check that we didn't get a null
                if (dinosaurs != null)
                {
                    foreach (var dinosaur in dinosaurs)
                    {
                        dinosaur.Stats = GetStatsById(dinosaur.StatsId);
                        dinosaur.MutatedStats = GetMutatedStatsById(dinosaur.MutatedStatsId);

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

                if (dinosaurList.Count == 0)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetAllDinosaursByBreedingLineId -- {ex.Message}");
                throw;
            }

            return dinosaurList;
        }
        #endregion

        #region Get Stats for a single Dinosaur by StatsId
        public Stats GetStatsById(int statsId)
        {
            var stats = new Stats();

            try
            {
                var dinosaurStats = context.Stats.Where(s => s.StatsId == statsId).FirstOrDefault();

                if (dinosaurStats == null)
                {
                    throw new InvalidOperationException($"Stats record with {statsId} was not found");
                }

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
            catch (Exception ex)
            {
                Console.WriteLine("GetStatsById --- " + ex.Message);
                throw;
            }

            return stats;
        }
        #endregion

        #region Get MutatedStats for a single Dinosaru by Id
        public MutatedStats GetMutatedStatsById(int mutatedStatsId)
        {
            var mutatedStats = new MutatedStats();

            try
            {
                var dinosaurMutatedStats = context.MutatedStats.Where(s => s.MutatedStatsId == mutatedStatsId).FirstOrDefault();

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
            catch (Exception ex)
            {
                Console.WriteLine("GetMutatedStatsById --- " + ex.Message);
                throw;
            }

            return mutatedStats;
        }
        #endregion

        #region Post new Dinosaur to an existing BreedingLine
        public async Task<SaveDinosaursResponse> AddDinosaurToBreedingLine(
    DinosaurRegistrationModel dinosaur)
        {
            SaveDinosaursResponse response = new();

            try
            {
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

                using (var db = new CalculatorContext(_config))
                {
                    db.Dinosaurs.Add(newDinosaur);

                    await db.SaveChangesAsync();
                }

                response.Status = true;
                response.StatusCode = 200;
                response.Message =
                    $"{newDinosaur.DinosaurName} was added successfully.";

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

                // At this point these IDs should exist
                Console.WriteLine($"DinosaurId: {newDinosaur.DinosaurId}");
                Console.WriteLine($"StatsId: {newDinosaur.StatsId}");
                Console.WriteLine($"MutatedStatsId: {newDinosaur.MutatedStatsId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AddDinosaurToBreedingLine -- {ex}");

                response.Status = false;
                response.StatusCode = 0;
                response.Message = ex.Message;
            }

            return response;
        }
        #endregion
    }
} 
