﻿using AutoMapper;
using EONIS_IT34_2020.Data.DogadjajRepository;
using EONIS_IT34_2020.Models.DTOs.Dogadjaj;
using EONIS_IT34_2020.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
        public ActionResult<List<DogadjajDto>> GetDogadjaj(int page = 1, int pageSize = 10, bool sortByDatumOdrzavanja = false, string sortOrder = "asc")
        {
                        
            var dogadjaji = dogadjajRepository.GetDogadjaj();

            if (sortByDatumOdrzavanja)
            {
                dogadjaji = sortOrder.ToLower() == "asc" ? dogadjaji.OrderBy(a => a.DatumOdrzavanja).ToList() : dogadjaji.OrderByDescending(a => a.DatumOdrzavanja).ToList();
            }


            if (dogadjaji == null || dogadjaji.Count == 0)
            {
                NoContent();
            }

            List<DogadjajDto> dogadjajiDto = new List<DogadjajDto>();
            foreach (var dogadjaj in dogadjaji)
            {
                dogadjajiDto.Add(mapper.Map<DogadjajDto>(dogadjaj));
            }

            var totalCount = dogadjajiDto.Count;
            var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            if (totalPages < page || page <= 0)
            {
                return StatusCode(StatusCodes.Status204NoContent, "Dogadjaj successfully deleted."); 
            }
            var itemsPerPage = dogadjajiDto.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(itemsPerPage);
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[AllowAnonymous]
        [HttpGet("{Id_dogadjaj}")]
        //[Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<DogadjajDto> GetDogadjajById(Guid Id_dogadjaj)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized("Da biste izvršili operaciju, morate kreirati nalog!");
            }
            var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && (c.Value == "Administrator"));

            if (roleClaim == null)
            {
                return Forbid();
            }

            var dogadjaj = dogadjajRepository.GetDogadjajById(Id_dogadjaj);

            if (dogadjaj == null)
            {
                return NotFound("Dogadjaj with the specified ID not found.");
            }
            return Ok(mapper.Map<DogadjajDto>(dogadjaj));
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        [HttpGet("naziv/{naziv}")]
        public ActionResult<List<DogadjajDto>> GetDogadjajByNaziv(string naziv)
        {

            var dogadjaji = dogadjajRepository.GetDogadjajByNaziv(naziv);

            if (dogadjaji == null || dogadjaji.Count == 0)
            {
                return NotFound("Dogadjaj with the specified NazivSporstkogDogadjaja not found.");
            }

            List<DogadjajDto> dogadjajiDto = new List<DogadjajDto>();
            foreach (var dogadjaj in dogadjaji)
            {
                dogadjajiDto.Add(mapper.Map<DogadjajDto>(dogadjaj));
            }
            return mapper.Map<List<DogadjajDto>>(dogadjajiDto);
        }


        [HttpPost]
        //[Authorize(Roles = "Administrator")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public ActionResult<DogadjajDto> CreateDogadjaj([FromBody] DogadjajCreationDto dogadjajCreationDto) 
        {
            if (dogadjajCreationDto == null)
            {
                return BadRequest("Invalid data provided!");
            }

            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return Unauthorized("Da biste izvršili operaciju, morate kreirati nalog!");
                }
                var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && (c.Value == "Administrator"));

                if (roleClaim == null)
                {
                    return Forbid(); 
                }

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
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<DogadjajDto> UpdateDogadjaj(DogadjajUpdateDto dogadjajUpdateDto) 
        {
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return Unauthorized("Da biste izvršili operaciju, morate kreirati nalog!");
                }
                var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && (c.Value == "Administrator"));

                if (roleClaim == null)
                {
                    return Forbid(); 
                }

                Dogadjaj dogadjaj = mapper.Map<Dogadjaj>(dogadjajUpdateDto);

                var dogadjajEntity = dogadjajRepository.UpdateDogadjaj(dogadjaj);

                DogadjajDto dogadjajDto = mapper.Map<DogadjajDto>(dogadjajEntity);
                return Ok(dogadjajDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Dogadjaj with the specified ID not found.");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error.");
            }
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[Authorize(Roles = "Administrator")]
        [HttpDelete("{Id_dogadjaj}")]
        public IActionResult DeleteDogadjaj(Guid Id_dogadjaj)
        {
            try
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return Unauthorized("Da biste izvršili operaciju, morate kreirati nalog!");
                }
                var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && (c.Value == "Administrator"));

                if (roleClaim == null)
                {
                    return Forbid(); 
                }

                var dogadjaj = dogadjajRepository.GetDogadjajById(Id_dogadjaj);
                if (dogadjaj == null)
                {
                    return NotFound("Dogadjaj with specified ID not found.");
                }

                dogadjajRepository.DeleteDogadjaj(Id_dogadjaj);
                dogadjajRepository.SaveChanges();
                return StatusCode(StatusCodes.Status204NoContent, "\"Dogadjaj with the specified ID successfully deleted.\"");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete Error");
            }
        }
    }
}
