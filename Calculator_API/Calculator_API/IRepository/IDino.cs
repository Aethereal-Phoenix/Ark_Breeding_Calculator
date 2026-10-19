using Calculator_API.Models;
using Calculator_API.Data;

namespace Calculator_API.IRepository
{
    public interface IDino
    {
        /// <summary>
        /// Returns a list of all BreedingLines
        /// </summary>
        /// <returns></returns>
        public List<BreedingLine> GetAllBreedingLines();

        /// <summary>
        /// Returns a list of all Dinosaurs in a BreedingLine
        /// </summary>
        /// <param name="breedingLineId"></param>
        /// <returns></returns>
        public List<Dinosaur> GetDinosaursByBreedingLine(int breedingLineId);

        /// <summary>
        /// Returns a Stats object associated with a specific Dinosaur
        /// </summary>
        /// <param name="dinosaurId"></param>
        /// <returns></returns>
        public Stats GetStatsById(int statsId);
    }
}
