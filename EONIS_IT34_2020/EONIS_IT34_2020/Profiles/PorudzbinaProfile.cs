using AutoMapper;
using EONIS_IT34_2020.Models.DTOs.Porudzbina;
using EONIS_IT34_2020.Models.Entities;

namespace EONIS_IT34_2020.Profiles
{
    public class PorudzbinaProfile : Profile
    {
        public PorudzbinaProfile() {

            CreateMap<Porudzbina, PorudzbinaDto>().ReverseMap();
            CreateMap<Porudzbina, PorudzbinaCreationDto>().ReverseMap();
            CreateMap<Porudzbina, PorudzbinaUpdateDto>().ReverseMap();
        }
    }
}
