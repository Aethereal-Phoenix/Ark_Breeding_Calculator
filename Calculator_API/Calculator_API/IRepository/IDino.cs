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
    }
}
