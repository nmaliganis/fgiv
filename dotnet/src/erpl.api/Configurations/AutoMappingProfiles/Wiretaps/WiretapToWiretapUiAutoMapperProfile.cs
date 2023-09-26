using AutoMapper;
using erpl.common.dtos.DTOs.Wiretaps;
using erpl.model.Wiretaps;

namespace erpl.api.Configurations.AutoMappingProfiles.Wiretaps;

internal class WiretapToWiretapUiAutoMapperProfile : Profile
{
    public WiretapToWiretapUiAutoMapperProfile()
    {
        ConfigureMapping();
    }

    public void ConfigureMapping()
    {
        CreateMap<Wiretap, WiretapDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ReverseMap()
            .MaxDepth(1);
    }
}