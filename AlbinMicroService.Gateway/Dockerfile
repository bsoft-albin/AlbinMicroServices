# Use .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
EXPOSE 80

# Copy only the required project files
COPY ../AlbinMicroService.Gateway/AlbinMicroService.Gateway.csproj AlbinMicroService.Gateway/

# Restore dependencies
RUN dotnet restore "AlbinMicroService.Gateway/AlbinMicroService.Gateway.csproj"

# Copy everything
COPY ../ .

# Build the application
WORKDIR /app/AlbinMicroService.Gateway
RUN dotnet build -c Release -o /app/build

# Publish the application
RUN dotnet publish -c Release -o /app/publish

# Use runtime image for final deployment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Run the application
ENTRYPOINT ["dotnet", "AlbinMicroService.Gateway.dll"]
