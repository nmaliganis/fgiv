using AutoMapper;
using erpl.common.dtos.DTOs.Suspects;
using erpl.model.Suspects;

namespace erpl.api.Configurations.AutoMappingProfiles.Suspects;

internal class SuspectToSuspectUiAutoMapperProfile : Profile
{
    public SuspectToSuspectUiAutoMapperProfile()
    {
        ConfigureMapping();
    }

    public void ConfigureMapping()
    {
        CreateMap<Suspect, SuspectDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => src.Firstname))
            .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => src.Lastname))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
            .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => src.Dob))
            .ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.Nationality))
            .ForMember(dest => dest.Calls, opt => opt.MapFrom(src => src.Calls))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
            .ReverseMap()
            .MaxDepth(1);
    }
}