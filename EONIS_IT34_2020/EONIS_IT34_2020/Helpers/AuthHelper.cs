using EONIS_IT34_2020.Data.AdministratorRepository;
using EONIS_IT34_2020.Data.KorisnikRepository;
using EONIS_IT34_2020.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EONIS_IT34_2020.Helpers
{ 
    public class AuthHelper : IAuthHelper
    {
        private readonly IConfiguration configuration;
        private readonly IAdministratorRepository administratorRepository;
        private readonly IKorisnikRepository korisnikRepository;

        public AuthHelper(IConfiguration configuration, IAdministratorRepository administratorRepository, IKorisnikRepository korisnikRepository)
        {
            this.configuration = configuration;
            this.administratorRepository = administratorRepository;
            this.korisnikRepository = korisnikRepository;
        }

        public bool AuthenticateCreds(AuthCreds creds, bool isEmployee)
        {
            if(isEmployee && administratorRepository.AdministratorWithCredentialsExists(creds.korisnickoIme, creds.lozinka))
            {
                return true;
            } 
            else if(!isEmployee && korisnikRepository.KorisnikWithCredentialsExists(creds.korisnickoIme, creds.lozinka))
            {
                return true;
            }
            return false;
        }

        public string GenerateJwt(AuthCreds creds, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, creds.korisnickoIme),
                    new Claim(ClaimTypes.Role, role)
                };

            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                                             configuration["Jwt:Issuer"],
                                             claims,
                                             expires: DateTime.Now.AddMinutes(120),
                                             signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
