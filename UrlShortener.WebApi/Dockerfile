FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src
COPY ["UrlShortener.WebApi/UrlShortener.WebApi.csproj", "UrlShortener.WebApi/"]
RUN dotnet restore "UrlShortener.WebApi/UrlShortener.WebApi.csproj"
COPY . .
WORKDIR "/src/UrlShortener.WebApi"
RUN dotnet build "UrlShortener.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UrlShortener.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UrlShortener.WebApi.dll"]