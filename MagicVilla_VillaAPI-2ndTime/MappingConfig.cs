using MagicVilla_VillaAPI_2ndTime.Model;
using MagicVilla_VillaAPI_2ndTime.Model.Dto;
using AutoMapper;

namespace MagicVilla_VillaAPI_2ndTime
{
    public class MappingConfig: Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa,VillaDTO>().ReverseMap();
            CreateMap<Villa,VillaCreateDTO>().ReverseMap();
            CreateMap<Villa, VillaUpdateDTO>().ReverseMap();

            CreateMap<VillaNumber, VillaNumberDTO>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberCreateDTO>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberUpdateDTO>().ReverseMap();

        }

       
    }
}
