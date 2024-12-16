# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG URL_SUPERHERO
ARG TOKEN_SUPERHERO
ENV URL_SUPERHERO_API=$URL_SUPERHERO
ENV TOKEN_SUPERHERO_API=$TOKEN_SUPERHERO
WORKDIR /src

COPY ["SuperHero.Api/SuperHero.Api.csproj", "SuperHero.Api/"]
COPY ["SuperHero.Service/SuperHero.Service.csproj", "SuperHero.Service/"]
RUN dotnet restore "SuperHero.Api/SuperHero.Api.csproj"
COPY . .
WORKDIR "/src/SuperHero.Api"
RUN dotnet build "SuperHero.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SuperHero.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV URL_SUPERHERO_API=$URL_SUPERHERO
ENV TOKEN_SUPERHERO_API=$TOKEN_SUPERHERO
ENTRYPOINT ["dotnet", "SuperHero.Api.dll"]
