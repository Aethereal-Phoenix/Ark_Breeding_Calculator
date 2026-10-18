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


        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Controller reached.");
        }

        [HttpGet("dbtest")]
        public async Task<IActionResult> DbTest()
        {
            bool canConnect = await _context.Database.CanConnectAsync();

            return Ok(canConnect);
        }

        [HttpGet("dbpath")]
        public IActionResult DbPath()
        {
            var dbPath = Path.Combine(
                AppContext.BaseDirectory,
                "Data",
                "dinosaur_database.db");

            return Ok(dbPath);
        }

        [HttpGet("cwd")]
        public IActionResult CurrentDirectory()
        {
            return Ok(Directory.GetCurrentDirectory());
        }

        [HttpGet("firstdino")]
        public async Task<IActionResult> FirstDino()
        {
            var dino = await _context.Dinosaurs.FirstOrDefaultAsync();

            if (dino == null)
            {
                return NotFound("No dinosaurs found.");
            }

            return Ok(dino);
        }

        [HttpGet("teststats")]
        public async Task<IActionResult> TestStats()
        {
            var dino = await _context.Dinosaurs
                .Include(d => d.Stats)
                .FirstOrDefaultAsync();

            return Ok(new
            {
                dino?.DinosaurName,
                dino?.Stats.Health,
                dino?.Stats.Stamina
            });
        }
    }
}
