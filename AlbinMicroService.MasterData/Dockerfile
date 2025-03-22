# Use .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy only the required project files
COPY ../AlbinMicroService.MasterData/AlbinMicroService.MasterData.csproj AlbinMicroService.MasterData/
COPY ../AlbinMicroService.Core/AlbinMicroService.Core.csproj AlbinMicroService.Core/

# Restore dependencies
RUN dotnet restore "AlbinMicroService.MasterData/AlbinMicroService.MasterData.csproj"

# Copy everything
COPY ../ .

# Build the application
WORKDIR /app/AlbinMicroService.MasterData
RUN dotnet build -c Release -o /app/build

# Publish the application
RUN dotnet publish -c Release -o /app/publish

# Use runtime image for final deployment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Run the application
ENTRYPOINT ["dotnet", "AlbinMicroService.MasterData.dll"]
