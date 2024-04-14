using AutoMapper;
using EONIS_IT34_2020.Models.DTOs.Korisnik;
using EONIS_IT34_2020.Models.Entities;

namespace EONIS_IT34_2020.Profiles
{
    public class KorisnikProfile : Profile
    {
        public KorisnikProfile()
        {

            CreateMap<Korisnik, KorisnikDto>().ReverseMap();
            CreateMap<Korisnik, KorisnikCreationDto>().ReverseMap();
            CreateMap<Korisnik, KorisnikUpdateDto>().ReverseMap();
        }
    }
}
