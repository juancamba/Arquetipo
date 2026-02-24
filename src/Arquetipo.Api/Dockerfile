FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

# ENV ASPNETCORE_URLS=http://+:5000

# USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/Arquetipo.Domain/Arquetipo.Domain.csproj", "src/Arquetipo.Domain/"]
COPY ["src/Arquetipo.Infrastructure/Arquetipo.Infrastructure.csproj", "src/Arquetipo.Infrastructure/"]
COPY ["src/Arquetipo.Application/Arquetipo.Application.csproj", "src/Arquetipo.Application/"]
COPY ["src/Arquetipo.Api/Arquetipo.Api.csproj", "src/Arquetipo.Api/"]
RUN dotnet restore "src/Arquetipo.Api/Arquetipo.Api.csproj"
COPY . .
WORKDIR "/src/src/Arquetipo.Api"
RUN dotnet build "Arquetipo.Api.csproj" -c $configuration  -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Arquetipo.Api.csproj" -c $configuration  -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Arquetipo.Api.dll"]
