using AutoMapper;
using RickAndMortyAPI.BL.Interfaces.Models;

namespace RickAndMortyAPI.BL;

public class RickAndMortyProfile : Profile
{
    public RickAndMortyProfile()
    {
        CreateMap<PersonDTO, Person>()
            .ForSourceMember(sourceMember => sourceMember.Origin, opt => opt.DoNotValidate())
            .ForMember(destinationMember => destinationMember.Origin, opt => opt.MapFrom(pd => pd.FullOrigin));
        CreateMap<FullOriginDTO, Origin>();
    }
}