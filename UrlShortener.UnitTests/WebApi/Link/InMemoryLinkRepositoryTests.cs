using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using UrlShortener.WebApi.Link;
using UrlShortener.WebApi.Util;

namespace UrlShortener.UnitTests.WebApi.Link;

[TestClass]
public sealed class InMemoryLinkRepositoryTests
{
    private Fixture _fixture;
    private UrlShortener.DataAccess.Link _link;
    private Mock<IMemoryCacheWrapper> _mockMemoryCache;
    private Mock<ILinkRepository> _mockInnerLinkRepository;
    private InMemoryLinkRepository _sut;
    
    [TestInitialize]
    public void Initialize()
    {
        _fixture = new Fixture();
        _link = _fixture.Create<UrlShortener.DataAccess.Link>();
        
        _mockMemoryCache = new Mock<IMemoryCacheWrapper>();
        _mockInnerLinkRepository = new Mock<ILinkRepository>();
        
        _sut = new InMemoryLinkRepository(_mockMemoryCache.Object, _mockInnerLinkRepository.Object);
    }
    
    [TestMethod]
    public async Task CreateAsync_WhenCalled_CallsInnerLinkRepository()
    {
        _mockInnerLinkRepository.Setup(x => x.CreateAsync(_link)).ReturnsAsync(_link);
        
        await _sut.CreateAsync(_link);
        
        _mockInnerLinkRepository.Verify(x => x.CreateAsync(_link), Times.Once);
    }

    [TestMethod]
    public async Task GetByIdAsync_WhenLinkIsInCache_ReturnsLinkFromCache()
    {
        _mockMemoryCache.Setup(x => x.TryGetValue(_link.Id, out _link)).Returns(true);
        
        var result = await _sut.GetByIdAsync(_link.Id);
        
        result.Should().BeSameAs(_link);
    }

    [TestMethod]
    public async Task GetByIdAsync_WhenLinkIsNotInCache_ReturnsLinkFromInnerRepository()
    {
        var link = _fixture.Build<UrlShortener.DataAccess.Link>().With(x => x.Id, _link.Id).Create();
        _mockMemoryCache.Setup(x => x.TryGetValue(_link.Id, out _link)).Returns(false);
        _mockInnerLinkRepository.Setup(x => x.GetByIdAsync(_link.Id)).ReturnsAsync(link);
        
        var result = await _sut.GetByIdAsync(_link.Id);
        
        result.Should().BeSameAs(link);
    }
    
    [TestMethod]
    public async Task GetByIdAsync_WhenLinkIsNotInCache_InsertsIntoMemoryCache()
    {
        var link = _fixture.Build<UrlShortener.DataAccess.Link>().With(x => x.Id, _link.Id).Create();
        _mockMemoryCache.Setup(x => x.TryGetValue(_link.Id, out _link)).Returns(false);
        _mockInnerLinkRepository.Setup(x => x.GetByIdAsync(_link.Id)).ReturnsAsync(link);
        
        await _sut.GetByIdAsync(_link.Id);

        _mockMemoryCache.Verify(x => 
            x.Set(link.Id, link, It.IsAny<MemoryCacheEntryOptions>()), Times.Once);
    }

    [TestMethod]
    public async Task UpdateAsync_WhenCalled_CallsInnerLinkRepository()
    {
        await _sut.UpdateAsync(_link);
        
        _mockInnerLinkRepository.Verify(x => x.UpdateAsync(_link), Times.Once);
    }

    [TestMethod]
    public async Task UpdateAsync_WhenLinkDoesNotExist_DoesNotInsertIntoMemoryCache()
    {
        _mockInnerLinkRepository
            .Setup(x => x.UpdateAsync(_link))
            .ReturnsAsync((UrlShortener.DataAccess.Link)null);
        
        await _sut.UpdateAsync(_link);
        
        _mockMemoryCache.Verify(x => 
            x.Set(It.IsAny<object>(),
                It.IsAny<UrlShortener.DataAccess.Link>(),
                It.IsAny<MemoryCacheEntryOptions>()), Times.Never);
    }

    [TestMethod]
    public async Task UpdateAsync_WhenLinkExistsAndInCache_UpdatesMemoryCache()
    {
        _mockInnerLinkRepository.Setup(x => x.UpdateAsync(_link)).ReturnsAsync(_link);
        _mockMemoryCache.Setup(x => x.TryGetValue(_link.Id, out _link)).Returns(true);
        
        await _sut.UpdateAsync(_link);
        
        _mockMemoryCache.Verify(x => 
            x.Set(_link.Id, _link, It.IsAny<MemoryCacheEntryOptions>()), Times.Once);
    }

    [TestMethod]
    public async Task DeleteAsync_WhenCalled_RemovesFromMemoryCache()
    {
        await _sut.DeleteByIdAsync(_link.Id);
        
        _mockMemoryCache.Verify(x => x.Remove(_link.Id), Times.Once);
    }

    [TestMethod]
    public async Task DeleteAsync_WhenCalled_RemovesFromInnerLinkRepository()
    {
        await _sut.DeleteByIdAsync(_link.Id);
        
        _mockInnerLinkRepository.Verify(x => x.DeleteByIdAsync(_link.Id), Times.Once);
    }
}