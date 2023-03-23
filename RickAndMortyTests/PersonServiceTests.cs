using AutoMapper;
using Moq;
using RickAndMortyAPI.BL;
using RickAndMortyAPI.BL.Interfaces;
using RickAndMortyAPI.BL.Interfaces.Models;

namespace RickAndMortyTests;

public class PersonServiceTests
{
    private PersonDTO _expectedPersonDTO;
    private Person _expectedPerson;
    private Mock<IPersonProvider> _mockProvider;
    private IPersonService _personService;
    private Mock<IMapper> _mockMapper;
    private string _nameRick = "rick";
    private string _nameString = "string";
    private string _nameMixup = "mixup";
    public PersonServiceTests()
    {
        _mockProvider = new Mock<IPersonProvider>();
        _mockMapper = new Mock<IMapper>();
        _expectedPerson = GetPerson();
        _expectedPersonDTO = GetPersonDTO();
        _mockProvider.Setup(provider => provider.GetPerson(_nameRick))
            .ReturnsAsync(_expectedPersonDTO);
        
        _mockProvider.Setup(provider => provider.GetPerson(_nameString))
            .ReturnsAsync((PersonDTO?) null);

        _mockProvider.Setup(provider => provider.CheckPerson(_nameRick, _nameMixup))
            .ReturnsAsync(true);
        _mockProvider.Setup(provider => provider.CheckPerson(_nameRick, _nameString))
            .ReturnsAsync((bool?) null);

        _mockMapper.Setup(x => x.Map<Person>(_expectedPersonDTO)).Returns(_expectedPerson);
        _personService = new PersonService(_mockProvider.Object, _mockMapper.Object);
    }

    private Person GetPerson()
    {
        var person = new Person()
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
        return person;
    }
    private PersonDTO GetPersonDTO()
    {
        var personDTO = new PersonDTO
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
        return personDTO;
    }

    [Test]
    public async Task MockConfigurationTest()
    {
        var fromMockObject = await _mockProvider.Object.GetPerson(_nameRick);
        // Demonstrate that the configuration works
        Assert.That(fromMockObject, Is.EqualTo(_expectedPersonDTO));

        // Verify that the mock was invoked
        _mockProvider.Verify(provider => provider.GetPerson(_nameRick));
    }
    
    [Test]
    public async Task CheckPersonTestReturnsTrue()
    {
        var expectedResult = true;

        var actualResult = await _personService.CheckPerson(_nameRick, _nameMixup);
        
        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }
    
    [Test]
    public async Task CheckPersonTestReturnsNull()
    {
        bool? expectedResult = null;

        var actualResult = await _personService.CheckPerson(_nameRick, _nameString);
        
        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }

    [Test]
    public async Task GetPersonTestReturnsFullPerson()
    {
        var expectedResult = _mockMapper.Object.Map<Person>(_expectedPersonDTO);
        
        var actualResult = await _personService.GetPerson(_nameRick);
        
        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }
    
    [Test]
    public async Task GetPersonTestReturnsNull()
    {
        Person? expectedResult = null;
        
        var actualResult = await _personService.GetPerson(_nameString);
        
        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }
}