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
    public class DinosaurController : ControllerBase
    {
        private readonly IDino repository;
    }
}
