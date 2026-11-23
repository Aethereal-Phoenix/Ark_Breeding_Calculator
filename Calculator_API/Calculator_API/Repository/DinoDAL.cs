using Calculator_API.Data;
using Calculator_API.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Calculator_API.Repository
{
    public class DinoDAL : IDino
    {
        private readonly CalculatorContext context;

        public DinoDAL(CalculatorContext Context, IConfiguration configuration)
        {
            context = Context;
        }

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
                var dinosaurs = context.Dinosaurs.Where(d => d.BreedingLineId == breedingLineId);

                // Check that we didn't get a null
                if(dinosaurs != null)
                {
                    foreach(var dinosaur in dinosaurs)
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

                        dinosaurList.Add(dinosaur);
                    }
                }

                if(dinosaurList.Count == 0)
                {
                    return null;
                }
            }
            catch(Exception ex)
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
            catch(Exception ex)
            {
                Console.WriteLine("GetStatsById --- " + ex.Message);
                throw;
            }

            return stats;
        }
        #endregion

        #region Get MutatedStats for a single Dinosaru by MutatedStatsId
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
    }
}
