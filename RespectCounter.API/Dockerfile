# Build stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RespectCounter.API/RespectCounter.API.csproj", "RespectCounter.API/"]
COPY ["RespectCounter.Application/RespectCounter.Application.csproj", "RespectCounter.Application/"]
COPY ["RespectCounter.Domain/RespectCounter.Domain.csproj", "RespectCounter.Domain/"]
COPY ["RespectCounter.Infrastructure/RespectCounter.Infrastructure.csproj", "RespectCounter.Infrastructure/"]
RUN dotnet restore "RespectCounter.API/RespectCounter.API.csproj"
COPY . .
WORKDIR "/src/RespectCounter.API"
RUN dotnet publish "RespectCounter.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RespectCounter.API.dll"]