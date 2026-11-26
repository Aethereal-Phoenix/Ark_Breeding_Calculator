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
    // Every endpoint returns a response model that contains:
    //   Status     = true/false (did it work?)
    //   StatusCode = 200 / 400 / 404 / 500 (what happened, stored in the body)
    //   Message    = human-readable text
    //
    // NOTE: The real HTTP status of every response is still 200 OK.
    // The StatusCode above only lives inside the response body.
    // ---------------------------------------------------------------------
    [ApiController]          // Turns on API-specific behavior (automatic model binding, etc.)
    [Route("[controller]")]  // Base route = class name minus "Controller" -> /Dino
    [AllowAnonymous]         // Anyone can call these endpoints without logging in
    public class DinoController : ControllerBase
    {
        #region Constructor

        // The repository (DAL) used for all database work.
        // Typed as the IDino interface so it can be swapped out in tests.
        private readonly IDino repository;

        // Database context and configuration. Currently NOT used by any method in this
        // controller (the repository does the database work), but they stay in the
        // constructor so existing tests and dependency injection keep working.
        private readonly CalculatorContext _context;
        private readonly IConfiguration config;

        // ASP.NET's dependency injection supplies these automatically at runtime.
        public DinoController(IDino repo, IConfiguration config, CalculatorContext context)
        {
            repository = repo;
            this.config = config;
            _context = context;
        }

        #endregion

        #region Get all BreedingLines method

        // GET /Dino/GetAllBreedingLines
        // Returns every breeding line as a list of BreedingLineData objects.
        //
        // StatusCodes returned in the body:
        //   200 = at least one breeding line found
        //   404 = no breeding lines found
        //   500 = something unexpected went wrong
        [HttpGet("GetAllBreedingLines", Name = "GetAllBreedingLines")]
        [AllowAnonymous]
        public async Task<GetBreedingLinesResponseModel> GetAllBreedingLines()
        {
            var response = new GetBreedingLinesResponseModel();

            try
            {
                // The DAL returns entities (List<BreedingLine>).
                // They get converted to DTOs below before being sent back.
                var breedingLines = repository.GetAllBreedingLines();

                // Nothing found -> 404. The list is set to an empty list (not null)
                // so the caller can safely loop over it.
                if (breedingLines == null || breedingLines.Count == 0)
                {
                    response.Status = false;
                    response.StatusCode = 404;
                    response.Message = "No breeding lines found";
                    response.BreedingLinesList = new List<BreedingLineData>();
                    return response;
                }

                // At least one breeding line found -> success.
                response.Status = true;
                response.StatusCode = 200;
                response.Message = "Get Successful";

                // Copy each BreedingLine entity into a BreedingLineData DTO.
                // Select() transforms each item, ToList() turns the result back into a list.
                response.BreedingLinesList = breedingLines.Select(b => new BreedingLineData()
                {
                    BreedingLineId = b.BreedingLineId,
                    Species = b.Species,
                    BreedingLineName = b.BreedingLineName
                }).ToList();

                return response;
            }
            catch (Exception ex)
            {
                // Unexpected error. Log the real reason for us, but only send
                // a generic message to the caller.
                Console.WriteLine("GetAllBreedingLines --- " + ex.Message);

                response.Status = false;
                response.Message = "Get Failed";
                response.StatusCode = 500;
                response.BreedingLinesList = new List<BreedingLineData>();
                return response;
            }
        }

        #endregion

        #region Get all Dionsaurs by BreedingLine ID

        // GET /Dino/GetAllDinosaursByBreedingLineId?breedingLineId=1
        // Returns every dinosaur in one breeding line, including stats,
        // mutated stats, and the names of the breeding line and parents.
        //
        // StatusCodes returned in the body:
        //   200 = at least one dinosaur found
        //   404 = none found (the DAL returns null when the line has no dinosaurs)
        //   500 = something unexpected went wrong
        [HttpGet("GetAllDinosaursByBreedingLineId", Name = "GetAllDinosaursByBreedingLineId")]
        [AllowAnonymous]
        public async Task<GetDinosaursResponseModel> GetAllDinosaursByBreedingLineId(int breedingLineId)
        {
            var response = new GetDinosaursResponseModel();

            try
            {
                // The DAL returns Dinosaur entities with Stats, MutatedStats,
                // BreedingLine, Father and Mother already loaded (or null if none found).
                var dinos = repository.GetDinosaursByBreedingLine(breedingLineId);

                // Handles both null (what the DAL returns) and an empty list.
                if (dinos == null || dinos.Count == 0)
                {
                    response.Status = false;
                    response.StatusCode = 404;
                    response.Message = "No Dinosaurs found";
                    response.DinosaurList = new List<DinosaurData>();
                    return response;
                }

                response.Status = true;
                response.StatusCode = 200;
                response.Message = "Get Successful";

                // Copy each Dinosaur entity into a DinosaurData DTO.
                // The "?." operator is null-safe: if a dinosaur has no BreedingLine,
                // Father or Mother, the name comes back as null instead of crashing.
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

                return response;
            }
            catch (Exception ex)
            {
                // Unexpected error. Log the real reason, return a generic message.
                Console.WriteLine("GetAllDinosaursByBreedingLineId --- " + ex.Message);

                response.Status = false;
                response.Message = "Get Failed";
                response.StatusCode = 500;
                response.DinosaurList = new List<DinosaurData>();
                return response;
            }
        }

        #endregion

        #region Create Dinosaur

        // POST /Dino/AddDinosaurToBreedingLine
        // Adds a new dinosaur (with its Stats and MutatedStats) to an existing
        // breeding line. The request body is a DinosaurRegistrationModel.
        //
        // StatusCodes returned in the body:
        //   200 = dinosaur added
        //   400 = bad request (no data sent, or the DAL reported missing Stats/MutatedStats)
        //   500 = something unexpected went wrong
        //
        // This method is async because it awaits the DAL's SaveChangesAsync.
        [HttpPost("AddDinosaurToBreedingLine", Name = "AddDinosaurToBreedingLine")]
        [AllowAnonymous]
        public async Task<CreateDinosaurResponseModel> AddDinosaurToBreedingLine(DinosaurRegistrationModel dinosaurModel)
        {
            CreateDinosaurResponseModel response = new CreateDinosaurResponseModel();

            // Guard clause: if no data was sent, stop immediately with a 400 (Bad Request)
            // so the rest of the method can assume the model exists.
            if (dinosaurModel == null)
            {
                response.Status = false;
                response.StatusCode = 400;
                response.Message = "Dinosaur data is required";
                return response;
            }

            try
            {
                // Ask the DAL to save the dinosaur. The DAL validates the input
                // (Stats and MutatedStats are required) and reports the result
                // through its StatusCode.
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
                    // The DAL reported a failure. Pass its status code and message through
                    // so a 400 stays a 400 instead of turning into a generic 500.
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
    }
}