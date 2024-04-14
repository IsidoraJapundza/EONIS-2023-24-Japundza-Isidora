using AutoMapper;
using EONIS_IT34_2020.Models.DTOs.Dogadjaj;
using EONIS_IT34_2020.Models.Entities;

namespace EONIS_IT34_2020.Profiles
{
    public class DogadjajProfile : Profile
    {
        public DogadjajProfile()
        {

            CreateMap<Dogadjaj, DogadjajDto>().ReverseMap();
            CreateMap<Dogadjaj, DogadjajCreationDto>().ReverseMap();
            CreateMap<Dogadjaj, DogadjajUpdateDto>().ReverseMap();
        }
    }
}
