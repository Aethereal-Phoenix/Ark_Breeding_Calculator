using Calculator_API.Data;
using Calculator_API.IRepository;

namespace Calculator_API.Repository
{
    public class DinoDAL : IDino
    {
        private readonly CalculatorContext context;

        public DinoDAL(CalculatorContext Context, IConfiguration configuration)
        {
            context = Context;
        }

    }
}
