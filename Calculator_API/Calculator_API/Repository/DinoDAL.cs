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

    }
}
