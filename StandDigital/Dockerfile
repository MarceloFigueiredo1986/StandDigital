FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["StandDigital/StandDigital.csproj", "StandDigital/"]
COPY ["StandDigital.Core/StandDigital.Core.csproj", "StandDigital.Core/"]
COPY ["StandDigital_Shared/StandDigital_Shared.csproj", "StandDigital_Shared/"]
COPY ["StandDigital_HttpClient/StandDigital_HttpClient.csproj", "StandDigital_HttpClient/"]
RUN dotnet restore "./StandDigital/StandDigital.csproj"
COPY . .
WORKDIR "/src/StandDigital"
RUN dotnet build "./StandDigital.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./StandDigital.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=true

FROM base AS final
WORKDIR /app
EXPOSE 8081
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StandDigital.dll"]