# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# BUILD - compila o projeto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia nuget.config e pacotes locais (relativos ao contexto definido no docker-compose)
COPY nuget.config ./nuget.config
COPY components/package ./components/package

# Copia os arquivos de projeto
COPY ["src/Authentication.Api/Authentication.Api.csproj", "Authentication.Api/"]
COPY ["src/AuthenticatioApi.Application/AuthenticatioApi.Application.csproj", "AuthenticatioApi.Application/"]
COPY ["src/AuthenticatioApi.Core/AuthenticatioApi.Core.csproj", "AuthenticatioApi.Core/"]
COPY ["src/AuthenticatioApi.Infra.Data/AuthenticatioApi.Infra.Data.csproj", "AuthenticatioApi.Infra.Data/"]
COPY ["src/AuthenticatioApi.Infra.Identity/AuthenticatioApi.Infra.Identity.csproj", "AuthenticatioApi.Infra.Identity/"]
COPY ["src/AuthenticatioApi.Infra.IoC/AuthenticatioApi.Infra.IoC.csproj", "AuthenticatioApi.Infra.IoC/"]
# Restaura pacotes usando o nuget.config com pacote local
RUN dotnet restore "Authentication.Api/Authentication.Api.csproj" --configfile nuget.config
COPY . .
WORKDIR "/src/Authentication.Api"
RUN dotnet build "Authentication.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Authentication.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Authentication.Api.dll"]