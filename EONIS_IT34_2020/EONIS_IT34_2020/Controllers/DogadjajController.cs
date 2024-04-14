using AutoMapper;
using ERP2024.Data.DogadjajRepository;
using ERP2024.Models.DTOs.Dogadjaj;
using ERP2024.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ERP2024.Controllers
{
    [ApiController]
    [Route("api/dogadjaj")]
    public class DogadjajController : Controller
    {
        private readonly IDogadjajRepository dogadjajRepository;
        private readonly IMapper mapper;

        public DogadjajController(IMapper mapper, IDogadjajRepository dogadjajRepository)
        {
            this.mapper = mapper;
            this.dogadjajRepository = dogadjajRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<DogadjajDto>> GetDogadjaj()
        {
            List<Dogadjaj> dogadjaji = korisnikRepository.GetDogadjaj();

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
        [HttpGet("{Id_dogadjaj}")]
        public ActionResult<Dogadjaj> GetDogadjajById(Guid Id_dogadjaj)
        {
            Dogadjaj dogadjaj = dogadjajRepository.GetDogadjajById(Id_dogadjaj);

            if (dogadjaj == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<Dogadjaj>(dogadjaj));
        }


        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<DogadjajDto> CreateDogadjaj([FromBody] DogadjajCreationDto dogadjaj)
        {
            try
            {
                dogadjaj dogadjajEntity = mapper.Map<Dogadjaj>(dogadjaj);
                dogadjajEntity.Id_dogadjaj = Guid.NewGuid();
                DogadjajDto confirmation = dogadjajRepository.CreateDogadjaj(dogadjajEntity);

                Console.WriteLine(mapper.Map<DogadjajDto>(confirmation));

                return Ok(mapper.Map<DogadjajDto>(confirmation));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{Id_dogadjaj}")]
        public IActionResult DeleteDogadjaj(Guid Id_dogadjaj)
        {
            try
            {
                Dogadjaj dogadjaj = dogadjajRepository.GetDogadjajById(Id_dogadjaj);
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


        [HttpPut]
        //[Authorize(Roles = "Korisnik")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<DogadjajDto> UpdateDogadjaj(DogadjajUpdateDto dogadjajUpdateDto)
        {
            try
            {
                var dogadjajEntity = dogadjajRepository.GetDogadjajById(dogadjaj.Id_dogadjaj);

                if (dogadjajEntity == null)
                {
                    return NotFound();
                }

                dogadjajEntityRepository.UpdateDogadjaj(mapper.Map<Dogadjaj>(dogadjaj));
                return Ok(mapper.Map<DogadjajDto>(dogadjaj));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error.");
            }
        }
    }
}
