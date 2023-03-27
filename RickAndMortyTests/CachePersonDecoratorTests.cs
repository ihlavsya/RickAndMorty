using Microsoft.Extensions.Caching.Memory;
using Moq;
using RickAndMortyAPI.BL.Externals;
using RickAndMortyAPI.BL.Interfaces;

namespace RickAndMortyTests;

public class CachePersonDecoratorTests
{
    private readonly IPersonProvider _cachePersonProvider;
    private Mock<IPersonProvider> _mockHttpPersonProvider;
    public CachePersonDecoratorTests()
    {
        _mockHttpPersonProvider = Consts.GetMockHttpProvider();
        _cachePersonProvider = new CachePersonDecorator(_mockHttpPersonProvider.Object,
            new MemoryCache(new MemoryCacheOptions()));
    }
    
    [Test]
    public async Task CheckPersonTestReturnsTrue()
    {
        var expectedResult = true;
        var actualResult = await _cachePersonProvider.CheckPerson(Consts.NameRick, Consts.NameMixup);

        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }

    [Test]
    public async Task CheckPersonTestIfCache()
    {
        var firstCall = await _cachePersonProvider.CheckPerson(Consts.NameRick, Consts.NameMixup);
        var secondCall = await _cachePersonProvider.CheckPerson(Consts.NameRick, Consts.NameMixup);
        var thirdCall = await _cachePersonProvider.CheckPerson(Consts.NameRick, Consts.NameMixup);
        _mockHttpPersonProvider.Verify(mp => mp.CheckPerson(Consts.NameRick, Consts.NameMixup), Times.Once);
    }
    
    
    [Test]
    public async Task GetPersonTestIfCache()
    {
        var firstCall = await _cachePersonProvider.GetPerson(Consts.NameRick);
        var secondCall = await _cachePersonProvider.GetPerson(Consts.NameRick);
        var thirdCall = await _cachePersonProvider.GetPerson(Consts.NameRick);
        _mockHttpPersonProvider.Verify(mp => mp.GetPerson(Consts.NameRick), Times.Once);
    }
}