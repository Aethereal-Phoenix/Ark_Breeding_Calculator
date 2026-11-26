using Calculator_API.Models;
using Calculator_API.Data;

namespace Calculator_API.IRepository
{
    // ---------------------------------------------------------------------
    // IDino
    //
    // Purpose: Defines all dinosaur-related database operations that must
    // be implemented by the DinoDAL.
    //
    // Layering (cheat sheet):
    //
    // Controller -> calls methods from this interface
    // IDino -> defines what operations are available
    // DinoDAL -> provides the actual database implementation
    //
    // Using an interface allows dependency injection and makes unit
    // testing easier because a mock repository can be substituted for
    // the real database repository.
    // ---------------------------------------------------------------------
    public interface IDino
    {
        /// <summary>
        /// Retrieves all BreedingLines currently stored in the database.
        ///
        /// Returns:
        /// List&lt;BreedingLine&gt; containing every breeding line that
        /// exists in the system.
        ///
        /// This method is used by the controller when building the
        /// GetAllBreedingLines API response.
        /// </summary>
        /// <returns>
        /// A list of BreedingLine entities.
        /// </returns>
        public List<BreedingLine> GetAllBreedingLines();

        /// <summary>
        /// Retrieves all Dinosaurs that belong to a specific BreedingLine.
        ///
        /// The returned dinosaur objects may include related data such as:
        /// - BreedingLine information
        /// - Stats
        /// - MutatedStats
        /// - Father information
        /// - Mother information
        ///
        /// This method is used by the controller when building the
        /// GetAllDinosaursByBreedingLineId API response.
        /// </summary>
        /// <param name="breedingLineId">
        /// Primary key of the BreedingLine being searched.
        /// </param>
        /// <returns>
        /// A list of Dinosaur entities associated with the specified
        /// BreedingLine.
        /// </returns>
        public List<Dinosaur> GetDinosaursByBreedingLine(int breedingLineId);

        /// <summary>
        /// Retrieves the Stats record associated with a specific StatsId.
        ///
        /// Stats contain a dinosaur's base values such as:
        /// Health, Stamina, Oxygen, Food, Water, Weight,
        /// Melee Damage, and Movement Speed.
        /// </summary>
        /// <param name="statsId">
        /// Primary key of the Stats record to retrieve.
        /// </param>
        /// <returns>
        /// A populated Stats object containing the dinosaur's
        /// base stat values.
        /// </returns>
        public Stats GetStatsById(int statsId);

        /// <summary>
        /// Adds a new Dinosaur to an existing BreedingLine.
        ///
        /// The registration model contains:
        /// - Dinosaur information
        /// - Base Stats
        /// - Mutated Stats
        ///
        /// A SaveDinosaursResponse object is returned indicating
        /// whether the database operation was successful.
        /// </summary>
        /// <param name="dinosaur">
        /// Registration model containing all information needed
        /// to create the Dinosaur and its related records.
        /// </param>
        /// <returns>
        /// A SaveDinosaursResponse containing the operation result,
        /// status code, message, and newly created dinosaur data.
        /// </returns>
        public Task<SaveDinosaursResponse> AddDinosaurToBreedingLine(DinosaurRegistrationModel dinosaur);
    }
}