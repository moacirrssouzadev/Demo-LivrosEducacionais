FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

USER app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["LivrosEducacionaisWeb/src/LivrosEducacionaisWeb.csproj", "LivrosEducacionaisWeb/src/"]
RUN dotnet restore "LivrosEducacionaisWeb/src/LivrosEducacionaisWeb.csproj"
COPY . .
WORKDIR "/src/LivrosEducacionaisWeb/src"
RUN dotnet build "LivrosEducacionaisWeb.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LivrosEducacionaisWeb.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish --chown=app:app /app/publish .
ENTRYPOINT ["dotnet", "LivrosEducacionaisWeb.dll"]
