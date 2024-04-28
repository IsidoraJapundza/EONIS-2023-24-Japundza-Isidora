using EONIS_IT34_2020.Data.AdministratorRepository;
using EONIS_IT34_2020.Data.KorisnikRepository;
using EONIS_IT34_2020.Helpers;
using EONIS_IT34_2020.Models;
using Microsoft.AspNetCore.Mvc;

namespace EONIS_IT34_2020.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class AuthController : Controller
    {
        private readonly IAdministratorRepository administratorRepository;
        private readonly IKorisnikRepository korisnikRepository;
        private readonly IAuthHelper authHelper;
        
        public AuthController(IAdministratorRepository administratorRepository, IKorisnikRepository korisnikRepository, IAuthHelper authHelper)
        {
            this.administratorRepository = administratorRepository;
            this.korisnikRepository = korisnikRepository;
            this.authHelper = authHelper;
        }

        [HttpPost("/administrator")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult AuthenticateAdministrator(AuthCreds creds)
        {
            if (authHelper.AuthenticateCreds(creds, true))
            {
                var tokenString = authHelper.GenerateJwt(creds);
                return Ok(new { token = tokenString});
            }

            return Unauthorized();
        }

        [HttpPost("/korisnik")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult AuthenticateKorisnik(AuthCreds creds)
        {
            if (authHelper.AuthenticateCreds(creds, true))
            {
                var tokenString = authHelper.GenerateJwt(creds);
                return Ok(new { token = tokenString });
            }

            return Unauthorized();
        }
    }
}
