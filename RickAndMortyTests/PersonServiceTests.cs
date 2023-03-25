using AutoMapper;
using Moq;
using RickAndMortyAPI.BL;
using RickAndMortyAPI.BL.Externals;
using RickAndMortyAPI.BL.Interfaces;
using RickAndMortyAPI.BL.Interfaces.Models;
using RickAndMortyTests;

namespace RickAndMortyTests;

public class PersonServiceTests
{
    private Mock<IPersonProvider> _mockProvider;
    private IPersonService _personService;
    private Mock<IMapper> _mockMapper;
    public PersonServiceTests()
    {
        _mockProvider = Consts.GetMockHttpProvider();

        _mockMapper = new Mock<IMapper>();
        _mockMapper.Setup(x => x.Map<Person>(Consts.GetPersonDTO())).Returns(Consts.GetPerson);
        _personService = new PersonService(_mockProvider.Object, _mockMapper.Object);
    }
    
    [Test]
    public async Task MockConfigurationTest()
    {
        var fromMockObject = await _mockProvider.Object.GetPerson(Consts.NameRick);
        // Demonstrate that the configuration works
        Assert.That(fromMockObject, Is.EqualTo(Consts.GetPersonDTO()));

        // Verify that the mock was invoked
        _mockProvider.Verify(provider => provider.GetPerson(Consts.NameRick));
    }
    
    [Test]
    public async Task CheckPersonTestReturnsTrue()
    {
        var expectedResult = true;

        var actualResult = await _personService.CheckPerson(Consts.NameRick, Consts.NameMixup);
        
        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }
    
    [Test]
    public async Task CheckPersonTestReturnsNull()
    {
        bool? expectedResult = null;

        var actualResult = await _personService.CheckPerson(Consts.NameRick, Consts.NameString);
        
        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }

    [Test]
    public async Task GetPersonTestReturnsFullPerson()
    {
        var expectedResult = _mockMapper.Object.Map<Person>(Consts.GetPersonDTO());
        
        var actualResult = await _personService.GetPerson(Consts.NameRick);
        
        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }
    
    [Test]
    public async Task GetPersonTestReturnsNull()
    {
        Person? expectedResult = null;
        
        var actualResult = await _personService.GetPerson(Consts.NameString);
        
        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }
}