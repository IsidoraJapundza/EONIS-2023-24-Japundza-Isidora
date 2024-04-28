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
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<List<AdministratorDto>> GetAdministrator()
        {
            var administratori = administratorRepository.GetAdministrator();

            if (administratori == null || administratori.Count == 0)
            {
                return NoContent();
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
        public ActionResult<AdministratorDto> GetAdministratorById(Guid Id_administrator) //dto?
        {
            var administrator = administratorRepository.GetAdministratorById(Id_administrator);

            if (administrator == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<AdministratorDto>(administrator));
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        [HttpGet("{KorisnickoImeAdministratora}")]
        public ActionResult<AdministratorDto> GetAdministratorByKorisnickoIme(string korisnickoIme)
        {
           
            var administrator = administratorRepository.GetAdministratorByKorisnickoIme(korisnickoIme);

            if (administrator == null)
            {
                return NotFound();
            }

            return mapper.Map<AdministratorDto>(administrator);
        }


        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<AdministratorDto> CreateAdministrator([FromBody] AdministratorCreationDto administratorCreationDto) //izmeniti!
        {

            if(administratorCreationDto == null)
            {
                return BadRequest("Invalid data provided!");
            }

            try
            {
                Administrator createdAdministrator = administratorRepository.CreateAdministrator(administratorCreationDto);
                return mapper.Map<AdministratorDto>(createdAdministrator);

                /*var administrator = mapper.Map<Administrator>(administratorCreationDto);
                administratorRepository.CreateAdministrator(administrator);
                administratorRepository.SaveChanges();

                var administratorDto = mapper.Map<AdministratorDto>(administrator);

                return Ok(administratorDto);*/

                //--

                /*Administrator administratorEntity = mapper.Map<Administrator>(administratorCreationDto);
                administratorEntity.Id_administrator = Guid.NewGuid();
                AdministratorDto confirmation = administratorRepository.CreateAdministrator(administratorEntity); // izmeniti

                Console.WriteLine(mapper.Map<AdministratorDto>(confirmation));

                return Ok(mapper.Map<AdministratorDto>(confirmation));*/
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }

        [HttpPut]
        //[Authorize(Roles = "Admin")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<AdministratorDto> UpdateAdministrator(AdministratorUpdateDto administrator) // (Administrator administrator)
        {
            try
            {
                var oldAdministrator = administratorRepository.GetAdministratorById(administrator.Id_administrator);
                if(oldAdministrator == null)
                {
                    return NotFound();
                }
                Administrator administratorEntity = mapper.Map<Administrator>(administrator);
                mapper.Map(administratorEntity, oldAdministrator);

                oldAdministrator.ImeAdministratora = administrator.ImeAdministratora;
                // dopuniti ostalo


                /*if (!string.IsNullOrEmpty(korisnik.Lozinka))
               {
                   korisnik.Lozinka = BCrypt.Net.BCrypt.HashPassword(korisnik.Lozinka);
               }*/

                administratorRepository.SaveChanges();
               
                return Ok(mapper.Map<AdministratorDto>(administrator));

                /*
                 Administrator UpdateAdministrator(AdministratorUpdateDto administrator);


                var updatedAdministrator = administratorRepository.UpdateAdministrator(administrator);
                AdministratorDto updatedAdministratorDto = mapper.Map<AdministratorDto>(updatedAdministrator);
                return Ok(updatedAdministratorDto);
                 */
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
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
    }
}
