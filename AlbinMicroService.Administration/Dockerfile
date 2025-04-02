# Use .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy only the required project files to leverage caching
#COPY AlbinMicroService.Administration/AlbinMicroService.Administration.csproj AlbinMicroService.Administration/
#COPY AlbinMicroService.Core/AlbinMicroService.Core.csproj AlbinMicroService.Core/

# Copy only the project files first to leverage Docker cache
COPY ./AlbinMicroService.Administration/AlbinMicroService.Administration.csproj ./
COPY ./AlbinMicroService.Core/AlbinMicroService.Core.csproj ./

# Restore dependencies
#RUN dotnet restore "AlbinMicroService.Administration/AlbinMicroService.Administration.csproj"
RUN dotnet restore

# Copy everything else (source code)
COPY ./AlbinMicroService.Administration/ ./AlbinMicroService.Administration/
COPY ./AlbinMicroService.Core/ ./AlbinMicroService.Core/

# Copy the full source code
COPY . .

# Build the application
WORKDIR /app/AlbinMicroService.Administration
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
ENTRYPOINT ["dotnet", "AlbinMicroService.Administration.dll"]

