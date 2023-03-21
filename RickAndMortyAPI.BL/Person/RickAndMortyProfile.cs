using AutoMapper;
using RickAndMortyAPI.BL.Interfaces.Models;

namespace RickAndMortyAPI.BL;

public class RickAndMortyProfile : Profile
{
    public RickAndMortyProfile()
    {
        CreateMap<PersonDTO, Person>().
            ForMember(p => p.Origin, b => b.MapFrom(pd => pd.FullOrigin));
        CreateMap<FullOriginDTO, Origin>();
    }
}