# Use .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy only the required project files to leverage caching
COPY AlbinMicroService.Users/AlbinMicroService.Users.csproj AlbinMicroService.Users/
COPY AlbinMicroService.Core/AlbinMicroService.Core.csproj AlbinMicroService.Core/

# Restore dependencies
RUN dotnet restore "AlbinMicroService.Users/AlbinMicroService.Users.csproj"

# Copy the full source code
COPY . .

# Build the application
WORKDIR /app/AlbinMicroService.Users
RUN dotnet build -c Release -o /app/build

# Publish the application
RUN dotnet publish -c Release -o /app/publish

# Use runtime image for final deployment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Set ASP.NET Core environment variable
ENV ASPNETCORE_ENVIRONMENT=Development

# Copy the published output from the build stage
COPY --from=build /app/publish .

# Run the application
ENTRYPOINT ["dotnet", "AlbinMicroService.Users.dll"]