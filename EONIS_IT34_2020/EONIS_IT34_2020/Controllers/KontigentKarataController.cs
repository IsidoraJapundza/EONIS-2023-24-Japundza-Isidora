using AutoMapper;
using EONIS_IT34_2020.Data.KontigentKarataRepository;
using EONIS_IT34_2020.Models.DTOs.KontigentKarata;
using EONIS_IT34_2020.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EONIS_IT34_2020.Controllers
{
    [ApiController]
    [Route("api/kontigentKarata")]
    public class KontigentKarataController : Controller
    {
        private readonly IKontigentKarataRepository kontigentKarataRepository;
        private readonly IMapper mapper;

        public KontigentKarataController(IMapper mapper, IKontigentKarataRepository kontigentKarataRepository)
        {
            this.mapper = mapper;
            this.kontigentKarataRepository = kontigentKarataRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<KontigentKarataDto>> GetKontigentKarata()
        {
            var kontigentiKarata = kontigentKarataRepository.GetKontigentKarata();

            if (kontigentiKarata == null || kontigentiKarata.Count == 0)
            {
                NoContent();
            }

            List<KontigentKarataDto> kontigentiKarataDto = new List<KontigentKarataDto>();
            foreach (var kontigentKarata in kontigentiKarata)
            {
                kontigentiKarataDto.Add(mapper.Map<KontigentKarataDto>(kontigentKarata));
            }
            return Ok(kontigentiKarataDto);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        [HttpGet("{Id_kontigentKarata}")]
        public ActionResult<KontigentKarata> GetKontigentKarataById(Guid Id_kontigentKarata)
        {
            var kontigentKarata = kontigentKarataRepository.GetKontigentKarataById(Id_kontigentKarata);

            if (kontigentKarata == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<KontigentKarata>(kontigentKarata));
        }


        [HttpPost]
        [Consumes("application/json")]
        //[Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<KontigentKarataDto> CreateKontigentKarata([FromBody] KontigentKarataCreationDto kontigentKarataCreationDto)
        {
            if(kontigentKarataCreationDto == null)
            {
                return BadRequest("Invalid data provided!");
            }

            try
            {
                KontigentKarata kontigentKarataEntity = mapper.Map<KontigentKarata>(kontigentKarataCreationDto);
                kontigentKarataEntity.Id_kontigentKarata = Guid.NewGuid();
                KontigentKarata confirmation = kontigentKarataRepository.CreateKontigentKarata(kontigentKarataEntity);

                return Ok(mapper.Map<KontigentKarataDto>(confirmation));
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
        public ActionResult<KontigentKarataDto> UpdateKontigentKarata(KontigentKarataUpdateDto kontigentKarataUpdateDto) // proveriti
        {
            try
            {
                var kontigentKarataEntity = kontigentKarataRepository.GetKontigentKarataById(kontigentKarataUpdateDto.Id_kontigentKarata);

                if (kontigentKarataEntity == null)
                {
                    return NotFound();
                }

                kontigentKarataRepository.UpdateKontigentKarata(mapper.Map<KontigentKarata>(kontigentKarataUpdateDto));
                return Ok(mapper.Map<KontigentKarataDto>(kontigentKarataUpdateDto));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error.");
            }
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Authorize(Roles = "Administrator")]
        [HttpDelete("{Id_kontigentKarata}")]
        public IActionResult DeleteKontigentKarata(Guid Id_kontigentKarata)
        {
            try
            {
                KontigentKarata kontigentKarata = kontigentKarataRepository.GetKontigentKarataById(Id_kontigentKarata);
                if (kontigentKarata == null)
                {
                    return NotFound();
                }

                kontigentKarataRepository.DeleteKontigentKarata(Id_kontigentKarata);
                kontigentKarataRepository.SaveChanges();
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }

    }
}
