#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/SimpleServicesDashboard.Api/SimpleServicesDashboard.Api.csproj", "src/SimpleServicesDashboard.Api/"]
RUN dotnet restore "src/SimpleServicesDashboard.Api/SimpleServicesDashboard.Api.csproj"
COPY . .
WORKDIR "/src/src/SimpleServicesDashboard.Api"
RUN dotnet build "SimpleServicesDashboard.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SimpleServicesDashboard.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleServicesDashboard.Api.dll"]
