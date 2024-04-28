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
        [HttpGet("{Id_korisnik}/{Id_kontigentKarata}")]
        public ActionResult<PorudzbinaDto> GetPorudzbinaById(Guid Id_korisnik, Guid Id_kontigentKarata)
        {
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
            try
            {
                Porudzbina porudzbinaEntity = mapper.Map<Porudzbina>(porudzbinaCreationDto);
                Porudzbina createdPorudzbina = porudzbinaRepository.CreatePorudzbina(porudzbinaEntity);
                return Ok(mapper.Map<PorudzbinaDto>(createdPorudzbina));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Create Error {ex.InnerException.Message ?? ex.Message}");
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
                /*Porudzbina porudzbinaEntity = mapper.Map<Porudzbina>(porudzbinaUpdateDto);

                var updatedPorudzbina = porudzbinaRepository.UpdatePorudzbina(porudzbinaEntity); // promeniti metodu

                PorudzbinaDto updatedPorudzbinaDto = mapper.Map<PorudzbinaDto>(updatedPorudzbina);
                return Ok(updatedPorudzbinaDto);*/

                var porudzbinaEntity = porudzbinaRepository.GetPorudzbinaById(porudzbinaUpdateDto.Id_korisnik, porudzbinaUpdateDto.Id_kontigentKarata);

                if (porudzbinaEntity == null)
                {
                    return NotFound();
                }

                porudzbinaRepository.UpdatePorudzbina(mapper.Map<Porudzbina>(porudzbinaUpdateDto));
                return Ok(mapper.Map<PorudzbinaDto>(porudzbinaUpdateDto));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Update Error {ex.InnerException.InnerException.Message ?? ex.Message}");
            }
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{Id_porudzbina}/{Id_korisnik}/{Id_kontigentKarata}")]
        //[Authorize(Roles = "Administrator")]
        public IActionResult DeletePorudzbina(Guid Id_korisnik, Guid Id_kontigentKarata)
        {
            try
            {
                
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
