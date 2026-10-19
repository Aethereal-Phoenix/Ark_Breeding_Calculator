using Calculator_API.Data;
using Calculator_API.IRepository;
using Calculator_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace Calculator_API.Controllers
{


    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class DinoController : ControllerBase
    {
        private readonly IDino repository;
        private readonly CalculatorContext _context;
        private readonly IConfiguration config;

        public DinoController(IDino repo, IConfiguration config, CalculatorContext context)
        {
            repository = repo;
            this.config = config;
            _context = context;
        }

        [HttpGet("GetAllBreedingLines", Name = "GetAllBreedingLines")]
        [AllowAnonymous]
        public async Task<GetBreedingLinesResponseModel> GetAllBreedingLines()
        {
            var response = new GetBreedingLinesResponseModel();


            try
            {
                // DAL returns List<User> (entity). Convert to DTO list.
                var breedingLines = repository.GetAllBreedingLines();

                if (breedingLines == null || breedingLines.Count == 0)
                {
                    response.Status = false;
                    response.StatusCode = 404;
                    response.Message = "No users found";
                    response.BreedingLinesList = new List<BreedingLineData>();
                    return response;
                }

                response.Status = true;
                response.StatusCode = 200;
                response.Message = "Get Successful";
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
                Console.WriteLine("GetAllUsers --- " + ex.Message);
                response.Status = false;
                response.Message = "Get Failed";
                response.StatusCode = 500;
                response.BreedingLinesList = new List<BreedingLineData>();
                return response;
            }
        }

        [HttpGet("GetAllDinosaursByBreedingLineId", Name = "GetAllDinosaursByBreedingLineId")]
        [AllowAnonymous]
        public async Task<GetDinosaursResponseModel> GetAllDinosaursByBreedingLineId(int breedingLineId)
        {
            var response = new GetDinosaursResponseModel();

            try
            {
                // DAL returns List<User> (entity). Convert to DTO list.
                var users = repository.GetDinosaursByBreedingLine(breedingLineId);

                if (users == null || users.Count == 0)
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
                response.DinosaurList = users.Select(d => new DinosaurData
                {
                    DinosaurId = d.DinosaurId,
                    BreedingLineId = d.BreedingLineId,
                    BreedingLine = d.BreedingLine,
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
                    Father = d.Father,
                    MotherId = d.MotherId,
                    Mother = d.Mother
                }).ToList();

                return response;
            }
            catch(Exception ex)
            {
                Console.WriteLine("GetAllDinosaursByBreedingLineId --- " + ex.Message);
                response.Status = false;
                response.Message = "Get Failed";
                response.StatusCode = 500;
                response.DinosaurList = new List<DinosaurData>();
                return response;
            }
        }
    }
}
