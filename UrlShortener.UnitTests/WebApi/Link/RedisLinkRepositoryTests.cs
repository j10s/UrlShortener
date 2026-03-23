using AutoFixture;
using FluentAssertions;
using Moq;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;
using UrlShortener.WebApi.Link;

namespace UrlShortener.UnitTests.WebApi.Link;

[TestClass]
public sealed class RedisLinkRepositoryTests
{
    private Fixture _fixture;
    private UrlShortener.DataAccess.Link _link;
    private Mock<IRedisDatabase> _mockRedis;
    private Mock<ILinkRepository> _mockInnerLinkRepository;
    private RedisLinkRepository _sut;

    [TestInitialize]
    public void Initialize()
    {
        _fixture = new Fixture();
        _link = _fixture.Create<UrlShortener.DataAccess.Link>();
        
        _mockRedis = new Mock<IRedisDatabase>();
        _mockInnerLinkRepository = new Mock<ILinkRepository>();
        
        _sut = new RedisLinkRepository(_mockRedis.Object, _mockInnerLinkRepository.Object);
    }

    [TestMethod]
    public async Task CreateAsync_WhenCalled_CallsInnerLinkRepository()
    {
        _mockInnerLinkRepository.Setup(x => x.CreateAsync(_link)).ReturnsAsync(_link);
        
        await _sut.CreateAsync(_link);
        
        _mockInnerLinkRepository.Verify(x => x.CreateAsync(_link), Times.Once);
    }

    [TestMethod]
    public async Task GetByIdAsync_WhenLinkIsInRedis_ReturnsLinkFromRedis()
    {
        _mockRedis
            .Setup(x => x.GetAsync<UrlShortener.DataAccess.Link>(_link.Id.ToString()))
            .ReturnsAsync(_link);
        
        var result = await _sut.GetByIdAsync(_link.Id);

        result.Should().BeSameAs(_link);
    }

    [TestMethod]
    public async Task GetByIdAsync_WhenLinkIsNotInRedis_ReturnsLinkFromInnerRepository()
    {
        var link = _fixture.Build<UrlShortener.DataAccess.Link>().With(x => x.Id, _link.Id).Create();
        _mockInnerLinkRepository.Setup(x => x.GetByIdAsync(_link.Id)).ReturnsAsync(link);
        _mockRedis
            .Setup(x => x.GetAsync<UrlShortener.DataAccess.Link>(_link.Id.ToString()))
            .ReturnsAsync((UrlShortener.DataAccess.Link)null);
        
        var result = await _sut.GetByIdAsync(_link.Id);

        result.Should().BeSameAs(link);
    }

    [TestMethod]
    public async Task GetByIdAsync_WhenLinkIsNotInRedis_InsertsIntoRedis()
    {
        var link = _fixture.Build<UrlShortener.DataAccess.Link>().With(x => x.Id, _link.Id).Create();
        _mockInnerLinkRepository.Setup(x => x.GetByIdAsync(_link.Id)).ReturnsAsync(link);
        _mockRedis
            .Setup(x => x.GetAsync<UrlShortener.DataAccess.Link>(_link.Id.ToString()))
            .ReturnsAsync((UrlShortener.DataAccess.Link)null);
        
        await _sut.GetByIdAsync(_link.Id);
        
        _mockRedis.Verify(x =>
            x.AddAsync(link.Id.ToString(),
                link,
                It.IsAny<When>(),
                CommandFlags.FireAndForget,
                null), Times.Once);
    }
    
    [TestMethod]
    public async Task UpdateAsync_WhenCalled_CallsInnerLinkRepository()
    {
        await _sut.UpdateAsync(_link);
        
        _mockInnerLinkRepository.Verify(x => x.UpdateAsync(_link), Times.Once);
    }

    [TestMethod]
    public async Task UpdateAsync_WhenLinkDoesNotExist_DoesNotInsertIntoRedis()
    {
        _mockInnerLinkRepository
            .Setup(x => x.UpdateAsync(_link))
            .ReturnsAsync((UrlShortener.DataAccess.Link)null);
        
        await _sut.UpdateAsync(_link);
        
        _mockRedis.Verify(x =>
            x.AddAsync(It.IsAny<string>(),
                It.IsAny<UrlShortener.DataAccess.Link>(),
                It.IsAny<When>(),
                CommandFlags.FireAndForget,
                null), Times.Never);
    }

    [TestMethod]
    public async Task UpdateAsync_WhenLinkExistsAndInRedis_UpdatesRedis()
    {
        _mockInnerLinkRepository.Setup(x => x.UpdateAsync(_link)).ReturnsAsync(_link);
        _mockRedis
            .Setup(x => x.GetAsync<UrlShortener.DataAccess.Link>(_link.Id.ToString()))
            .ReturnsAsync(_link);
        
        await _sut.UpdateAsync(_link);
        
        _mockRedis.Verify(x =>
            x.ReplaceAsync(_link.Id.ToString(),
                _link,
                When.Exists,
                CommandFlags.FireAndForget), Times.Once);
    }

    [TestMethod]
    public async Task DeleteAsync_WhenCalled_RemovesFromRedis()
    {
        await _sut.DeleteByIdAsync(_link.Id);

        _mockRedis.Verify(x =>
            x.RemoveAsync(_link.Id.ToString(), CommandFlags.FireAndForget), Times.Once);
    }

    [TestMethod]
    public async Task DeleteAsync_WhenCalled_RemovesFromInnerLinkRepository()
    {
        await _sut.DeleteByIdAsync(_link.Id);
        
        _mockInnerLinkRepository.Verify(x => x.DeleteByIdAsync(_link.Id), Times.Once);
    }
}