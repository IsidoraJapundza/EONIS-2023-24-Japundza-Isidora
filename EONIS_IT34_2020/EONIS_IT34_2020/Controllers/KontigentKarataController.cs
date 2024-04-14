using AutoMapper;
using ERP2024.Data.KontigentKarataRepository;
using ERP2024.Models.DTOs.KontigentKarata;
using ERP2024.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ERP2024.Controllers
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<KontigentKarataDto>> GetKontigentKarata()
        {
            List<KontigentKarata> kontigentiKarata = kontigentKarataRepository.GetKontigentKarata();

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
        [HttpGet("{Id_kontigentKarata}")]
        public ActionResult<KontigentKarata> GetKontigentKarataById(Guid Id_kontigentKarata)
        {
            KontigentKarata kontigentKarata = kontigentKarataRepository.GetKontigentKarataById(Id_kontigentKarata);

            if (kontigentKarata == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<KontigentKarata>(kontigentKarata));
        }


        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<KontigentKarataDto> CreateKontigentKarata([FromBody] KontigentKarataCreationDto kontigentKarata)
        {
            try
            {
                kontigentKarata kontigentKarataEntity = mapper.Map<KontigentKarata>(kontigentKarata);
                kontigentKarataEntity.Id_kontigentKarata = Guid.NewGuid();
                KontigentKarataDto confirmation = kontigentKarataRepository.CreateKontigentKarata(kontigentKarataEntity);

                Console.WriteLine(mapper.Map<KontigentKarataDto>(confirmation));

                return Ok(mapper.Map<KontigentKarataDto>(confirmation));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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


        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<KontigentKarataDto> UpdateKontigentKarata(KontigentKarataUpdateDto kontigentKarataUpdateDto)
        {
            try
            {
                var kontigentKarataEntity = kontigentKarataRepository.GetKontigentKarataById(kontigentKarata.Id_kontigentKarata);

                if (kontigentKarata == null)
                {
                    return NotFound();
                }

                kontigentKarataEntityRepository.UpdateKontigentKarata(mapper.Map<KontigentKarata>(kontigentKarata));
                return Ok(mapper.Map<KontigentKarataDto>(kontigentKarata));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error.");
            }
        }
    }
}
