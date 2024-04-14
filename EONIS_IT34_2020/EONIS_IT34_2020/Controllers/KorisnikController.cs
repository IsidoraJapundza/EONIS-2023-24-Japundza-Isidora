using AutoMapper;
using ERP2024.Data.KorisnikRepository;
using ERP2024.Models.DTOs.Korisnik;
using ERP2024.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ERP2024.Controllers
{
    [ApiController]
    [Route("api/korisnik")]
    public class KorisnikController : Controller
    {
        private readonly IKorisnikRepository korisnikRepository;
        private readonly IMapper mapper;

        public KorisnikController(IMapper mapper, IKorisnikRepository korisnikRepository)
        {
            this.mapper = mapper;
            this.korisnikRepository = korisnikRepository;
        }

        [HttpGet]
        //[Authorize(Roles = "Korisnik")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<List<KorisnikDto>> GetKorisnik()
        {
            List<Korisnik> korisnici = korisnikRepository.GetKorisnik();

            if (korisnici == null || korisnici.Count == 0)
            {
                NoContent();
            }

            List<KorisnikDto> korisniciDto = new List<KorisnikDto>();
            foreach (var korisnik in korisnici)
            {
                korisniciDto.Add(mapper.Map<KorisnikDto>(korisnik));
            }
            return Ok(korisniciDto);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{Id_korisnik}")]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[Authorize(Roles = "Admin")]
        public ActionResult<Korisnik> GetKorisnikById(Guid Id_korisnik)
        {
            Korisnik korisnik = korisnikRepository.GetKorisnikById(Id_korisnik);
            
            if (korisnik == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<Korisnik>(korisnik));
        }


        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<KorisnikDto> CreateKorisnik([FromBody] KorisnikCreationDto korisnik)
        {
            try
            {
                korisnik korisnikEntity = mapper.Map<Korisnik>(korisnik);
                korisnikEntity.Id_korisnik = Guid.NewGuid();
                KorisnikDto confirmation = korisnikRepository.CreateKorisnik(korisnikEntity);

                Console.WriteLine(mapper.Map<KorisnikDto>(confirmation));

                return Ok(mapper.Map<KorisnikDto>(confirmation));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create Error");
            }
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{Id_korisnik}")]
        //[Authorize(Roles = "Korisnik")]
        public IActionResult DeleteKorisnik(Guid Id_korisnik)
        {
            try
            {
                Korisnik korisnik = korisnikRepository.GetKorisnikById(Id_korisnik);
                if (korisnik == null)
                {
                    return NotFound();
                }

                korisnikRepository.DeleteKorisnik(Id_korisnik);
                korisnikRepository.SaveChanges();
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
        public ActionResult<KorisnikDto> UpdateKorisnik(KorisnikUpdateDto korisnikUpdateDto)
        {
            try
            {
                var korisnikEntity = korisnikRepository.GetKorisnikById(korisnik.Id_korisnik);

                if (korisnikEntity == null)
                {
                    return NotFound();
                }

                /*if (!string.IsNullOrEmpty(korisnik.Lozinka))
                {
                    korisnik.Lozinka = BCrypt.Net.BCrypt.HashPassword(korisnik.Lozinka);
                }*/

                korisnikRepository.UpdateKorisnik(mapper.Map<Korisnik>(korisnik));
                return Ok(mapper.Map<KorisnikDto>(korisnik));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error.");
            }
        }
    }
}
