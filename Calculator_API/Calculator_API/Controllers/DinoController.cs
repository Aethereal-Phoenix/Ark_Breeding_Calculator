using Microsoft.AspNetCore.Mvc;
using Calculator_API.Data;
using Calculator_API.IRepository;
using Calculator_API.Models;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Calculator_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class DinoController : ControllerBase
    {
        private readonly IDino repository;
        private readonly CalculatorContext context;
        private readonly IConfiguration config;

        public DinoController(IDino repo, IConfiguration config)
        {
            repository = repo;
            this.config = config;
        }


        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Controller reached.");
        }
    }
}
