using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Moq;
using UrlShortener.WebApi.Util;

namespace UrlShortener.UnitTests.WebApi.Util;

[TestClass]
public class WebHostEnvironmentExtensionsTests
{
    private Fixture _fixture;
    private Mock<IWebHostEnvironment> _mockWebHostEnvironment;
    
    [TestInitialize]
    public void Initialize()
    {
        _fixture = new Fixture();
        _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
    }

    [TestMethod]
    public void IsDevelopmentOrIntegration_WhenDevelopment_ReturnsTrue()
    {
        _mockWebHostEnvironment.SetupGet(x => x.EnvironmentName).Returns("Development");
        
        var result = _mockWebHostEnvironment.Object.IsDevelopmentOrIntegration();

        result.Should().BeTrue();
    }
    
    [TestMethod]
    public void IsDevelopmentOrIntegration_WhenIntegration_ReturnsTrue()
    {
        _mockWebHostEnvironment.SetupGet(x => x.EnvironmentName).Returns("Integration");
        
        var result = _mockWebHostEnvironment.Object.IsDevelopmentOrIntegration();

        result.Should().BeTrue();
    }

    [TestMethod]
    public void IsDevelopmentOrIntegration_WhenNeither_ReturnsFalse()
    {
        var environmentName = _fixture.Create<string>();
        _mockWebHostEnvironment.SetupGet(x => x.EnvironmentName).Returns(environmentName);
        
        var result = _mockWebHostEnvironment.Object.IsDevelopmentOrIntegration();

        result.Should().BeFalse();
    }
}