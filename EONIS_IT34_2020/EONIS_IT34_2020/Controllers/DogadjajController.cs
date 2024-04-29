using AutoMapper;
using EONIS_IT34_2020.Data.DogadjajRepository;
using EONIS_IT34_2020.Models.DTOs.Dogadjaj;
using EONIS_IT34_2020.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EONIS_IT34_2020.Controllers
{
    [ApiController]
    [Route("api/dogadjaj")]
    public class DogadjajController : Controller
    {
        private readonly IDogadjajRepository dogadjajRepository;
        private readonly IMapper mapper;

        public DogadjajController(IDogadjajRepository dogadjajRepository, IMapper mapper)
        {
            this.dogadjajRepository = dogadjajRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<DogadjajDto>> GetDogadjaj()
        {
            /*
             if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized("Da biste izvršili operaciju, morate kreirati nalog!");
            }
             */
            var dogadjaji = dogadjajRepository.GetDogadjaj();

            if (dogadjaji == null || dogadjaji.Count == 0)
            {
                NoContent();
            }

            List<DogadjajDto> dogadjajiDto = new List<DogadjajDto>();
            foreach (var dogadjaj in dogadjaji)
            {
                dogadjajiDto.Add(mapper.Map<DogadjajDto>(dogadjaj));
            }
            return Ok(dogadjajiDto);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        [HttpGet("{Id_dogadjaj}")]
        public ActionResult<DogadjajDto> GetDogadjajById(Guid Id_dogadjaj)
        {
            var dogadjaj = dogadjajRepository.GetDogadjajById(Id_dogadjaj);

            if (dogadjaj == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<DogadjajDto>(dogadjaj));
        }


        [HttpPost]
        //[Authorize(Roles = "Administrator")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<DogadjajDto> CreateDogadjaj([FromBody] DogadjajCreationDto dogadjajCreationDto) 
        {
            if (dogadjajCreationDto == null)
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
                    return Forbid();
                }*/

                Dogadjaj dogadjajEntity = mapper.Map<Dogadjaj>(dogadjajCreationDto);
                dogadjajEntity.Id_dogadjaj = Guid.NewGuid();
                Dogadjaj confirmation = dogadjajRepository.CreateDogadjaj(dogadjajEntity);
                return Ok(mapper.Map<DogadjajDto>(confirmation));
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
        public ActionResult<DogadjajDto> UpdateDogadjaj(DogadjajUpdateDto dogadjajUpdateDto) 
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

                Dogadjaj dogadjaj = mapper.Map<Dogadjaj>(dogadjajUpdateDto);

                var dogadjajEntity = dogadjajRepository.UpdateDogadjaj(dogadjaj);

                DogadjajDto dogadjajDto = mapper.Map<DogadjajDto>(dogadjajEntity);
                return Ok(dogadjajDto);

                /*Dogadjaj dogadjaj = mapper.Map<Dogadjaj>(dogadjajUpdateDto);

                var dogadjajEntity = dogadjajRepository.GetDogadjajById(dogadjajUpdateDto.Id_dogadjaj);

                if (dogadjajEntity == null)
                {
                    return NotFound();
                }

                dogadjajRepository.UpdateDogadjaj(mapper.Map<Dogadjaj>(dogadjajUpdateDto));
                return Ok(mapper.Map<DogadjajDto>(dogadjajUpdateDto));*/
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
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[Authorize(Roles = "Administrator")]
        [HttpDelete("{Id_dogadjaj}")]
        public IActionResult DeleteDogadjaj(Guid Id_dogadjaj)
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

                var dogadjaj = dogadjajRepository.GetDogadjajById(Id_dogadjaj);
                if (dogadjaj == null)
                {
                    return NotFound();
                }

                dogadjajRepository.DeleteDogadjaj(Id_dogadjaj);
                dogadjajRepository.SaveChanges();
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }
    }
}
