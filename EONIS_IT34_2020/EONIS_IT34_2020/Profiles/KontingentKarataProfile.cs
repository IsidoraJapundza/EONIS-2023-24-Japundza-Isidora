using AutoMapper;
using EONIS_IT34_2020.Models.DTOs.KontingentKarata;
using EONIS_IT34_2020.Models.Entities;

namespace EONIS_IT34_2020.Profiles
{
    public class KontingentKarataProfile : Profile
    {
        public KontingentKarataProfile()
        {
            CreateMap<KontingentKarata, KontingentKarataDto>().ReverseMap();
            CreateMap<KontingentKarata, KontingentKarataCreationDto>().ReverseMap();
            CreateMap<KontingentKarata, KontingentKarataUpdateDto>().ReverseMap();
        }
    }
}
