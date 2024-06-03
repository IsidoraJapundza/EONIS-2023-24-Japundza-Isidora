using AutoMapper;
using EONIS_IT34_2020.Data.KontingentKarataRepository;
using EONIS_IT34_2020.Data.KorisnikRepository;
using EONIS_IT34_2020.Models.DTOs.KontingentKarata;
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
    [Route("api/kontingentKarata")]
    public class KontingentKarataController : Controller
    {
        private readonly IKontingentKarataRepository kontingentKarataRepository;
        private readonly IMapper mapper;

        public KontingentKarataController(IKontingentKarataRepository kontingentKarataRepository, IMapper mapper)
        {
            this.kontingentKarataRepository = kontingentKarataRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<KontingentKarataDto>> GetKontingentKarata(int page = 1, int pageSize = 10, bool sortByCena = false, string sortOrder = "asc")
        {
            var kontingentiKarata = kontingentKarataRepository.GetKontingentKarata();

            if (sortByCena)
            {
                kontingentiKarata = sortOrder.ToLower() == "asc" ? kontingentiKarata.OrderBy(a => a.Cena).ToList() : kontingentiKarata.OrderByDescending(a => a.Cena).ToList();
            }

            if (kontingentiKarata == null || kontingentiKarata.Count == 0) 
            {
                NoContent();
            }

            List<KontingentKarataDto> kontingentiKarataDto = new List<KontingentKarataDto>();
            foreach (var kontingentKarata in kontingentiKarata)
            {
                kontingentiKarataDto.Add(mapper.Map<KontingentKarataDto>(kontingentKarata));
            }

            var totalCount = kontingentiKarataDto.Count;
            var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            if (totalPages < page || page <= 0)
            {
                return NoContent();
            }
            var itemsPerPage = kontingentiKarataDto.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(itemsPerPage);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        [HttpGet("{Id_kontingentKarata}")]
        public ActionResult<KontingentKarataDto> GetKontingentKarataById(Guid Id_kontingentKarata)
        {
            var kontingentKarata = kontingentKarataRepository.GetKontingentKarataById(Id_kontingentKarata);

            if (kontingentKarata == null)
            {
                return NotFound("KontingentKarata with the specified ID not found.");
            }
            return Ok(mapper.Map<KontingentKarataDto>(kontingentKarata));
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        [HttpGet("nazivKarte/{naziv}")]
        public ActionResult<List<KontingentKarataDto>> GetKontingentKarataByNaziv(string naziv)
        {

            var kontingentiKarata = kontingentKarataRepository.GetKontingentKarataByNaziv(naziv);

            if (kontingentiKarata == null || kontingentiKarata.Count == 0)
            {
                return NotFound("KontigentKarata with the specified NazivKarte not found.");
            }

            List<KontingentKarataDto> kontingentiKarataDto = new List<KontingentKarataDto>();
            foreach (var kontingentKarata in kontingentiKarata)
            {
                kontingentiKarataDto.Add(mapper.Map<KontingentKarataDto>(kontingentKarata));
            }

            return mapper.Map<List<KontingentKarataDto>>(kontingentiKarataDto);
        }

        [HttpPost]
        [Consumes("application/json")]
        //[Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<KontingentKarataDto> CreateKontingentKarata([FromBody] KontingentKarataCreationDto kontingentKarataCreationDto)
        {
            if(kontingentKarataCreationDto == null)
            {
                return BadRequest("Invalid data provided!");
            }

            try
            {
                /*if (!HttpContext.User.Identity.IsAuthenticated)
               {
                   return Unauthorized("Da biste izvršili operaciju, morate kreirati nalog!");
               }
               var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && (c.Value == "Administrator"));

               if (roleClaim == null)
               {
                   return Forbid(); //"You don't have permission to create kontingentKarata."
               }*/

                KontingentKarata kontingentKarataEntity = mapper.Map<KontingentKarata>(kontingentKarataCreationDto);
                kontingentKarataEntity.Id_kontingentKarata = Guid.NewGuid();
                KontingentKarata confirmation = kontingentKarataRepository.CreateKontingentKarata(kontingentKarataEntity);

                return Ok(mapper.Map<KontingentKarataDto>(confirmation));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }


        [HttpPut]
        [Consumes("application/json")]
        //[Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<KontingentKarataDto> UpdateKontingentKarata(KontingentKarataUpdateDto kontingentKarataUpdateDto) 
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
                    return Forbid(); //"You don't have permission to update kontingentKarata."
                }*/

                KontingentKarata kontingentKarata = mapper.Map<KontingentKarata>(kontingentKarataUpdateDto);

                var kontingentKarataEntity = kontingentKarataRepository.UpdateKontingentKarata(kontingentKarata);

                KontingentKarataDto kontingentKarataDto = mapper.Map<KontingentKarataDto>(kontingentKarataEntity);
                return Ok(kontingentKarataDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("KontingentKarata with the specified ID not found.");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error.");
            }
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[Authorize(Roles = "Administrator")]
        [HttpDelete("{Id_kontingentKarata}")]
        public IActionResult DeleteKontingentKarata(Guid Id_kontingentKarata)
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
                    return Forbid(); //"You don't have permission to delete kontingentKarata."
                }*/

                var kontingentKarata = kontingentKarataRepository.GetKontingentKarataById(Id_kontingentKarata);
                if (kontingentKarata == null)
                {
                    return NotFound("KontingentKarata with the specified ID not found.");
                }

                kontingentKarataRepository.DeleteKontingentKarata(Id_kontingentKarata);
                kontingentKarataRepository.SaveChanges();
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }
    }
}