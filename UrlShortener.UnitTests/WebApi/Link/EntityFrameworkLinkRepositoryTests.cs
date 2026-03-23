using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using UrlShortener.DataAccess;
using UrlShortener.WebApi.Link;

namespace UrlShortener.UnitTests.WebApi.Link;

[TestClass]
public sealed class EntityFrameworkLinkRepositoryTests
{
    private Fixture _fixture;
    private DataContext _db;
    private EntityFrameworkLinkRepository _sut;
    
    [TestInitialize]
    public void Initialize()
    {
        _fixture = new Fixture();
        
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        
        var contextOptions = new DbContextOptionsBuilder<DataContext>()
            .UseSqlite(connection)
            .Options;
        
        _db =  new DataContext(contextOptions);
        _db.Database.EnsureCreated();
        _sut = new EntityFrameworkLinkRepository(_db);
    }

    [TestMethod]
    public async Task CreateAsync_WhenCalled_ReturnsLinkWithId()
    {
        var link = _fixture.Build<UrlShortener.DataAccess.Link>().Without(x => x.Id).Create();
        
        var result = await _sut.CreateAsync(link);

        result.Id.Should().Be(1);
    }

    [TestMethod]
    public async Task GetByIdAsync_WhenLinkDoesntExist_ReturnsNull()
    {
        var id = _fixture.Create<long>();
        
        var result = await _sut.GetByIdAsync(id);
        
        result.Should().BeNull();
    }

    [TestMethod]
    public async Task GetByIdAsync_WhenLinkExists_ReturnsLink()
    {
        var link = _fixture.Build<UrlShortener.DataAccess.Link>().Without(x => x.Id).Create();
        var createdLink = await _sut.CreateAsync(link);
        
        var result = await _sut.GetByIdAsync(createdLink.Id);
        
        result.Should().BeEquivalentTo(createdLink);
    }

    [TestMethod]
    public async Task UpdateAsync_WhenLinkDoesntExist_ReturnsNull()
    {
        var link = _fixture.Create<UrlShortener.DataAccess.Link>();
        
        var result = await _sut.UpdateAsync(link);
        
        result.Should().BeNull();
    }

    [TestMethod]
    public async Task UpdateAsync_WhenLinkExists_ReturnsUpdatedLink()
    {
        var link = _fixture.Build<UrlShortener.DataAccess.Link>().Without(x => x.Id).Create();
        var createdLink = await _sut.CreateAsync(link);
        var updatedLink = _fixture.Build<UrlShortener.DataAccess.Link>()
            .With(x => x.Id, createdLink.Id).Create();
        
        var result = await _sut.UpdateAsync(updatedLink);

        using (new AssertionScope())
        {
            result.Id.Should().Be(createdLink.Id);
            result.TargetUri.Should().Be(updatedLink.TargetUri);
            result.CreatedAt.Should().Be(createdLink.CreatedAt);
            result.UpdatedAt.Should().Be(updatedLink.UpdatedAt);
        }
    }

    [TestMethod]
    public async Task DeleteByIdAsync_WhenLinkDoesntExist_ReturnsFalse()
    {
        var id = _fixture.Create<long>();
        
        var result = await _sut.DeleteByIdAsync(id);
        
        result.Should().BeFalse();
    }
    
    [TestMethod]
    public async Task DeleteByIdAsync_WhenLinkExists_ReturnsTrue()
    {
        var link = _fixture.Build<UrlShortener.DataAccess.Link>().Without(x => x.Id).Create();
        var createdLink = await _sut.CreateAsync(link);
        
        var result = await _sut.DeleteByIdAsync(createdLink.Id);
        
        result.Should().BeTrue();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _db.Database.EnsureDeleted();
        _db.Dispose();
    }
}