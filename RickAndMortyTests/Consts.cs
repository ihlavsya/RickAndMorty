using Moq;
using RickAndMortyAPI.BL;
using RickAndMortyAPI.BL.Interfaces;
using RickAndMortyAPI.BL.Interfaces.Models;

namespace RickAndMortyTests;

public static class Consts
{
    public static string NameRick = "rick";
    public static string NameString = "string";
    public static string NameMixup = "mixup";
    private static Person _person;
    private static PersonDTO _personDto;

    public static Mock<IPersonProvider> GetMockHttpProvider()
    {
        var mockPersonProvider = new Mock<IPersonProvider>();
        mockPersonProvider.Setup(provider => provider.GetPerson(Consts.NameRick))
            .ReturnsAsync(Consts.GetPersonDTO);
        mockPersonProvider.Setup(provider => provider.GetPerson(Consts.NameString))
            .ReturnsAsync((PersonDTO?) null);
        
        mockPersonProvider.Setup(provider => provider.CheckPerson(Consts.NameRick, Consts.NameMixup))
            .ReturnsAsync(true);
        mockPersonProvider.Setup(provider => provider.CheckPerson(Consts.NameRick, Consts.NameString))
            .ReturnsAsync((bool?) null);
        return mockPersonProvider;
    }

    static Consts()
    {
        var _person = new Person()
        {
            Name = "Rick Sanchez",
            Status = "Alive",
            Species = "Human",
            Type = "",
            Gender = "Male",
            Origin = new Origin
            {
                Name = "Earth (C-137)",
                Type = "Planet",
                Dimension = "Dimension C-137",
            },
        };
        
        _personDto = new PersonDTO
        {
            Name = "Rick Sanchez",
            Status = "Alive",
            Species = "Human",
            Type = "",
            Gender = "Male",
            Origin = new OriginUrlDTO
            {
                Name = "Earth (C-137)",
                Url = "url"
            },
            FullOrigin = new FullOriginDTO
            {
                Name = "Earth (C-137)",
                Type = "Planet",
                Dimension = "Dimension C-137"
            }
        };
    }
    public static Person GetPerson()
    {
        return _person;
    }
    
    public static PersonDTO GetPersonDTO()
    {
        return _personDto;
    }

}