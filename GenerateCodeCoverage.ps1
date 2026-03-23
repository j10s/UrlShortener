if (Test-Path .\UrlShortener.IntegrationTests\TestResults) {
    Remove-Item -Recurse -Force .\UrlShortener.IntegrationTests\TestResults
}
if (Test-Path .\UrlShortener.IntegrationTests\TestResults) {
    Remove-Item -Recurse -Force .\UrlShortener.UnitTests\TestResults
}

dotnet test UrlShortener.sln --collect:"XPlat Code Coverage" --settings:coverage.runsettings
reportgenerator -reports:./**/coverage.cobertura.xml -targetdir:./CoverageReport -reporttypes:Html