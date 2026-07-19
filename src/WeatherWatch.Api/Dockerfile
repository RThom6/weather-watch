# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csprojs first so restore is cached until a project file changes.
COPY src/WeatherWatch.Api/WeatherWatch.Api.csproj WeatherWatch.Api/
COPY src/WeatherWatch.Application/WeatherWatch.Application.csproj WeatherWatch.Application/
COPY src/WeatherWatch.Infrastructure/WeatherWatch.Infrastructure.csproj WeatherWatch.Infrastructure/
RUN dotnet restore WeatherWatch.Api/WeatherWatch.Api.csproj

# Copy the rest of the source and publish.
COPY src/ ./
RUN dotnet publish WeatherWatch.Api/WeatherWatch.Api.csproj -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Non-root user plus a writable directory for the SQLite database.
RUN useradd -m appuser && mkdir /data && chown appuser /data
USER appuser

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "WeatherWatch.Api.dll"]
