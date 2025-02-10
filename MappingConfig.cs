using AutoMapper;
using MagicVilla_API.Modelo;
using MagicVilla_API.Modelo.Dto;

namespace MagicVilla_API;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<Villa, VillaDto>();
        CreateMap<VillaDto, Villa>();

        CreateMap<Villa, VillaCreateDto>().ReverseMap();
        CreateMap<Villa, VillaUpdateDto>().ReverseMap();

        CreateMap<NumeroVilla, NumeroVillaDto>().ReverseMap();
        CreateMap<NumeroVilla, NumeroVillaCreateDto>().ReverseMap();
        CreateMap<NumeroVilla, NumeroVillaUpdateDto>().ReverseMap();
    }
}
