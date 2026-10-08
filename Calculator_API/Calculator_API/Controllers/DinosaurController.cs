using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Calculator_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class DinosaurController : ControllerBase
    {
        private readonly IUser repository;
    }
}
