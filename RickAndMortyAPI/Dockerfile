FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RickAndMortyAPI/RickAndMortyAPI.csproj", "RickAndMortyAPI/"]
COPY ["RickAndMortyAPI.BL/RickAndMortyAPI.BL.csproj", "RickAndMortyAPI.BL/"]
RUN dotnet restore "RickAndMortyAPI/RickAndMortyAPI.csproj"
COPY . .
WORKDIR "/src/RickAndMortyAPI"
RUN dotnet build "RickAndMortyAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RickAndMortyAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RickAndMortyAPI.dll"]
