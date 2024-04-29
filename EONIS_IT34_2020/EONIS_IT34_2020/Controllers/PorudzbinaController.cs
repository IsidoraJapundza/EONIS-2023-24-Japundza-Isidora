using AutoMapper;
using EONIS_IT34_2020.Data.PorudzbinaRepository;
using EONIS_IT34_2020.Models.DTOs.Porudzbina;
using EONIS_IT34_2020.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult<List<PorudzbinaDto>> GetPorudzbina()
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

            if (porudzbine == null || porudzbine.Count == 0)
            {
                return NoContent();
            }

            List<PorudzbinaDto> porudzbineDto = new List<PorudzbinaDto>();
            foreach (var porudzbina in porudzbine)
            {
                porudzbineDto.Add(mapper.Map<PorudzbinaDto>(porudzbina));
            }
            return Ok(porudzbineDto);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[Authorize(Roles = "Administrator, Korisnik")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{Id_korisnik}/{Id_kontigentKarata}")]
        public ActionResult<PorudzbinaDto> GetPorudzbinaById(Guid Id_korisnik, Guid Id_kontigentKarata)
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
            var porudzbina = porudzbinaRepository.GetPorudzbinaById(Id_korisnik, Id_kontigentKarata);

            if (porudzbina == null)
            {
                return NotFound();
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
                    return Forbid();
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
                    return Forbid();
                }
                 */

                Porudzbina porudzbinaEntity = mapper.Map<Porudzbina>(porudzbinaUpdateDto);

                var updatedPorudzbina = porudzbinaRepository.UpdatePorudzbina(porudzbinaEntity); 

                PorudzbinaDto updatedPorudzbinaDto = mapper.Map<PorudzbinaDto>(updatedPorudzbina);
                return Ok(updatedPorudzbinaDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
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
        //[HttpGet("{Id_porudzbina}/{Id_korisnik}/{Id_kontigentKarata}")]
        [HttpDelete("{Id_korisnik}/{Id_kontigentKarata}")]
        //[Authorize(Roles = "Administrator")]
        public IActionResult DeletePorudzbina(Guid Id_korisnik, Guid Id_kontigentKarata)
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

                var porudzbina = porudzbinaRepository.GetPorudzbinaById(Id_korisnik, Id_kontigentKarata);
                if (porudzbina == null)
                {
                    return NotFound();
                }

                porudzbinaRepository.DeletePorudzbina(Id_korisnik, Id_kontigentKarata);

                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }

    }
}
