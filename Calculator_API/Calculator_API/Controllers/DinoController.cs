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
    }
}
