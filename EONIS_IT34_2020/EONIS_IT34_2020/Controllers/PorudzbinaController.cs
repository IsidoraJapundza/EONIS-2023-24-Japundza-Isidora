using AutoMapper;
using EONIS_IT34_2020.Data.KontingentKarataRepository;
using EONIS_IT34_2020.Data.PorudzbinaRepository;
using EONIS_IT34_2020.Models.DTOs.KontingentKarata;
using EONIS_IT34_2020.Models.DTOs.Porudzbina;
using EONIS_IT34_2020.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EONIS_IT34_2020.Controllers
{
    [ApiController]
    [Route("api/porudzbina")]
    public class PorudzbinaController : Controller
    {
        private readonly IPorudzbinaRepository porudzbinaRepository;
        private readonly IMapper mapper;

        public PorudzbinaController(IMapper mapper, IPorudzbinaRepository porudzbinaRepository)
        {
            this.mapper = mapper;
            this.porudzbinaRepository = porudzbinaRepository;
        }

        [HttpGet]
        //[Authorize(Roles = "Administrator, Korisnik")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<List<PorudzbinaDto>> GetPorudzbina(int page = 1, int pageSize = 10, string? PotvrdaPlacanja = null, bool sortByUkupnaCena = false, string sortOrder = "asc")
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
            var porudzbine = porudzbinaRepository.GetPorudzbina();

            if(PotvrdaPlacanja != null)
            {
                porudzbine = porudzbine.Where(p => p.PotvrdaPlacanja.Equals(PotvrdaPlacanja)).ToList();
            }

            if (sortByUkupnaCena)
            {
                porudzbine = sortOrder.ToLower() == "asc" ? porudzbine.OrderBy(a => a.UkupnaCena).ToList() : porudzbine.OrderByDescending(a => a.UkupnaCena).ToList();
            }

            if (porudzbine == null || porudzbine.Count == 0)
            {
                return NoContent();
            }

            List<PorudzbinaDto> porudzbineDto = new List<PorudzbinaDto>();
            foreach (var porudzbina in porudzbine)
            {
                porudzbineDto.Add(mapper.Map<PorudzbinaDto>(porudzbina));
            }

            var totalCount = porudzbineDto.Count;
            var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            if (totalPages < page || page <= 0)
            {
                return NoContent();
            }
            var itemsPerPage = porudzbineDto.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(itemsPerPage);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[Authorize(Roles = "Administrator, Korisnik")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{Id_porudzbina}")]
        public ActionResult<PorudzbinaDto> GetExactPorudzbina(Guid Id_porudzbina)
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
            var porudzbina = porudzbinaRepository.GetExactPorudzbina(Id_porudzbina);

            if (porudzbina == null)
            {
                return NotFound("Porudzbina with the specified ID not found.");
            }

            return mapper.Map<PorudzbinaDto>(porudzbina);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[Authorize(Roles = "Administrator, Korisnik")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{Id_porudzbina}/{Id_korisnik}/{Id_kontingentKarata}")]
        public ActionResult<PorudzbinaDto> GetPorudzbinaById(Guid Id_porudzbina, Guid Id_korisnik, Guid Id_kontingentKarata)
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
            var porudzbina = porudzbinaRepository.GetPorudzbinaById(Id_porudzbina, Id_korisnik, Id_kontingentKarata);

            if (porudzbina == null)
            {
                return NotFound("Porudzbina with the specified ID not found.");
            }

            return mapper.Map<PorudzbinaDto>(porudzbina);
        }


        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[Authorize(Roles = "Administrator, Korisnik")]
        public ActionResult<PorudzbinaDto> CreatePorudzbina([FromBody] PorudzbinaCreationDto porudzbinaCreationDto)
        {
            if (porudzbinaCreationDto == null)
            {
                return BadRequest("Invalid data provided!");
            }

            try
            {
                /*if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return Unauthorized("Da biste izvršili operaciju, morate kreirati nalog!");
                }
                var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && (c.Value == "Administrator" || c.Value == "Korisnik"));

                if (roleClaim == null)
                {
                    return Forbid(); "You don't have permission to create porudzbina."
                }*/

                Porudzbina porudzbinaEntity = mapper.Map<Porudzbina>(porudzbinaCreationDto);
                Porudzbina createdPorudzbina = porudzbinaRepository.CreatePorudzbina(porudzbinaEntity);
                return Ok(mapper.Map<PorudzbinaDto>(createdPorudzbina));
            }
            catch (Exception ex)
            {
                //return StatusCode(StatusCodes.Status500InternalServerError, $"Create Error {ex.InnerException.Message ?? ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }

        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[Authorize(Roles = "Administrator")]
        public ActionResult<PorudzbinaDto> UpdatePorudzbina(PorudzbinaUpdateDto porudzbinaUpdateDto)
        {
            try
            {
                /*
                 if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return Unauthorized("Da biste izvršili operaciju, morate kreirati nalog!");
                }
                var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && (c.Value == "Administrator"));

                if (roleClaim == null)
                {
                    return Forbid(); //"You don't have permission to create porudzbina."
                }
                 */

                Porudzbina porudzbinaEntity = mapper.Map<Porudzbina>(porudzbinaUpdateDto);

                var updatedPorudzbina = porudzbinaRepository.UpdatePorudzbina(porudzbinaEntity); 

                PorudzbinaDto updatedPorudzbinaDto = mapper.Map<PorudzbinaDto>(updatedPorudzbina);
                return Ok(updatedPorudzbinaDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Porudzbina with the specified ID not found.");
            }
            catch (Exception ex)
            {
                // return StatusCode(StatusCodes.Status500InternalServerError, $"Update Error {ex.InnerException.InnerException.Message ?? ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error.");
            }
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{Id_porudzbina}/{Id_korisnik}/{Id_kontingentKarata}")]
        //[Authorize(Roles = "Administrator")]
        public IActionResult DeletePorudzbina(Guid Id_porudzbina, Guid Id_korisnik, Guid Id_kontingentKarata)
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
                    return Forbid(); // "You don't have permission to create porudzbina."
                }*/

                var porudzbina = porudzbinaRepository.GetPorudzbinaById(Id_porudzbina, Id_korisnik, Id_kontingentKarata);
                if (porudzbina == null)
                {
                    return NotFound("Porudzbina with the specified ID not found.");
                }

                porudzbinaRepository.DeletePorudzbina(Id_porudzbina, Id_korisnik, Id_kontingentKarata);

                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }


        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[Authorize(Roles = "Administrator, Korisnik")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("GetPorudzbinaByKorisnik/{Id_korisnik}")]
        public ActionResult<List<PorudzbinaDto>> GetPorudzbinaByKorisnik(Guid Id_korisnik)
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
            var porudzbine = porudzbinaRepository.GetPorudzbinaByKorisnik(Id_korisnik);

            if (porudzbine == null || porudzbine.Count == 0)
            {
                return NotFound("Porudzbina with the specified Korisnik not found.");

            }

            List<PorudzbinaDto> porudzbineDto = new List<PorudzbinaDto>();
            foreach(var porudzbina in porudzbine)
            {
                porudzbineDto.Add(mapper.Map<PorudzbinaDto>(porudzbina));
            }

            return mapper.Map<List<PorudzbinaDto>>(porudzbineDto);
        }
    }
}
