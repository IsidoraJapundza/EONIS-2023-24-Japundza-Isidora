using AutoMapper;
using EONIS_IT34_2020.Data.AdministratorRepository;
using EONIS_IT34_2020.Models.DTOs.Administrator;
using EONIS_IT34_2020.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO.MemoryMappedFiles;
using System.Security.Claims;

namespace EONIS_IT34_2020.Controllers
{
    [ApiController]
    [Route("api/administrator")]
    public class AdministratorController : Controller
    {
        private readonly IAdministratorRepository administratorRepository;
        private readonly IMapper mapper;

        public AdministratorController(IMapper mapper, IAdministratorRepository administratorRepository)
        {
            this.mapper = mapper;
            this.administratorRepository = administratorRepository;
        }

        [HttpGet]
        //[Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<List<AdministratorDto>> GetAdministrator(int page = 1, int pageSize = 10, bool sortByKorisnickoIme = false, string sortOrder = "asc")
        {
            /*
             if (!HttpContext.User.Identity.IsAuthenticated)
           {
               return Unauthorized("Da biste izvršili operaciju, morate kreirati nalog!");
           }
           var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && (c.Value == "Administrator" ));

           if (roleClaim == null)
           {
               return Forbid();
           }
           */

            var administratori = administratorRepository.GetAdministrator();

            if (sortByKorisnickoIme)
            {
                administratori = sortOrder.ToLower() == "asc" ? administratori.OrderBy(a => a.KorisnickoImeAdministratora).ToList() : administratori.OrderByDescending(a => a.KorisnickoImeAdministratora).ToList();
            }

            if (administratori == null || administratori.Count == 0)
            {
                return NoContent();
            }

            List<AdministratorDto> administratoriDto = new List<AdministratorDto>();
            foreach (var administrator in administratori)
            {
                administratoriDto.Add(mapper.Map<AdministratorDto>(administrator));
            }

            var totalCount = administratoriDto.Count;
            var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            if (totalPages < page || page <= 0)
            {
                return NoContent();
            }
            var itemsPerPage = administratoriDto.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(itemsPerPage);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{Id_administrator}")]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[Authorize(Roles = "Admin")]
        public ActionResult<AdministratorDto> GetAdministratorById(Guid Id_administrator) 
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
            var administrator = administratorRepository.GetAdministratorById(Id_administrator);

            if (administrator == null)
            {
                return NotFound("Administrator with the specified ID not found.");
            }
            return mapper.Map<AdministratorDto>(administrator);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        [HttpGet("username/{korisnickoIme}")]
        public ActionResult<AdministratorDto> GetAdministratorByKorisnickoIme(string korisnickoIme)
        {

            var administrator = administratorRepository.GetAdministratorByKorisnickoIme(korisnickoIme);

            if (administrator == null)
            {
                return NotFound($"Administrator with the specified KorisnickoIme {administrator.KorisnickoImeAdministratora} not found.");
            }

            return mapper.Map<AdministratorDto>(administrator);
        }

 
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<AdministratorDto> CreateAdministrator([FromBody] AdministratorCreationDto administratorCreationDto) 
        {

            if(administratorCreationDto == null)
            {
                return BadRequest("Invalid data provided!");
            }

            try
            {
                Administrator createdAdministrator = administratorRepository.CreateAdministrator(administratorCreationDto);
                return mapper.Map<AdministratorDto>(createdAdministrator);

                /*Administrator administratorEntity = mapper.Map<Administrator>(administratorCreationDto);
                administratorEntity.Id_administrator = Guid.NewGuid();
                AdministratorDto confirmation = administratorRepository.CreateAdministrator(administratorEntity);

                return Ok(mapper.Map<AdministratorDto>(confirmation));*/
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }

        [HttpPut]
        //[Authorize(Roles = "Administrator")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<AdministratorDto> UpdateAdministrator(AdministratorUpdateDto administratorUpdateDto) 
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
                   return Forbid(); // "You don't have permission to update administrator."
               } 
                */

                Administrator administrator = mapper.Map<Administrator>(administratorUpdateDto);
                var updatedAdministrator = administratorRepository.UpdateAdministrator(administratorUpdateDto);

                AdministratorDto updatedAdministratorDto = mapper.Map<AdministratorDto>(updatedAdministrator);

                return Ok(updatedAdministratorDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Administrator with the specified ID not found.");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error.");
            }
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{Id_administrator}")]
        //[Authorize(Roles = "Administrator")]
        public IActionResult DeleteAdministrator(Guid Id_administrator)
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
                Administrator administrator = administratorRepository.GetAdministratorById(Id_administrator);
                if (administrator == null)
                {
                    return NotFound("Administrator with the specified ID not found.");
                }

                administratorRepository.DeleteAdministrator(Id_administrator);
                administratorRepository.SaveChanges();
                return StatusCode(StatusCodes.Status204NoContent, "\"Administrator with the specified ID successfully deleted.\"");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("korisnickoIme/{KorisnickoImeAdministratora}")]
        //[Authorize(Roles = "Administrator")]
        public IActionResult DeleteAdministratorByKorisnickoIme(string korisnickoIme)
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
                Administrator administrator = administratorRepository.GetAdministratorByKorisnickoIme(korisnickoIme);
                if (administrator == null)
                {
                    return NotFound("Administrator with the specified username not found.");
                }

                administratorRepository.DeleteAdministrator(korisnickoIme);
                administratorRepository.SaveChanges();
                return StatusCode(StatusCodes.Status204NoContent, "\"Administrator with the specified username successfully deleted.\"");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }
    }
}
