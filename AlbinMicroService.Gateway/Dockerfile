# Use official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything and build
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Use runtime image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Set ASP.NET Core environment variable
ENV ASPNETCORE_ENVIRONMENT=Development

COPY --from=build /app/out ./

# Expose both HTTP and HTTPS
EXPOSE 9001
EXPOSE 9002

# Run the Gateway API
ENTRYPOINT ["dotnet", "AlbinMicroService.Gateway.dll"]