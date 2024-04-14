using AutoMapper;
using ERP2024.Data.AdministratorRepository;
using ERP2024.Models.DTOs.Administrator;
using ERP2024.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ERP2024.Controllers
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
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<List<AdministratorDto>> GetAdministrator()
        {
            List<Administrator> administratori = administratorRepository.GetAdministrator();

            if (administratori == null || administratori.Count == 0)
            {
                NoContent();
            }

            List<AdministratorDto> administratoriDto = new List<AdministratorDto>();
            foreach (var administrator in administratori)
            {
                administratoriDto.Add(mapper.Map<AdministratorDto>(administrator));
            }
            return Ok(administratoriDto);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{Id_administrator}")]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[Authorize(Roles = "Admin")]
        public ActionResult<Administrator> GetAdministratorById(Guid Id_administrator)
        {
            Administrator administrator = administratorRepository.GetAdministratorById(Id_administrator);

            if (administrator == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<Administrator>(administrator));
        }

        
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<AdministratorDto> CreateAdministrator([FromBody] AdministratorCreationDto administrator)
        {
            try
            {
                administrator administratorEntity = mapper.Map<Administrator>(administrator);
                administratorEntity.Id_administrator = Guid.NewGuid();
                AdministratorDto confirmation = administratorRepository.CreateAdministrator(administratorEntity);

                Console.WriteLine(mapper.Map<AdministratorDto>(confirmation));

                return Ok(mapper.Map<AdministratorDto>(confirmation));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{Id_administrator}")]
        //[Authorize(Roles = "Admin")]
        public IActionResult DeleteAdministrator(Guid Id_administrator)
        {
            try
            {
                Administrator administrator = administratorRepository.GetAdministratorById(Id_administrator);
                if (administrator == null)
                {
                    return NotFound();
                }

                administratorRepository.DeleteAdministrator(Id_administrator);
                administratorRepository.SaveChanges();
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }


        [HttpPut]
        //[Authorize(Roles = "Admin")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<AdministratorDto> UpdateAdministrator(AdministratorUpdateDto administratorUpdateDto)
        {
            try
            {
                var administratorEntity = administratorRepository.GetAdministratorById(administrator.Id_administrator);

                if (administratorEntity == null)
                {
                    return NotFound();
                }

                /*if (!string.IsNullOrEmpty(korisnik.Lozinka))
                {
                    korisnik.Lozinka = BCrypt.Net.BCrypt.HashPassword(korisnik.Lozinka);
                }*/

                administratorRepository.UpdateAdministrator(mapper.Map<Administrator>(administrator));
                return Ok(mapper.Map<AdministratorDto>(administrator));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error.");
            }
        }
    }
}
