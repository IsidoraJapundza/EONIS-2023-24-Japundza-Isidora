using AutoMapper;
using EONIS_IT34_2020.Models.DTOs.Administrator;
using EONIS_IT34_2020.Models.Entities;

namespace EONIS_IT34_2020.Profiles
{
    public class AdministratorProfile : Profile
    {
        public AdministratorProfile()
        {
            CreateMap<Administrator, AdministratorDto>().ReverseMap();
            CreateMap<Administrator, AdministratorCreationDto>().ReverseMap();
            CreateMap<Administrator, AdministratorUpdateDto>().ReverseMap();
        }
    }
}
