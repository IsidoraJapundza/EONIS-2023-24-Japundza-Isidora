using AutoMapper;
using EONIS_IT34_2020.Data.DogadjajRepository;
using EONIS_IT34_2020.Data.KorisnikRepository;
using EONIS_IT34_2020.Models.DTOs.Dogadjaj;
using EONIS_IT34_2020.Models.DTOs.Korisnik;
using EONIS_IT34_2020.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EONIS_IT34_2020.Controllers
{
    [ApiController]
    [Route("api/korisnik")]
    public class KorisnikController : Controller
    {
        private readonly IKorisnikRepository korisnikRepository;
        private readonly IMapper mapper;

        public KorisnikController(IMapper mapper, IKorisnikRepository korisnikRepository)
        {
            this.mapper = mapper;
            this.korisnikRepository = korisnikRepository;
        }

        [HttpGet]
        //[Authorize(Roles = "Administrator, Korisnik")] //izmenitii
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<List<KorisnikDto>> GetKorisnik()
        {
            /*
              if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized("Da biste izvršili operaciju, morate kreirati nalog!");
            }
            var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && (c.Value == "Administrator" || c.Value == "Korisnik"));

            if (roleClaim == null)
            {
                return Forbid();
            }
            */
            var korisnici = korisnikRepository.GetKorisnik();

            if (korisnici == null || korisnici.Count == 0)
            {
                NoContent();
            }

            List<KorisnikDto> korisniciDto = new List<KorisnikDto>();
            foreach (var korisnik in korisnici)
            {
                korisniciDto.Add(mapper.Map<KorisnikDto>(korisnik));
            }
            return Ok(korisniciDto);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{Id_korisnik}")]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[Authorize(Roles = "Administrator, Korisnik")]
        public ActionResult<KorisnikDto> GetKorisnikById(Guid Id_korisnik)
        {
            /*
             if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized("Da biste izvršili operaciju, morate kreirati nalog!");
            }
            var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && (c.Value == "Administrator" || c.Value == "Korisnik"));

            if (roleClaim == null)
            {
                return Forbid();
            }
             */
            var korisnik = korisnikRepository.GetKorisnikById(Id_korisnik);
            
            if (korisnik == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<KorisnikDto>(korisnik));
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        [HttpGet("{KorisnickoImeKorisnika}")]
        public ActionResult<KorisnikDto> GetKorisnikByKorisnickoIme(string korisnickoIme)
        {

            var korisnik = korisnikRepository.GetKorisnikByKorisnickoIme(korisnickoIme);

            if (korisnik == null)
            {
                return NotFound();
            }

            return mapper.Map<KorisnikDto>(korisnik);
        }

        [HttpPost]
        [AllowAnonymous]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<KorisnikDto> CreateKorisnik([FromBody] KorisnikCreationDto korisnikCreationDto) 
        {
            if (korisnikCreationDto == null)
            {
                return BadRequest("Invalid data provided!");
            }

            try
            {
                Korisnik createdKorisnik = korisnikRepository.CreateKorisnik(korisnikCreationDto);
                return mapper.Map<KorisnikDto>(createdKorisnik);

                /*Korisnik korisnikEntity = mapper.Map<Korisnik>(korisnikCreationDto);
                korisnikEntity.Id_korisnik = Guid.NewGuid();
                Korisnik confirmation = korisnikRepository.CreateKorisnik(korisnikEntity);
                return Ok(mapper.Map<KorisnikDto>(confirmation));*/
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }

        [HttpPut]
        //[Authorize(Roles = "Administrator, Korisnik")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<KorisnikDto> UpdateKorisnik(KorisnikUpdateDto korisnikUpdateDto)  
        {
            try
            {
                /*
                  if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return Unauthorized("Da biste izvršili operaciju, morate kreirati nalog!");
                }
                var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && (c.Value == "Administrator" || c.Value == "Korisnik"));

                if (roleClaim == null)
                {
                    return Forbid();
                } 
                 */

                var updatedKorisnik = korisnikRepository.UpdateKorisnik(korisnikUpdateDto);

                KorisnikDto updatedKorisnikDto = mapper.Map<KorisnikDto>(updatedKorisnik);

                return Ok(updatedKorisnikDto);

                /*
                 Korisnik UpdateKorisnik(KorisnikUpdateDto korisnik);

                var updatedKorisnik = korisnikRepository.UpdateKorisnik(korisnik);
                KorisnikDto updatedKorisnikDto = mapper.Map<KorisnikDto>(updatedKorisnik);
                return Ok(updatedKorisnikDto);
                 */
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error.");
            }
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{Id_korisnik}")]
        //[Authorize(Roles = "Administrator")]
        public IActionResult DeleteKorisnik(Guid Id_korisnik)
        {
            try
            {
                /*if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return Unauthorized("Da biste izvršili operaciju, morate kreirati nalog!");
                }
                var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && (c.Value == "Administrator"));

                if (roleClaim == null)
                {
                    return Forbid();
                }*/

                var korisnik = korisnikRepository.GetKorisnikById(Id_korisnik);
                if (korisnik == null)
                {
                    return NotFound();
                }

                korisnikRepository.DeleteKorisnik(Id_korisnik);
                korisnikRepository.SaveChanges();
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }      
    }
}
