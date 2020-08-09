#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["BandwidthMonitorService.Portal/BandwidthMonitorService.Portal.csproj", "BandwidthMonitorService.Portal/"]
COPY ["BandwidthMonitorService.Client/BandwidthMonitorService.Client.csproj", "BandwidthMonitorService.Client/"]
COPY ["BandwidthMonitorService.Dto/BandwidthMonitorService.Dto.csproj", "BandwidthMonitorService.Dto/"]
RUN dotnet restore "BandwidthMonitorService.Portal/BandwidthMonitorService.Portal.csproj"
COPY . .
WORKDIR "/src/BandwidthMonitorService.Portal"
RUN dotnet build "BandwidthMonitorService.Portal.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BandwidthMonitorService.Portal.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BandwidthMonitorService.Portal.dll"]