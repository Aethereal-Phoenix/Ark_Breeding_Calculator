using Calculator_API.Models;
using Calculator_API.Data;

namespace Calculator_API.IRepository
{
    public interface IDino
    {
        List<Dinosaur> GetAllDinosaurs();

        Task<Dinosaur> GetDinosaurById();

        Task<SaveDinosaurResponse> SaveNewDinosaur(); // TODO! Fill in the response Model

        Task<SaveDinosaurResponse> UpdateDinosaur(); // TODO! Fill in the response Model

        List<BreedingLine> GetAllBreedingLines();
        Task<BreedingLine> GetBreedingLineById();
    }
}
