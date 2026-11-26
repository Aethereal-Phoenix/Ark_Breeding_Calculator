using Calculator_API.Data;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ArkCalculatorTests.Helpers
{
    public static class TestDataSeeder
    {
        public static void BasicData(CalculatorContext context)
        {
            #region context data setup

            #region BreedingLines
            context.BreedingLines.Add(new BreedingLine
            {
                BreedingLineId = 1,
                Species = SpeciesEnum.Shadowmane,
                BreedingLineName = "Test Shadowmane Line Test"
            });

            context.BreedingLines.Add(new BreedingLine
            {
                BreedingLineId = 2,
                Species = SpeciesEnum.Rex,
                BreedingLineName = "Test Rex Line Test"
            });

            context.SaveChanges();
            #endregion

            #region Shadowmane
            context.Dinosaurs.Add(new Dinosaur
            {
                DinosaurId = 1,
                BreedingLineId = 1,
                DinosaurName = "Shadowmane test 1",
                Species = SpeciesEnum.Shadowmane,
                Gender = GenderEnum.Male,
                Generation = GenerationsEnum.Base,
                StatsId = 1,
                Stats = CreateStatsAquatic1(1),
                MutatedStatsId = 1,
                MutatedStats = CreateMutatedStatsAquatic0(1),
                Mutated = false,
                AquaticDino = true,
                FatherId = null,
                Father = null,
                MotherId = null,
                Mother = null
            });

            context.Dinosaurs.Add(new Dinosaur
            {
                DinosaurId = 2,
                BreedingLineId = 1,
                DinosaurName = "Shadowmane test 2",
                Species = SpeciesEnum.Shadowmane,
                Gender = GenderEnum.Female,
                Generation = GenerationsEnum.Base,
                StatsId = 2,
                Stats = CreateStatsAquatic2(2),
                MutatedStatsId = 2,
                MutatedStats = CreateMutatedStatsAquatic0(2),
                Mutated = false,
                AquaticDino = true,
                FatherId = null,
                Father = null,
                MotherId = null,
                Mother = null
            });

            context.SaveChanges();
            #endregion


            #endregion
        }

        #region Stats methods
        #region Create Stats methods for NonAquatic dinos
        public static Stats CreateStatsNonAquatic1(int statsId)
        {
            return new Stats
            {
                StatsId = statsId,
                Health = 100,
                Stamina = 200,
                Oxygen = 300,
                Food = 400,
                Water = 500,
                Weight = 600,
                Melee = 700,
                MovementSpeed = 800
            };
        }

        public static Stats CreateStatsNonAquatic2(int statsId)
        {
            return new Stats
            {
                StatsId = statsId,
                Health = 800,
                Stamina = 700,
                Oxygen = 600,
                Food = 500,
                Water = 400,
                Weight = 300,
                Melee = 200,
                MovementSpeed = 100
            };
        }
        #endregion

        #region Create Stats for Aquatic dinos
        public static Stats CreateStatsAquatic1(int statsId)
        {
            return new Stats
            {
                StatsId = statsId,
                Health = 100,
                Stamina = 200,
                Oxygen = null,
                Food = 400,
                Water = null,
                Weight = 600,
                Melee = 700,
                MovementSpeed = 800
            };
        }

        public static Stats CreateStatsAquatic2(int statsId)
        {
            return new Stats
            {
                StatsId = statsId,
                Health = 800,
                Stamina = 700,
                Oxygen = null,
                Food = 500,
                Water =  null,
                Weight = 300,
                Melee = 200,
                MovementSpeed = 100
            };
        }
        #endregion
        #endregion

        #region MutatedStats methods
        #region Create MutatedStats methods for NonAquatic Dinos
        public static MutatedStats CreateMutatedStatsNonAquatic0(int mutatedStatsId) 
        {
            return new MutatedStats
            {
                MutatedStatsId = mutatedStatsId,
                Health = 0,
                Stamina = 0,
                Oxygen = 0,
                Food = 0,
                Water = 0,
                Weight = 0,
                Melee = 0,
                MovementSpeed = 0
            };
        }
        #endregion

        #region Create MutatedStats methods for AquaticDinos
        public static MutatedStats CreateMutatedStatsAquatic0(int mutatedStatsId)
        {
            return new MutatedStats
            {
                MutatedStatsId = mutatedStatsId,
                Health = 0,
                Stamina = 0,
                Oxygen = null,
                Food = 0,
                Water = null,
                Weight = 0,
                Melee = 0,
                MovementSpeed = 0
            };
        }
        #endregion
        #endregion
    }
}
