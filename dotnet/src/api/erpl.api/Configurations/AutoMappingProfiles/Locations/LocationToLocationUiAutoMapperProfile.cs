using AutoMapper;
using erpl.common.dtos.DTOs.Locations;
using erpl.model.Locations;

namespace erpl.api.Configurations.AutoMappingProfiles.Locations;

internal class LocationToLocationUiAutoMapperProfile : Profile
{
    public LocationToLocationUiAutoMapperProfile()
    {
        ConfigureMapping();
    }

    public void ConfigureMapping()
    {
        CreateMap<Location, LocationDto>()
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Street))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
            .ForMember(dest => dest.Postcode, opt => opt.MapFrom(src => src.Postcode))
            .ReverseMap()
            .MaxDepth(1);
    }
}//Class : LocationToLocationUiAutoMapperProfile