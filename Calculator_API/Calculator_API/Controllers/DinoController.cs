using Calculator_API.Data;
using Calculator_API.IRepository;
using Calculator_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Calculator_API.Controllers
{
    // ---------------------------------------------------------------------
    // DinoController
    //
    // Purpose: HTTP entry point for dinosaur-related requests.
    //
    // Layering (cheat sheet):
    //   Controller  -> receives the request, calls the repository (DAL),
    //                  and converts entities into response DTOs.
    //   DAL (IDino) -> talks to the database through EF Core.
    //
    // The controller never touches the database directly. It only knows
    // about the IDino interface, which is what makes it easy to test.
    // ---------------------------------------------------------------------
    [ApiController]          // Enables automatic model binding/validation behavior for APIs
    [Route("[controller]")]  // Base route is the class name minus "Controller" -> /Dino
    [AllowAnonymous]         // TODO: Everything is public right now. Fine for development,
                             // but add [Authorize] to endpoints that write data before deploying.
    public class DinoController : ControllerBase
    {
        #region Constructor

        // The repository (DAL) used for all data access. Typed as the interface
        // so tests can pass in a real DinoDAL on an in-memory DB, or a mock.
        private readonly IDino repository;

        // The DI container supplies the IDino implementation (DinoDAL) automatically.
        // NOTE: Tests now build the controller with: new DinoController(repo)
        public DinoController(IDino repo)
        {
            repository = repo;
        }

        #endregion

        #region Get all BreedingLines

        // GET /Dino/GetAllBreedingLines
        // Returns every breeding line as a list of BreedingLineData DTOs.
        //
        // Response codes (inside the body, the HTTP status is always 200 for now):
        //   200 = at least one breeding line found
        //   404 = none found
        //   500 = unexpected error
        //
        // No async/await here because the DAL call is synchronous.
        // Task.FromResult wraps the finished response in a Task so callers can still await it.
        [HttpGet("GetAllBreedingLines", Name = "GetAllBreedingLines")]
        [AllowAnonymous]
        public Task<GetBreedingLinesResponseModel> GetAllBreedingLines()
        {
            var response = new GetBreedingLinesResponseModel();

            try
            {
                // The DAL returns entities (List<BreedingLine>).
                // We convert them to DTOs below so the API never exposes raw entities.
                var breedingLines = repository.GetAllBreedingLines();

                // Nothing found -> 404 with an empty list (never null, so callers
                // can loop over the list without a null check).
                if (breedingLines == null || breedingLines.Count == 0)
                {
                    response.Status = false;
                    response.StatusCode = 404;
                    response.Message = "No breeding lines found";
                    response.BreedingLinesList = new List<BreedingLineData>();
                    return Task.FromResult(response);
                }

                // Found at least one -> success.
                response.Status = true;
                response.StatusCode = 200;
                response.Message = "Get Successful";

                // Map each BreedingLine entity to a BreedingLineData DTO.
                response.BreedingLinesList = breedingLines.Select(b => new BreedingLineData()
                {
                    BreedingLineId = b.BreedingLineId,
                    Species = b.Species,
                    BreedingLineName = b.BreedingLineName
                }).ToList();

                return Task.FromResult(response);
            }
            catch (Exception ex)
            {
                // Something unexpected went wrong. Log the real error for us,
                // but only send a generic message to the caller.
                Console.WriteLine("GetAllBreedingLines --- " + ex.Message);

                response.Status = false;
                response.Message = "Get Failed";
                response.StatusCode = 500;
                response.BreedingLinesList = new List<BreedingLineData>();
                return Task.FromResult(response);
            }
        }

        #endregion

        #region Get all Dinosaurs by BreedingLine ID

        // GET /Dino/GetAllDinosaursByBreedingLineId?breedingLineId=1
        // Returns every dinosaur in one breeding line, including its stats,
        // mutated stats, and parent names.
        //
        // Response codes (inside the body):
        //   200 = at least one dinosaur found
        //   404 = none found (the DAL returns null when the line has no dinosaurs)
        //   500 = unexpected error
        [HttpGet("GetAllDinosaursByBreedingLineId", Name = "GetAllDinosaursByBreedingLineId")]
        [AllowAnonymous]
        public Task<GetDinosaursResponseModel> GetAllDinosaursByBreedingLineId(int breedingLineId)
        {
            var response = new GetDinosaursResponseModel();

            try
            {
                // The DAL returns entities with Stats, MutatedStats, BreedingLine,
                // Father and Mother already loaded (via .Include) or null if none found.
                var dinos = repository.GetDinosaursByBreedingLine(breedingLineId);

                // Handles both null (what the DAL returns) and an empty list.
                if (dinos == null || dinos.Count == 0)
                {
                    response.Status = false;
                    response.StatusCode = 404;
                    response.Message = "No Dinosaurs found";
                    response.DinosaurList = new List<DinosaurData>();
                    return Task.FromResult(response);
                }

                response.Status = true;
                response.StatusCode = 200;
                response.Message = "Get Successful";

                // Map each Dinosaur entity to a DinosaurData DTO.
                // The "?." operators are null-safe: if a dinosaur has no BreedingLine,
                // Father, or Mother, the name comes back null instead of crashing.
                response.DinosaurList = dinos.Select(d => new DinosaurData
                {
                    DinosaurId = d.DinosaurId,
                    BreedingLineId = d.BreedingLineId,
                    BreedingLineName = d.BreedingLine?.BreedingLineName,
                    DinosaurName = d.DinosaurName,
                    Species = d.Species,
                    Gender = d.Gender,
                    Generation = d.Generation,
                    StatsId = d.StatsId,
                    Stats = d.Stats,
                    MutatedStatsId = d.MutatedStatsId,
                    MutatedStats = d.MutatedStats,
                    Mutated = d.Mutated,
                    AquaticDino = d.AquaticDino,
                    FatherId = d.FatherId,
                    FatherName = d.Father?.DinosaurName,
                    MotherId = d.MotherId,
                    MotherName = d.Mother?.DinosaurName
                }).ToList();

                return Task.FromResult(response);
            }
            catch (Exception ex)
            {
                // Log the real error, return a generic message to the caller.
                Console.WriteLine("GetAllDinosaursByBreedingLineId --- " + ex.Message);

                response.Status = false;
                response.Message = "Get Failed";
                response.StatusCode = 500;
                response.DinosaurList = new List<DinosaurData>();
                return Task.FromResult(response);
            }
        }

        #endregion

        #region Create Dinosaur

        // POST /Dino/AddDinosaurToBreedingLine
        // Adds a new dinosaur (with its Stats and MutatedStats) to an existing breeding line.
        // The request body is a DinosaurRegistrationModel.
        //
        // Response codes (inside the body):
        //   200 = dinosaur added
        //   400 = missing/invalid data (null model, or the DAL reported missing Stats/MutatedStats)
        //   500 = unexpected error
        //
        // TODO: Add [Authorize] here before deploying. This endpoint writes data.
        // This method IS async because it awaits the DAL's SaveChangesAsync.
        [HttpPost("AddDinosaurToBreedingLine", Name = "AddDinosaurToBreedingLine")]
        [AllowAnonymous]
        public async Task<CreateDinosaurResponseModel> AddDinosaurToBreedingLine(DinosaurRegistrationModel dinosaurModel)
        {
            var response = new CreateDinosaurResponseModel();

            // Guard clause: reject an empty request up front with a 400
            // (Bad Request) so the rest of the method can assume the model exists.
            if (dinosaurModel == null)
            {
                response.Status = false;
                response.StatusCode = 400;
                response.Message = "Dinosaur data is required";
                return response;
            }

            try
            {
                // Ask the DAL to save the dinosaur. The DAL does its own validation
                // (Stats/MutatedStats required) and reports the outcome via StatusCode.
                var dino = await repository.AddDinosaurToBreedingLine(dinosaurModel).ConfigureAwait(true);

                if (dino.StatusCode == 200)
                {
                    // The dinosaur was saved. Build the success response.
                    response.Status = true;
                    response.Message = "Dinosaur added";
                    response.StatusCode = 200;

                    // Send back the dinosaur we just added (including its new database IDs).
                    // NOTE: The DAL doesn't currently fill in BreedingLineName, Stats,
                    // FatherName or MotherName, so those come back null for now.
                    response.Data = new DinosaurData();

                    response.Data.DinosaurId = dino.Data.DinosaurId;
                    response.Data.BreedingLineId = dino.Data.BreedingLineId;
                    response.Data.BreedingLineName = dino.Data.BreedingLineName;
                    response.Data.DinosaurName = dino.Data.DinosaurName;
                    response.Data.Species = dino.Data.Species;
                    response.Data.Gender = dino.Data.Gender;
                    response.Data.Generation = dino.Data.Generation;
                    response.Data.StatsId = dino.Data.StatsId;
                    response.Data.Stats = dino.Data.Stats;
                    response.Data.MutatedStatsId = dino.Data.MutatedStatsId;
                    response.Data.MutatedStats = dino.Data.MutatedStats;
                    response.Data.Mutated = dino.Data.Mutated;
                    response.Data.AquaticDino = dino.Data.AquaticDino;
                    response.Data.FatherId = dino.Data.FatherId;
                    response.Data.FatherName = dino.Data.FatherName;
                    response.Data.MotherId = dino.Data.MotherId;
                    response.Data.MotherName = dino.Data.MotherName;
                }
                else
                {
                    // The DAL reported a failure. Pass its status code and message
                    // straight through so a 400 stays a 400 instead of becoming a generic 500.
                    response.Status = false;
                    response.StatusCode = dino.StatusCode;
                    response.Message = dino.Message;
                }
            }
            catch (Exception ex)
            {
                // Unexpected error. Log the full exception (type + stack trace) for us,
                // and return only a generic message to the caller.
                Console.WriteLine($"AddDinosaurToBreedingLine -- {ex}");

                response.Status = false;
                response.Message = "Registration Failed";
                response.StatusCode = 500;
            }

            return response;
        }

        #endregion

        // TODO (later): Right now every response is HTTP 200 and the real status lives in
        // response.StatusCode. To return true HTTP codes, change the return types to
        // ActionResult<T> and use Ok(...), NotFound(...), BadRequest(...).
        // Tests would then assert on the ActionResult instead of the response body.
    }
}