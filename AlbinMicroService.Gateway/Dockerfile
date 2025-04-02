# Use .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy only the API Gateway project file to avoid unnecessary dependencies
COPY AlbinMicroService.Gateway/*.csproj ./ 

# Restore dependencies
RUN dotnet restore

# Copy the entire API Gateway source code
COPY AlbinMicroService.Gateway/ ./AlbinMicroService.Gateway/

# Build and publish the API Gateway
WORKDIR /app/AlbinMicroService.Gateway
RUN dotnet publish -c Release -o /app/out

# Use a runtime image for running the published app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Set environment variable
ENV ASPNETCORE_ENVIRONMENT=Development

# Copy the published application from the build stage
COPY --from=build /app/out ./

# Expose both HTTP and HTTPS ports
EXPOSE 9001
EXPOSE 9002

# Run the API Gateway
ENTRYPOINT ["dotnet", "AlbinMicroService.Gateway.dll"]
