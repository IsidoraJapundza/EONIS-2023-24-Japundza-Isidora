using AutoMapper;
using EONIS_IT34_2020.Models.DTOs.KontigentKarata;
using EONIS_IT34_2020.Models.Entities;

namespace EONIS_IT34_2020.Profiles
{
    public class KontigentKarataProfile : Profile
    {
        public KontigentKarataProfile()
        {

            CreateMap<KontigentKarata, KontigentKarataDto>().ReverseMap();
            CreateMap<KontigentKarata, KontigentKarataCreationDto>().ReverseMap();
            CreateMap<KontigentKarata, KontigentKarataUpdateDto>().ReverseMap();
        }
    }
}
