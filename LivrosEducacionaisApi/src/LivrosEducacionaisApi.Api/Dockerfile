FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

USER app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["LivrosEducacionaisApi/src/LivrosEducacionaisApi.Api/LivrosEducacionaisApi.Api.csproj", "LivrosEducacionaisApi/src/LivrosEducacionaisApi.Api/"]
COPY ["LivrosEducacionaisApi/src/LivrosEducacionaisApi.Application/LivrosEducacionaisApi.Application.csproj", "LivrosEducacionaisApi/src/LivrosEducacionaisApi.Application/"]
COPY ["LivrosEducacionaisApi/src/LivrosEducacionaisApi.Infrastructure/LivrosEducacionaisApi.Infrastructure.csproj", "LivrosEducacionaisApi/src/LivrosEducacionaisApi.Infrastructure/"]
COPY ["LivrosEducacionaisApi/src/LivrosEducacionaisApi.Domain/LivrosEducacionaisApi.Domain.csproj", "LivrosEducacionaisApi/src/LivrosEducacionaisApi.Domain/"]
RUN dotnet restore "LivrosEducacionaisApi/src/LivrosEducacionaisApi.Api/LivrosEducacionaisApi.Api.csproj"
COPY . .
WORKDIR "/src/LivrosEducacionaisApi/src/LivrosEducacionaisApi.Api"
RUN dotnet build "LivrosEducacionaisApi.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LivrosEducacionaisApi.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish --chown=app:app /app/publish .
ENTRYPOINT ["dotnet", "LivrosEducacionaisApi.Api.dll"]
