using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using UrlShortener.DataAccess;

namespace UrlShortener.UnitTests.DataAccess;

[TestClass]
public sealed class MigrationRunnerTests
{
    private Fixture _fixture;
    private Mock<IConfiguration> _mockConfiguration;
    private Mock<IConfigurationSection> _mockConfigurationSection;

    [TestInitialize]
    public void TestInitialize()
    {
        _fixture = new Fixture();
        _mockConfiguration = new Mock<IConfiguration>();
        _mockConfigurationSection = new Mock<IConfigurationSection>();

        _mockConfiguration
            .Setup(x => x.GetSection("ConnectionStrings"))
            .Returns(_mockConfigurationSection.Object);
    }
    
    [TestMethod]
    public void Initialize_WhenMasterConnectionStringIsNull_UsesDefaultConnectionString()
    {
        var connectionString = _fixture.Create<string>();
        
        _mockConfigurationSection.SetupGet(x => x["Master"]).Returns((string)null);
        _mockConfigurationSection.SetupGet(x => x["Default"]).Returns(connectionString);
        var act = () => MigrationRunner.Initialize(_mockConfiguration.Object);

        act.Should().NotThrow();
    }

    [TestMethod]
    public void Initialize_WhenMasterAndDefaultConnectionStringsAreNull_ThrowsException()
    {
        _mockConfigurationSection.SetupGet(x => x["Master"]).Returns((string)null);
        _mockConfigurationSection.SetupGet(x => x["Default"]).Returns((string)null);
        var act = () => MigrationRunner.Initialize(_mockConfiguration.Object);
        
        act.Should().Throw<Exception>()
            .WithMessage("Either Master or Default ConnectionString(s) must be provided in configuration");
    }
}