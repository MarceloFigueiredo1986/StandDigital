FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    clang zlib1g-dev
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["StandDigital_SharedMicroservice/StandDigital_SharedMicroservice.csproj", "StandDigital_SharedMicroservice/"]
RUN dotnet restore "./StandDigital_SharedMicroservice/./StandDigital_SharedMicroservice.csproj"
COPY . .
WORKDIR "/src/StandDigital_SharedMicroservice"
RUN dotnet build "./StandDigital_SharedMicroservice.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./StandDigital_SharedMicroservice.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=true

FROM base AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet","StandDigital_SharedMicroservice.dll"]