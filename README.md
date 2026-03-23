# UrlShortener

Example UrlShortener project using ASP.NET Core

## Getting Started
### Prequisites

- Docker / Rancher / Podman / Any `docker-compose` compatible orchestration framework
- .NET 10
- [Setup dev-certs](https://learn.microsoft.com/en-us/dotnet/core/additional-tools/self-signed-certificates-guide)

### How to run
#### Docker

Run `docker compose -f docker-compose.yml -f docker-compose.override.yml up --detach` in the project root and navigate to
http://localhost:5000/swagger

#### Local development

Run `docker compose -f docker-compose.integration.yml up --detach` in the project root to spin up dependencies then
`dotnet run --project UrlShortener.WebApi --launch-profile WebApi` or run / debug the UrlShortener.WebApi in your favourite IDE.
Navigate to http://localhost:5000/swagger if it does not open automatically.

### Testing

> Ensure the docker daemon is already running before running tests or coverage

Run `dotnet test dotnet test UrlShortener.sln --settings:coverage.runsettings` in the project root.

#### Code coverage
Run `GenerateCoverage.ps1` in the project root to run tests and generate a code coverage report and open the `index.html` file in
`./CoverageReport` too see a full breakdown of code and branch summary in HTML format.