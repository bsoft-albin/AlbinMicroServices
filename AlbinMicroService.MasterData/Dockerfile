# ---- BUILD STAGE ----
# Use .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY AlbinMicroService.MasterData/AlbinMicroService.MasterData.csproj AlbinMicroService.MasterData/
COPY AlbinMicroService.Kernel/AlbinMicroService.Kernel.csproj AlbinMicroService.Kernel/
COPY AlbinMicroService.Libraries/AlbinMicroService.Libraries.csproj AlbinMicroService.Libraries/
COPY AlbinMicroService.Core/AlbinMicroService.Core.csproj AlbinMicroService.Core/

# Restore dependencies
RUN dotnet restore AlbinMicroService.MasterData/AlbinMicroService.MasterData.csproj

# Copy everything else and build
COPY . .
WORKDIR /app/AlbinMicroService.MasterData
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

# ---- RUNTIME STAGE ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Development

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "AlbinMicroService.MasterData.dll"]